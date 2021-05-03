// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MvvmGen.SourceGenerator.Extensions
{
  public static class AttributeArgumentExtensions
  {
    public static string GetStringValueFromAttributeArgument(this AttributeArgumentSyntax attributeArgumentSyntax)
    {
      string stringValue = attributeArgumentSyntax.Expression switch
      {
        InvocationExpressionSyntax invocationExpressionSyntax => invocationExpressionSyntax.ArgumentList.Arguments[0].ToString(),
        LiteralExpressionSyntax literalExpressionSyntax => literalExpressionSyntax.Token.ValueText,
        _ => attributeArgumentSyntax.Expression.ToString()
      };
      

      return stringValue;
    }
  }
}
