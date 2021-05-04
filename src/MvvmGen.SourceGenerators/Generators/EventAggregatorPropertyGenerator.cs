// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Text;

namespace MvvmGen.SourceGenerators
{
    internal static class EventAggregatorPropertyGenerator
    {
        internal static void Generate(StringBuilder stringBuilder, string indent, bool isEventSubscriber)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"{indent}protected IEventAggregator EventAggregator {{ get; set; }}");
        }
    }

}
