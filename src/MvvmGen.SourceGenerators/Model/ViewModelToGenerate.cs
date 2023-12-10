// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using System.Collections.Generic;
using MvvmGen.SourceGenerators.Extensions;

namespace MvvmGen.Model
{
    /// <summary>
    /// Contains all the details that must be generated for a class that is decorated with the MvvmGen.ViewModelAttribute.
    /// </summary>
    internal class ViewModelToGenerate : IEquatable<ViewModelToGenerate?>
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

        public string? WrappedModelPropertyName { get; set; }

        public bool IsEventSubscriber { get; set; }

        public bool GenerateConstructor { get; set; }

        public bool InheritFromViewModelBase { get; set; }

        public IEnumerable<CommandToGenerate>? CommandsToGenerate { get; set; }

        public IEnumerable<CommandInvalidationToGenerate>? CommandInvalidationsToGenerate { get; set; }

        public IList<PropertyToGenerate>? PropertiesToGenerate { get; set; }

        public IEnumerable<InjectionToGenerate>? InjectionsToGenerate { get; set; }

        public IEnumerable<InjectionToGenerate>? BaseClassInjectionsToGenerate { get; set; }

        public FactoryToGenerate? ViewModelFactoryToGenerate { get; set; }

        public InterfaceToGenerate? ViewModelInterfaceToGenerate { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ViewModelToGenerate);
        }

        public bool Equals(ViewModelToGenerate? other)
        {
            return other is not null &&
                   ClassName == other.ClassName &&
                   ClassAccessModifier == other.ClassAccessModifier &&
                   NamespaceName == other.NamespaceName &&
                   WrappedModelType == other.WrappedModelType &&
                   WrappedModelPropertyName == other.WrappedModelPropertyName &&
                   IsEventSubscriber == other.IsEventSubscriber &&
                   GenerateConstructor == other.GenerateConstructor &&
                   InheritFromViewModelBase == other.InheritFromViewModelBase &&
                   EqualityComparer<FactoryToGenerate?>.Default.Equals(ViewModelFactoryToGenerate, other.ViewModelFactoryToGenerate) &&
                   EqualityComparer<InterfaceToGenerate?>.Default.Equals(ViewModelInterfaceToGenerate, other.ViewModelInterfaceToGenerate) &&
                   CommandsToGenerate.SequenceEqualWithNullCheck(other.CommandsToGenerate) &&
                   CommandInvalidationsToGenerate.SequenceEqualWithNullCheck(other.CommandInvalidationsToGenerate) &&
                   PropertiesToGenerate.SequenceEqualWithNullCheck(other.PropertiesToGenerate) &&
                   InjectionsToGenerate.SequenceEqualWithNullCheck(other.InjectionsToGenerate) &&
                   BaseClassInjectionsToGenerate.SequenceEqualWithNullCheck(other.BaseClassInjectionsToGenerate);
        }

        public override int GetHashCode()
        {
            var hashCode = -147933575;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClassName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(ClassAccessModifier);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NamespaceName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(WrappedModelType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(WrappedModelPropertyName);
            hashCode = hashCode * -1521134295 + IsEventSubscriber.GetHashCode();
            hashCode = hashCode * -1521134295 + GenerateConstructor.GetHashCode();
            hashCode = hashCode * -1521134295 + InheritFromViewModelBase.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<FactoryToGenerate?>.Default.GetHashCode(ViewModelFactoryToGenerate);
            hashCode = hashCode * -1521134295 + EqualityComparer<InterfaceToGenerate?>.Default.GetHashCode(ViewModelInterfaceToGenerate);
            return hashCode;
        }
    }
}
