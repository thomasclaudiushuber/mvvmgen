// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MvvmGen.Events;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace MvvmGen.SourceGenerators
{
    [UsesVerify]
    public class ViewModelGeneratorTestsBase
    {
        static PortableExecutableReference[] metadataReferences;

        static ViewModelGeneratorTestsBase()
        {
#if(!MVVMGEN_PURE_CODE_GENERATION)
            //Endure MvvmGen is loaded
            System.Reflection.Assembly.Load("MvvmGen");
#endif

            metadataReferences = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .ToArray();

            VerifierSettings.NameForParameter<string>(parameter => {
                return parameter
                    .Replace(" ", "")
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("\"", "");
            });
            VerifierSettings.DerivePathInfo((sourceFile, _, type, method) =>
            {
                var directoryName = Path.GetDirectoryName(sourceFile)!;
                var methodName = method.Name.Replace("Generate", "");
                return new(directoryName, type.Name, methodName);
            });
            var version = typeof(IEventAggregator).Assembly.GetName().Version!.ToString(3);
            VerifierSettings.AddScrubber(builder => builder.Replace(version, "TheVersion"));
        }

        protected static SettingsTask VerifyGenerateCode(string inputCode, [CallerFilePath] string sourceFile = "")
        {
            var inputCompilation = CreateCompilation(inputCode, metadataReferences);

#if MVVMGEN_PURE_CODE_GENERATION
            ViewModelAndLibraryGenerator generator = new();
#else
            ViewModelGenerator generator = new();
#endif
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            var runResult = driver.GetRunResult();

            var generatorResult = runResult.Results[0];
            var target = generatorResult.GeneratedSources.Select(x => x.SourceText.ToString()).ToList();
            if (target.Count > 1)
            {
                return Verifier.Verify(target, null, sourceFile);
            }

            return Verifier.Verify(target.FirstOrDefault(), null, sourceFile);
        }

        protected static void ShouldGenerateExpectedCode(string inputCode, params string[] expectedGeneratedCode)
        {
            var inputCompilation = CreateCompilation(inputCode, metadataReferences);

#if MVVMGEN_PURE_CODE_GENERATION
            ViewModelAndLibraryGenerator generator = new();
#else
            ViewModelGenerator generator = new();
#endif
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            var runResult = driver.GetRunResult();

            //Assert.Equal(expectedGeneratedCode.Length, runResult.GeneratedTrees.Length);
            //Assert.True(runResult.Diagnostics.IsEmpty);

            var generatorResult = runResult.Results[0];
            //Assert.Equal(generator, generatorResult.Generator);
            //Assert.True(generatorResult.Diagnostics.IsEmpty);
            //Assert.Equal(expectedGeneratedCode.Length, generatorResult.GeneratedSources.Length);
            //Assert.Null(generatorResult.Exception);

            foreach (var expectedCode in expectedGeneratedCode)
            {
                var found = generatorResult.GeneratedSources.Any(x => x.SourceText.ToString() == expectedCode);

                if (!found)
                {
                    Console.WriteLine($"Expected code not found: {expectedCode}");
                    Console.WriteLine("Generated sources:");
                    foreach (var generatedSourceResult in generatorResult.GeneratedSources)
                    {
                        Console.WriteLine(generatedSourceResult.SourceText.ToString());
                    }
                }

                Assert.True(found, "Expected code must be in generated sources");
            }
        }

        protected static Compilation CreateCompilation(string source, MetadataReference[] metadataReferences)
                => CSharpCompilation.Create("compilation",
                    new[] { CSharpSyntaxTree.ParseText(source) },
                    metadataReferences,
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


        protected static string AutoGeneratedComment {
            get {
#if MVVMGEN_PURE_CODE_GENERATION
                var generatorType = typeof(ViewModelAndLibraryGenerator);
#else
                var generatorType = typeof(ViewModelGenerator);
#endif

                var comment = $@"// <auto-generated>
//   This code was generated for you by
//   ⚡ MvvmGen, a tool created by Thomas Claudius Huber (https://www.thomasclaudiushuber.com)
//   Generator version: { generatorType.Assembly.GetName().Version?.ToString(3) }
// </auto-generated>";
                return comment;
            }
        }

        protected static string AutoGeneratedUsings => $@"using MvvmGen.Commands;
using MvvmGen.Events;
using MvvmGen.ViewModels;";
    }
}
