﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace MtdKey.Storage
{
    internal static class FuncExtensions
    {
        public static IQueryable<T> FilterPages<T>(this IQueryable<T> query, int page = 0, int pageSize = 0)
        {
            int skipQty = (page <= 0 ? 1 : page) - 1;
            int takeQty = pageSize <= 0 ? 100 : pageSize;
            return query.Skip(skipQty * takeQty).Take(takeQty);
        }

        public static IEnumerable<string> SplitByLength(this string str, int maxLength)
        {
            int index = 0;
            while (true)
            {
                if (index + maxLength >= str.Length)
                {
                    yield return str[index..];
                    yield break;
                }
                yield return str.Substring(index, maxLength);
                index += maxLength;
            }
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool controlCheck, Expression<Func<T, bool>> condition)
        {
            if (controlCheck)
            {
                return query.Where(condition);
            }
            return query;
        }
    }

}
