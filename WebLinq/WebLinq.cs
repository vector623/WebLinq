using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// A linq method to return adjacent groupings, given a sequence.  Eg invocation:
        /// GroupAdjacentBy((x, y) => x + 1 == y).  Sourced from stackoverflow: https://stackoverflow.com/a/4682163
        /// </summary>
        /// <param name="source">the collection you want grouped</param>
        /// <param name="predicate">the func used to group the collection</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>the grouped collection</returns>
        public static IEnumerable<IEnumerable<T>> GroupAdjacentBy<T>(this IEnumerable<T> source,
            Func<T, T, bool> predicate)
        {
            using (var e = source.GetEnumerator())
            {
                if (!e.MoveNext()) yield break;
                var list = new List<T> {e.Current};
                var pred = e.Current;
                while (e.MoveNext())
                {
                    if (predicate(pred, e.Current))
                    {
                        list.Add(e.Current);
                    }
                    else
                    {
                        yield return list;
                        list = new List<T> {e.Current};
                    }

                    pred = e.Current;
                }

                yield return list;
            }
        }

        public static IEnumerable<IEnumerable<T>> Slice<T>(this IEnumerable<T> source, int sliceSize)
        {
            var slices = source
                .Select((element, index) =>
                {
                    var indexedElement = new
                    {
                        element,
                        index
                    };
                    return indexedElement;
                })
                .GroupBy(indexElement => indexElement.index % sliceSize)
                .ToList();
            throw new NotImplementedException();
        }
    }
}