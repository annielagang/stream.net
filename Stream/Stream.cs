using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Numerics;

namespace Seq
{
    public class Stream<T> 
    {
        public readonly bool IsEmpty;
        private readonly T head;
        public T Head
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Cannot get the head of an empty stream.");
                }
                else
                {
                    return head;
                }
            }
        }

        private readonly Func<Stream<T>> tail;
        // same functionality with force function of stream.js
        public Stream<T> Tail
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Cannot get the tail of an empty stream.");
                }
                else
                {
                    return tail();
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Cannot use indexer on an empty stream.");
                }
                else
                {
                    var stream = this;
                    int n = index;
                    while (n != 0)
                    {
                        n--;

                        try
                        {
                            stream = stream.Tail;
                        }
                        catch (Exception)
                        {
                            throw new IndexOutOfRangeException("Stream index out of range.");
                        }
                    }

                    try
                    {
                        return stream.Head;
                    }
                    catch (Exception)
                    {
                        throw new IndexOutOfRangeException("Stream index out of range.");
                    }
                }
            }
        }

        public int Length
        {
            get
            {
                var stream = this;
                int n = 0;

                while (!stream.IsEmpty)
                {
                    n++;
                    stream = stream.Tail;
                }

                return n;
            }
        }

        private StringBuilder strBuilder = new StringBuilder();

        private Stream() { IsEmpty = true; }
        public static readonly Stream<T> Empty = new Stream<T>();       

        public Stream(T head) : this(head, () => Stream<T>.Empty) {}

        public Stream(T head, Func<Stream<T>> tail)
        {
            this.head = head;
            this.tail = tail;
            IsEmpty = false;
        }

        public Stream(T head, Func<T, T> func)
             : this(head, () => Stream<T>.Empty)
        {
            Func<Stream<T>> helperFunc = null;
            helperFunc = () => new Stream<T>(func(head), func);
            this.tail = helperFunc;
        }

        public Stream(T head, T item, Func<T, T, T> func)
             : this(head, () => Stream<T>.Empty)
        {
            Func<T, T, Stream<T>> helperFunc = null;
            helperFunc = (x, y) => new Stream<T>(func(x, y), () => helperFunc(func(x, y), y));
            this.tail = () => helperFunc.Invoke(head, item); 
        }
        
        public static Stream<T> Make(params T[] args)
        {
            if (args.Length > 0)
            {
                var head = args[0];
                var tail = args.Skip(1).ToArray();
                return new Stream<T>(head, () => Make(tail));
            }

            return Stream<T>.Empty;
        }

        // same functionality with Stream.fromArray of stream.js
        public static Stream<T> Make(IEnumerable<T> args)
        {
            return Make(args.ToArray());
        }

        /// <summary>
        /// Returns stream containing an infinite repetition of the original list.
        /// </summary>
        public static Stream<T> Cycle(params T[] args)
        {
            Func<T[], int, Stream<T>> helperFunc = null;
            helperFunc = (array, index) =>
            {
                if (index >= array.Length) index = 0;
                return new Stream<T>(array[index], () => helperFunc(array, index + 1));
            };

            return new Stream<T>(args[0], () => helperFunc.Invoke(args, 1));
        }

        public static Stream<T> Cycle(IEnumerable<T> args)
        {
            return Cycle(args.ToArray());
        }

        /// <summary>
        /// Returns an infinite stream of repeated applications of func to the given item.
        /// </summary>
        public static Stream<T> Iterate(T item, Func<T, T> func)
        {
            return new Stream<T>(func(item), func);
        }

        /// <summary>
        /// Returns an infinite stream, with the given item as the value of every element.
        /// </summary>
        public static Stream<T> Repeat(T item)
        {
            Func<T, Stream<T>> helperFunc = null;
            helperFunc = x => new Stream<T>(x, () => helperFunc(x));
            return helperFunc.Invoke(item);
        }

        /// <summary>
        /// Returns a finite stream of length n, with the given item as the value of every element.
        /// </summary>
        public static Stream<T> Replicate(T item, int length)
        {
            return Repeat(item).Take(length);
        }

        private string tailFormatted = string.Empty;
        public override string ToString()
        {
            if (IsEmpty)
            {
                return "- End of Stream -";
            }

            string headFormatted = string.Format("Head: {0}", Head); 
            string tailFormatted = string.Format("Next {0}", Tail);
            return string.Format("{0} {1}", headFormatted, tailFormatted);
        }
    }

    public static class Stream
    {
        public static Stream<int> MakeNaturalNumbers()
        {
            return new Stream<int>(1, () => Stream.MakeNaturalNumbers().Add(Stream.MakeOnes()));
        }
        
        public static Stream<int> MakeOnes()
        {
            return new Stream<int>(1, () => Stream.MakeOnes());
        }
        
        public static Stream<int> Range(int low = 1, int high = 0)
        {
            if (low == high)
            {
                return Stream<int>.Make(new int[] { low });
            }

            return new Stream<int>(low, () => Stream.Range(low + 1, high));
        }       
    }
}
