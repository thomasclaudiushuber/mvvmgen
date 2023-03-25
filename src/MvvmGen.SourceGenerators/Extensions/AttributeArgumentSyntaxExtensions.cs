// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MvvmGen.Extensions
{
    internal static class AttributeArgumentSyntaxExtensions
    {
        /// <summary>
        /// This method is used to resolve property names when they are specified as arguments
        /// in the attributes CommandInvalidate and PropertyInvalidate.
        /// 
        /// For example, this method returns an IEnumerable with the string "LastName"
        /// for the following argument syntaxes:
        /// - "LastName"
        /// - nameof(LastName)
        /// - new string[]{"LastName"}
        /// - new string[]{nameof(LastName)}
        /// - new []{"LastName"}
        /// - new []{nameof(LastName)}
        /// 
        /// The array arguments are the reason why this method 
        /// returns an IEnumerable and not just a single string
        /// </summary>
        /// <param name="syntax">The strings in </param>
        /// <returns></returns>
        internal static IEnumerable<string> GetStringValues(this AttributeArgumentSyntax syntax)
        {
            if (syntax.Expression is ArrayCreationExpressionSyntax arrayCreationSyntax)
            {
                foreach (var str in GetStringsFromArrayInitializer(arrayCreationSyntax.Initializer))
                {
                    yield return str;
                }
            }
            else if (syntax.Expression is ImplicitArrayCreationExpressionSyntax implicitArrayCreationSyntax)
            {
                foreach (var str in GetStringsFromArrayInitializer(implicitArrayCreationSyntax.Initializer))
                {
                    yield return str;
                }
            }
            else
            {
                yield return GetStringValueFromExpression(syntax.Expression);
            }
        }

        private static IEnumerable<string> GetStringsFromArrayInitializer(InitializerExpressionSyntax? initializer)
        {
            if (initializer is not null)
            {
                foreach (var initializerExpression in initializer.Expressions)
                {
                    yield return GetStringValueFromExpression(initializerExpression);
                }
            }
        }

        private static string GetStringValueFromExpression(ExpressionSyntax expressionSyntax)
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
