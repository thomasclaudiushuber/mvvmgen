// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;

namespace MvvmGen.Model
{
    /// <summary>
    /// Contains all the details that must be generated for a class that is decorated with the MvvmGen.ViewModelAttribute.
    /// </summary>
    internal class ViewModelToGenerate
    {
        public ViewModelToGenerate(string className, string namespaceName)
        {
            ClassName = className;
            NamespaceName = namespaceName;
        }

        public string ClassName { get; }

        public string? ClassAccessModifier { get; set; }

        public string NamespaceName { get; }

        public string? WrappedModelType { get; set; }

        public bool IsEventSubscriber { get; set; }

        public bool GenerateConstructor { get; set; }

        public bool InheritFromViewModelBase { get; set; }

        public IEnumerable<CommandToGenerate>? CommandsToGenerate { get; set; }

        public IDictionary<string, List<string>>? CommandsToInvalidateByPropertyName { get; set; }

        public IList<PropertyToGenerate>? PropertiesToGenerate { get; set; }

        public IEnumerable<InjectionToGenerate>? InjectionsToGenerate { get; set; }

        public ViewModelFactoryToGenerate? ViewModelFactoryToGenerate { get; set; }

        public ViewModelInterfaceToGenerate? ViewModelInterfaceToGenerate { get; set; }
    }
}
