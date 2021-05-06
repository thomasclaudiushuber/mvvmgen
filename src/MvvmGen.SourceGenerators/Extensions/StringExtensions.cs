// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.SourceGenerators.Extensions
{
    public static class StringExtensions
    {
        public static string PascalCaseToCamelCase(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
    }
}
