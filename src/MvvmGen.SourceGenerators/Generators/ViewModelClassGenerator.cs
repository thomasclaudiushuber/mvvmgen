// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using MvvmGen.Model;

namespace MvvmGen.Generators
{
    internal static class ClassGenerator
    {
        internal static void GenerateClass(this ViewModelBuilder vmBuilder, ViewModelToGenerate viewModelToGenerate)
        {
            vmBuilder.Append($"partial class {viewModelToGenerate.ClassName}");

            if (viewModelToGenerate.InheritFromViewModelBase)
            {
                vmBuilder.Append(" : global::MvvmGen.ViewModels.ViewModelBase");
            }

            if (viewModelToGenerate.ViewModelInterfaceToGenerate is not null)
            {
                if (viewModelToGenerate.InheritFromViewModelBase)
                {
                    vmBuilder.Append(", ");
                }
                else
                {
                    vmBuilder.Append(" : ");
                }

                vmBuilder.Append(viewModelToGenerate.ViewModelInterfaceToGenerate.InterfaceName);
            }

            vmBuilder.AppendLine();
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
        }
    }
}
