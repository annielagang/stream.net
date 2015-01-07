using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seq;
using System.Numerics;

namespace HowToUse
{
    class Program
    {
        static int DoubleNum(int x)
        {
            return x * 2;
        }

        static void Main(string[] args)
        {
            #region Getting Started
            // creates a finite stream containing 10, 20, 30
            Stream<int> finiteStream = Stream<int>.Make(10, 20, 30);
            finiteStream.Print("Stream length", finiteStream.Length.ToString()); // Stream length: 3
            finiteStream.Print("Head", finiteStream.Head.ToString()); // Head: 10
            finiteStream.Print("Item 1", finiteStream[0].ToString()); // Item 1: 10
            finiteStream.Print("Item 2", finiteStream[1].ToString()); // Item 2: 20
            finiteStream.Print("Item 3", finiteStream[2].ToString()); // Item 3: 30
            var t = finiteStream.Tail; // returns the stream that contains two items: 20 and 30
            t.Print("Next item", t.Head.ToString()); // Next item: 20
            var u = t.Tail; // returns the stream that contains one item: 30
            u.Print("Next item", u.Head.ToString()); // Next item: 30
            var v = u.Tail; // returns the empty stream
            v.Print("Is stream empty", v.IsEmpty.ToString()); // Is stream empty: True

            // creating empty streams and checking if they're indeed empty
            var emptyStream1 = Stream<int>.Empty;
            emptyStream1.Print("Is stream1 empty", emptyStream1.IsEmpty.ToString());
            var emptyStream2 = Stream<int>.Make();
            emptyStream2.Print("Is stream2 empty", emptyStream2.IsEmpty.ToString());

            Console.WriteLine();

            // print elements of a stream
            Stream<int> s = Stream<int>.Make(10, 20, 30);
            while (!s.IsEmpty)
            {
                Console.WriteLine(s.Head);
                s = s.Tail;
            }

            // print shortcut
            Stream<int> s2 = Stream<int>.Make(10, 20, 30);
            s2.Print("Prints all the elements in a stream");
            
            Console.WriteLine();
            #endregion

            #region More Stream Examples
            Stream<int> rangeStream = Stream.Range(10, 20);
            rangeStream.Print("Range test 1"); // prints numbers from 10 to 20

            rangeStream = Stream.Range(10, 15);
            rangeStream.Print("New range");
            var doubles = rangeStream.Map(DoubleNum);
            doubles.Print("10 - 15 doubled");
            // DoubleNum() can be also written as Func<int, int> delegate with an anonymous method
            Func<int, int> doubleNum = delegate(int x) { return x * 2; };
            // DoubleNum() can be also written as Func<int, int> delegate using a lambda expression
            Func<int, int> doubleNum2 = x => x * 2; 
            // or simply use the lambda expression directly
            rangeStream.Map(x => x * 3).Print("Chained methods; 10 - 15 tripled");
            rangeStream.Filter(x => x % 2 != 0).Print("Output odd numbers");

            Console.WriteLine();

            rangeStream = Stream.Range(10, 12);
            rangeStream.Walk(n => Console.WriteLine("The element is: " + n));

            rangeStream = Stream.Range(10, 100);
            rangeStream.Take(10).Print("First 10 items");

            var multiplesOfTen = rangeStream.Scale(10);
            multiplesOfTen.Print("Multiples of ten");
            rangeStream.Add(multiplesOfTen).Print();

            Stream<string> streamStr = new Stream<string>("hello", () => new Stream<string>("world"));
            streamStr.Print("First string", streamStr.Head.ToString()); // First string: hello
            streamStr.Print("Next string", streamStr.Tail.Head.ToString()); // Next string: world

            var textStream = Stream<string>.Make("I'm", "a", "little", "teapot");
            textStream.Print();
            #endregion

            #region Infinite Streams
            Stream.Range(10).Take(5).Print(); // Prints 10 11 12 13 14

            var nats = Stream.Range();
            var oneToTen = nats.Take(10);
            oneToTen.Print(); // Prints 1 2 3 4 5 6 7 8 9 10

            nats.Map(x => x * 2).Take(3).Print("Evens"); // Prints 2 4 6
            nats.Filter(x => x % 2 != 0).Take(3).Print("Odds"); // Prints 1 3 5            

            // first overload: Print() without parameters 
            // by default will print the first 20 items from the stream
            // evens and odds stream are new streams; they don't modify nats stream at all
            nats.Print(); // Prints 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20

            // second overload: used to print specific stream properties (Length, Head etc)
            oneToTen.Print("Stream length", oneToTen.Length.ToString()); // Stream length: 10
            oneToTen.Print("Head", oneToTen.Head.ToString()); // Head: 1
            oneToTen.Print("Item 2", oneToTen[1].ToString()); // Item 2: 2
            oneToTen.Print("Item 3", oneToTen[2].ToString()); // Item 3: 3
            oneToTen.Print("Item 4", oneToTen[3].ToString()); // Item 4: 4
            #endregion

            #region Manual Stream Creation
            // a stream that only contains a head (tail is implied to be Stream<int>.Empty)
            var singleItem = new Stream<int>(10);
            singleItem.Print();

            var chainedStream1 = new Stream<int>(10, 
                                      () => new Stream<int>(20, 
                                                 () => new Stream<int>(30)));
            chainedStream1.Print("Chained stream 1 (end of stream implied)");

            var chainedStream2 = new Stream<int>(10, 
                                      () => new Stream<int>(20, 
                                                 () => new Stream<int>(30, 
                                                            () => Stream<int>.Empty)));
            chainedStream2.Print("Chained stream 2 (end of stream explicitly included)");
            #endregion

            #region Alternative Way of Creating Infinite Streams
            Stream<int> ones = null;
            ones = new Stream<int>(1, () => ones);
            ones.Take(3).Print();

            Stream<int> nats2 = null;
            nats2 = new Stream<int>(1, () => nats2.Add(ones));
            nats2.Take(5).Print("Alternative way to create natural numbers");

            var builtInOnes = Stream.MakeOnes();
            builtInOnes.Print();

            var builtInNats = Stream.MakeNaturalNumbers();
            builtInNats.Print();

            // "Hi" string printed 20 times
            Stream<string>.Repeat("hi").Print();

            // Prints the first 20 items from a infinitely repeating 1,2,3 stream
            Stream<int>.Cycle(1, 2, 3).Print();

            // Stream<int>.Iterate invokes the func on the first param
            var evens = Stream<int>.Iterate(0, x => x + 2);
            evens.Print("Evens"); 
             
            // Infinite stream creation via constructors
            // almost same as Stream<int>.Iterate but uses the first param as head instead
            var odds = new Stream<int>(1, x => x + 2);
            odds.Print("Odds");            

            var powTwo = new Stream<int>(2, 2, (x, y) => x * y);
            powTwo.Print("Powers of Two 2");
            #endregion

            #region Streams at Work
            Func<Stream<int>, Stream<int>> sieve = null;
            sieve = source => new Stream<int>(source.Head, 
                                              () => sieve(source.Tail.Filter(n => n % source.Head != 0)));
            sieve(Stream.Range(2)).Take(10).Print("First 10 primes");

            SieveReadable(Stream.Range(2)).Take(10).Print("First 10 primes - alternative");

            Func<int, int, Stream<int>> fibonacci = null;
            fibonacci = (h, n) => new Stream<int>(h, () => fibonacci(n, h + n));
            fibonacci(1, 1).Print("First 20 Fibonacci");
            #endregion
            
            Console.ReadLine();
        }

        static Stream<int> SieveReadable(Stream<int> source)
        {
            return new Stream<int>(source.Head, 
                                   () => SieveReadable(source.Tail.Filter(n => n % source.Head != 0)));
        }
    }
}
