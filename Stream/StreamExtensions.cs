using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Seq
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Adds the elements of stream2 to stream1 and returning the new stream as result.
        /// </summary>
        public static Stream<T> Append<T>(this Stream<T> stream1, Stream<T> stream2)
        {
            if (stream1.IsEmpty)
            {
                return stream2;
            }

            return new Stream<T>(stream1.Head, () => stream1.Tail.Append(stream2));
        }

        /// <summary>
        /// Returns only the elements following a certain number of initial elements that must be skipped.
        /// </summary>
        public static Stream<T> Drop<T>(this Stream<T> stream, int count)
        {
            while (count > 0)
            {
                if (stream.IsEmpty)
                {
                    return Stream<T>.Empty;
                }

                stream = stream.Tail;
                count--;
            }

            return new Stream<T>(stream.Head, () => stream.Tail);
        }

        /// <summary>
        /// Bypasses elements in a stream as long as a specified condition is true and then returns the remaining elements.
        /// </summary>
        public static Stream<T> DropWhile<T>(this Stream<T> stream, Predicate<T> pred)
        {
            if (stream.IsEmpty)
            {
                return Stream<T>.Empty;
            }

            var head = stream.Head;
            var tail = stream.Tail;

            if (pred(head))
            {
                return tail.DropWhile(pred);
            }
            else
            {
                return new Stream<T>(head, () => tail);
            }
        }

        /// <summary>
        /// Returns true if the contents of stream1 is same with the contents of stream2. 
        /// It also returns true if both streams are empty.
        /// </summary>
        public static bool Equals<T>(this Stream<T> stream1, Stream<T> stream2)
        {
            if (stream1.IsEmpty && stream2.IsEmpty)
            {
                return true;
            }

            if (stream1.IsEmpty || stream2.IsEmpty)
            {
                return false;
            }

            if (EqualityComparer<T>.Default.Equals(stream1.Head, stream2.Head))
            {
                return stream1.Tail.Equals<T>(stream2.Tail);
            }

            return false;
        }

        /// <summary>
        /// Reduces the stream using the binary operator, from left to right.
        /// </summary>
        public static T FoldL<T>(this Stream<T> stream, Func<T, T, T> func, T init)
        {
            if (stream.IsEmpty)
            {
                return init;
            }

            // Haskell implementation of FoldL
            return stream.Tail.FoldL(func, func(init, stream.Head));
            // SML implementation of FoldL
            //return stream.Tail.FoldL(func, func(stream.Head, init));
        }

        /// <summary>
        /// Reduces the stream using the binary operator, from right to left.
        /// </summary>
        public static T FoldR<U,T>(this Stream<U> stream, Func<U, T, T> func, T init)
        {
            if (stream.IsEmpty)
            {
                return init;
            }

            return func(stream.Head, stream.Tail.FoldR(func, init));
        }

        /// <summary>
        /// Applies the predicate to the stream and returns a new stream of those elements that satisfy the predicate.
        /// </summary>
        public static Stream<T> Filter<T>(this Stream<T> stream, Predicate<T> pred)
        {
            if (stream.IsEmpty)
            {
                return Stream<T>.Empty;
            }

            if (pred(stream.Head))
            {
                return new Stream<T>(stream.Head, () => stream.Tail.Filter(pred));
            }
            else
            {
                return stream.Tail.Filter(pred);
            }
        }

        /// <summary>
        /// If the item is in the stream, return the item's index/position. Otherwise, return -1.
        /// </summary>
        public static int IndexOf<T>(this Stream<T> stream, T item)
        {
            int itemIndex = -1;

            for (int i = 0; i < stream.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(stream[i], item))
                {
                    itemIndex = i;
                    break;
                }
            }

            return itemIndex;
        }

        /// <summary>
        /// Applies func to each element of the stream and returns a new stream of the results.
        /// </summary>
        public static Stream<T> Map<T>(this Stream<T> stream, Func<T, T> func)
        {
            if (stream.IsEmpty)
            {
                return Stream<T>.Empty;
            }

            return new Stream<T>(func(stream.Head), () => stream.Tail.Map(func));
        }

        /// <summary>
        /// Checks if the stream contains the given item. 
        /// </summary>
        public static bool Member<T>(this Stream<T> stream, T item)
        {
            while (!stream.IsEmpty)
            {
                if (EqualityComparer<T>.Default.Equals(stream.Head, item))
                {
                    return true;
                }

                stream = stream.Tail;
            }

            return false;
        }

        /// <summary>
        /// Adds the elements of stream1 to stream2 and returning the new stream as result.
        /// </summary>
        public static Stream<T> Prepend<T>(this Stream<T> stream, T singleItem)
        {
            if (stream.IsEmpty)
            {
                return new Stream<T>(singleItem);
            }

            return new Stream<T>(singleItem, () => stream);
        }

        /// <summary>
        /// Prints the first 20 items (can be changed) of the stream.
        /// </summary>
        public static void Print<T>(this Stream<T> stream, string description = null, int count = 20)
        {
            Console.WriteLine();
            string formatted = description != null ? string.Format("*{0}*", description) : null;
            Console.WriteLine(formatted);
            stream.Take(count).Walk(n => Console.WriteLine(n));
        }

        // stream param not needed in function body but added so that this show as extension method
        // this function is used for printing individual properties of a stream (i.e. head, length etc)
        /// <summary>
        /// Prints an item from the stream along with it's description.
        /// </summary>
        public static void Print<T>(this Stream<T> stream, string description, string value)
        {
            Console.WriteLine();
            string formatted = string.Format("{0}: {1}", description, value);
            Console.Write(formatted);
        }

        /// <summary>
        /// A variant of FoldL that has no starting value argument, and thus must be applied to non-empty lists.
        /// </summary>
        public static T Reduce<T>(this Stream<T> stream, Func<T, T, T> func)
        {
            if (stream.IsEmpty)
            {
                return default(T);
            }

            return stream.Tail.FoldL(func, stream.Head);
        }

        /// <summary>
        /// Returns a portion from a stream with the first n number of items given.
        /// </summary>
        public static Stream<T> Take<T>(this Stream<T> stream, int count = 20)
        {
            if (stream.IsEmpty || count == 0)
            {
                return Stream<T>.Empty;
            }

            return new Stream<T>(stream.Head, () => stream.Tail.Take(count - 1));
        }

        /// <summary>
        /// Returns elements starting from the beginning of the stream until it satisfies the condition specified.
        /// </summary>
        public static Stream<T> TakeWhile<T>(this Stream<T> stream, Predicate<T> pred)
        {
            if (stream.IsEmpty || !pred(stream.Head))
            {
                return Stream<T>.Empty;
            }

            return new Stream<T>(stream.Head, () => stream.Tail.TakeWhile(pred));
        }

        /// <summary>
        /// Converts a stream object to a list object.
        /// </summary>
        public static List<T> ToList<T>(this Stream<T> stream, int count = 20)
        {
            List<T> temp = new List<T>();
            stream.Take(count).Walk(n => temp.Add(n));
            return temp;
        }

        /// <summary>
        /// Applies the given action on each item in the stream.
        /// </summary>
        public static void Walk<T>(this Stream<T> stream, Action<T> action)
        {
            while (!stream.IsEmpty)
            {
                action(stream.Head);
                stream = stream.Tail;            
            }
        }

        /// <summary>
        /// Takes two streams and returns a new stream of the corresponding pairs. 
        /// </summary>
        public static Stream<T> Zip<T>(this Stream<T> stream1, Func<T, T, T> func, Stream<T> stream2)
        {
            if (stream1.IsEmpty)
            {
                return stream2;
            }

            if (stream2.IsEmpty)
            {
                return stream1;
            }

            return new Stream<T>(func(stream1.Head, stream2.Head),
                                      () => stream1.Tail.Zip(func, stream2.Tail));
        }

        #region Special Numeric (Integer )Stream Functions
        /// <summary>
        /// Returns the sum of the two finite streams.
        /// </summary>        
        public static Stream<int> Add(this Stream<int> stream1, Stream<int> stream2)
        {
            return stream1.Zip((x, y) => x + y, stream2);
        }

        /// <summary>
        /// Multiplies the given number to each element of the stream and returns a new stream containing the products.
        /// </summary> 
        public static Stream<int> Scale(this Stream<int> stream, int factor)
        {
            return stream.Map(x => x * factor);
        }

        /// <summary>
        /// Returns the sum of the given stream.
        /// </summary>    
        public static int Sum(this Stream<int> stream)
        {
            return stream.FoldL<int>((x, y) => x + y, 0);
        }
        #endregion
    }
}
