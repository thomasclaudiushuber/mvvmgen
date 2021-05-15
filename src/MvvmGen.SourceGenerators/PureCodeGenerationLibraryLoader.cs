// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#if MVVMGEN_PURE_CODE_GENERATION

using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace MvvmGen
{
    internal static class PureCodeGenerationLibraryLoader
    {
        internal static void AddLibraryFilesToContext(GeneratorPostInitializationContext context)
        {
            var libraryPath = "MvvmGen.SourceGenerators.MvvmGenLib";
            var assembly = typeof(PureCodeGenerationLibraryLoader).Assembly;
            var embeddedLibraryCodeFiles = assembly.GetManifestResourceNames().Where(x => x.StartsWith(libraryPath));

            foreach (var codeFile in embeddedLibraryCodeFiles)
            {
                var codeFileContent = GetContentOfEmbeddedResource(assembly, codeFile);
                var fileNameHint = codeFile.Replace(libraryPath, "MvvmGen").Replace(".cs", ".g.cs");
                context.AddSource(fileNameHint, codeFileContent);
            }
        }

        private static string GetContentOfEmbeddedResource(Assembly assembly, string resourceName)
        {
            using var resourceStream = assembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(resourceStream);
            return streamReader.ReadToEnd();
        }
    }
}

#endif
