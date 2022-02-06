// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MvvmGen.Extensions
{
    public static class ExpressionSyntaxExtensions
    {
        public static string GetStringValueFromExpression(this ExpressionSyntax expressionSyntax)
        {
            var stringValue = expressionSyntax switch
            {
                InvocationExpressionSyntax invocationExpressionSyntax => invocationExpressionSyntax.ArgumentList.Arguments[0].ToString(),
                LiteralExpressionSyntax literalExpressionSyntax => literalExpressionSyntax.Token.ValueText,
                _ => expressionSyntax.ToString()
            };

            return stringValue;
        }
    }
}
