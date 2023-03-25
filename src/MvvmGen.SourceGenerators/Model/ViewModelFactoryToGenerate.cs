// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

namespace MvvmGen.Model
{
    internal class ViewModelFactoryToGenerate
    {
        public ViewModelFactoryToGenerate(string className, string interfaceName, string? customReturnType)
        {
            ClassName = className;
            InterfaceName = interfaceName;
            CustomReturnType = customReturnType;
        }

        public string ClassName { get; }

        public string InterfaceName { get; }

        public string? CustomReturnType { get; }
    }
}
