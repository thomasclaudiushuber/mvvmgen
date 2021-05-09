// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.SourceGenerators.Model
{
    public class ViewModelFactoryToGenerate
    {
        public ViewModelFactoryToGenerate(string className, string interfaceName)
        {
            ClassName = className;
            InterfaceName = interfaceName;
        }

        public string ClassName { get; }

        public string InterfaceName { get; }
    }
}
