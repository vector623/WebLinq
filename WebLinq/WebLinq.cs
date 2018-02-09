using System;
using System.Collections.Generic;

namespace WebLinq
{
    public static class WebLinq
    {
        /// <summary>
        /// This is a variant of c#'s Aggregate(), which takes a function which will end enumeration given a specified
        /// condition.  This is useful for a lot of web api pulls, which often yield records in batches and you won't
        /// know how many batches to execute until after the first pull.  This enumerator should be able to replace a
        /// lot of messy do-while loops that rely on a lot of stateful variables.
        /// 
        /// Usually, you will want to pass in a very large number range (using Enumerator.Range()) as the source, but
        /// accumulate your results into a page object (or something else that contains your next query params and also
        /// your aggregating collection).
        /// </summary>
        /// <param name="source">collection to be iterated over</param>
        /// <param name="seed">some page result entity, with the results collection and something like a next token or
        /// total result count value, to be used later in the untilFunc() method</param>
        /// <param name="func">put your web pull here and return an updated/concatenated page result object</param>
        /// <param name="untilFunc">a func telling this method when to stop looping (meaning, no more results to fetch)
        /// </param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TAccumulate"></typeparam>
        /// <returns>a fully filled out page result object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TAccumulate AggregateUntil<TSource, TAccumulate>(this IEnumerable<TSource> source, 
            TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate,bool> untilFunc) 
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            TAccumulate result = seed;
            foreach (TSource element in source)
            {
                result = func(result, element);
                if (untilFunc(result))
                {
                    break;
                }
            }
            return result;
        }
    }
}