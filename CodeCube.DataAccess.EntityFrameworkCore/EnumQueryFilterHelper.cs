using System;
using System.Collections.Generic;

namespace CodeCube.DataAccess.EntityFrameworkCore
{
    public sealed class EnumQueryFilterHelper<T> where T : struct
    {
        public static IEnumerable<T> ApplyFilters(string[] filters)
        {
            var result = new List<T>();

            if (filters.Length == 0)
            {
                foreach (var filter in (T[])Enum.GetValues(typeof(T)))
                {
                    result.Add(filter);
                }
            }
            else
            {
                foreach (var filter in filters)
                {
                    if (Enum.TryParse<T>(filter, out var parsedFilter))
                    {
                        result.Add(parsedFilter);
                    }
                }
            }

            return result;
        }
    }
}
