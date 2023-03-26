// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace MvvmGen.SourceGenerators.Extensions
{
    internal static class EnumerableExtensions
    {
        public static bool SequenceEqualWithNullCheck<T>(this IEnumerable<T>? enumerable1, IEnumerable<T>? enumerable2)
        {
            if (enumerable1 is null)
            {
                return enumerable2 is null || !enumerable2.Any();
            }

            if (enumerable2 is null)
            {
                return !enumerable1.Any();
            }

            return enumerable1.SequenceEqual(enumerable2);
        }
    }
}
