using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OracleFiddler.Core.Infrastructure
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string line)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(line.ToLowerInvariant());
        }

        public static string JoinUsing<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        public static string FromSnakeUpperToPascalCase(this string line)
        {
            return line
                .Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToTitleCase())
                .JoinUsing("");
        }
    }
}