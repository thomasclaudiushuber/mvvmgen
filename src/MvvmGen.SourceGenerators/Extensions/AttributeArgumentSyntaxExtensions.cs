// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MvvmGen.SourceGenerators.Extensions
{
    public static class AttributeArgumentSyntaxExtensions
    {
        public static string GetStringValueFromAttributeArgument(this AttributeArgumentSyntax attributeArgumentSyntax)
        {
            var stringValue = attributeArgumentSyntax.Expression switch
            {
                InvocationExpressionSyntax invocationExpressionSyntax => invocationExpressionSyntax.ArgumentList.Arguments[0].ToString(),
                LiteralExpressionSyntax literalExpressionSyntax => literalExpressionSyntax.Token.ValueText,
                _ => attributeArgumentSyntax.Expression.ToString()
            };

            return stringValue;
        }
    }
}
