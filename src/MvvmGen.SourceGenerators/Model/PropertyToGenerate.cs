// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;

namespace MvvmGen.SourceGenerators.Model
{
    internal class PropertyToGenerate
    {
        public PropertyToGenerate(string propertyName, string propertyType, string backingField, bool isReadOnly = false)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            BackingField = backingField;
            IsReadOnly = isReadOnly;
        }

        public string PropertyName { get; }
        public string PropertyType { get; }
        public string BackingField { get; }
        public bool IsReadOnly { get; }
        public IEnumerable<CommandToGenerate>? CommandsToInvalidate { get; set; }
        public string? EventToPublish { get; internal set; }
        public string? EventToPublishConstructorArgs { get; internal set; }
    }
}
