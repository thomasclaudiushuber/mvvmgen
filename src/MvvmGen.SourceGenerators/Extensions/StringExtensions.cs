// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

namespace MvvmGen.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
    }
}
