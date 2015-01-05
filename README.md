Stream.Net
========

Stream.Net is a C#-port of [stream.js](https://github.com/dionyziz/stream.js) library (created by Dionysis Zindros) that uses a data structure called streams. It resides in `Seq` namespace to avoid name collisions with .Net library `System.IO.Stream`. 

# Table of Contents
 
* [Requirements](#req)
* [Usage](#usage)
* [Introduction: What are streams?](#intro)
* [Getting Started](#start)
* [More Stream Examples](#more)
* [Infinite Streams](#infinite)
* [Manual Stream Creation](#manual)
* [Alternative Way of Creating Infinite Streams](#alt-ctor)
* [Streams at Work](#work)
* [Tribute](#tribute)
* [Other Stream Implementations in CSharp](#others)

# <a name="req"></a>Requirements
* .Net 4.0 or newer
* .Net 3.5 can be used as well (most advanced C# features used in this library was introduced in 3.5), however, [BigInteger](http://msdn.microsoft.com/en-us/library/system.numerics.biginteger(v=vs.100).aspx) type was added in .Net 4.0.

# <a name="usage"></a>Usage
* Add reference to the Seq.Stream.dll and include `using Seq;` namespace on your project.

# <a name="intro"></a>Introduction: What are streams?
A stream is a lazily evaluated or delayed sequence of data elements. A stream can be used similarly to a list, but later elements are only calculated when needed. Streams can therefore represent infinite sequences and series. In object-oriented programming, input streams are generally implemented as iterators (generators).

# <a name="start"></a>Getting Started
Streams are containers. They contain items. To make a stream with some items using `Stream<T>.Make()` (change `T` to the datatype you want to use). Just pass it as arguments the items you want to be part of your stream:

```
// creates a finite stream containing 10, 20, 30
Stream<int> finiteStream = Stream<int>.Make(10, 20, 30);
```

`finiteStream` is a stream that contains 3 integers: 10, 20, and 30. Use `finiteStream.Length` property to check the length of the stream and use the indexer to retrieve items by index. Alternatively, you can get the first item in the stream by calling its `Head` property.

```
Stream<int> finiteStream = Stream<int>.Make(10, 20, 30);
finiteStream.Print("Stream length", finiteStream.Length.ToString()); // Stream length: 3
finiteStream.Print("Head", finiteStream.Head.ToString()); // Head: 10
finiteStream.Print("Item 1", finiteStream[0].ToString()); // Item 1: 10
finiteStream.Print("Item 2", finiteStream[1].ToString()); // Item 2: 20
finiteStream.Print("Item 3", finiteStream[2].ToString()); // Item 3: 30
```

An empty stream can be created by either `Stream<T>.Empty` or `Stream<T>.Make().` The stream containing all the items of the original stream except the head can be obtained using the `Tail` property of the stream. Calling the `Head` or `Tail` properties on an empty stream will throw an `InvalidOperationException`. To check is a stream is empty, call its `IsEmpty` property that will return either true or false.

```
var finiteStream = Stream<int>.Make(10, 20, 30);
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
```

To print all the elements in a stream:
```
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
```

# <a name="more"></a>More Stream Examples
One of the useful shortcuts is the `Stream.Range(min, max)` function. It returns a stream with the natural numbers ranging from min to max inclusive.

```
Stream<int> rangeStream = Stream.Range(10, 20);
rangeStream.Print("Range test 1"); // prints numbers from 10 to 20
```

Functions like `Map<T>`, `Filter<T>`, `Walk<T>`, `Take<T>`, and `Scale` are just some of the ways to “transform” your streams (refer to Stream function list for all the stream-related functions and their description/syntax). Take note that the streams are immutable, that is they can’t be modified once they’ve been created. To “modify” a stream means to create a new stream that contains the new items/updated data.  Functions that work on the same stream can be chained (pipelining) together.

`Map<T>` takes a function `func` and applies `func` to each of the element in the stream and returns a new stream containing the results from applying `func`. 

```
static int DoubleNum(int x)
{
    return x * 2;
}
 
var rangeStream = Stream.Range(10, 15);
var doubles = rangeStream.Map(DoubleNum);
doubles.Print("10 - 15 doubled");
// DoubleNum() can be also written as Func<int, int> delegate with an anonymous method
Func<int, int> doubleNum = delegate(int x) { return x * 2; };
// DoubleNum() can be also written as Func<int, int> delegate using a lambda expression
Func<int, int> doubleNum2 = x => x * 2; 
// or simply use the lambda expression directly
rangeStream.Map(x => x * 3).Print("Chained methods; 10 - 15 tripled");
```

`Filter<T>` takes a function `func` and runs `func` on every item in the stream and returns a new stream that contains the items for which `func` returned true.

`rangeStream.Filter(x => x % 2 != 0).Print("Output odd numbers");`

Another way to print streams is to use `Walk<T>`:

```
var rangeStream = Stream.Range(10, 12);
rangeStream.Walk(n => Console.WriteLine("The element is: " + n));
```

`Take<T>` takes a number `n` and returns a stream with the first `n` elements of the original stream. If the number parameter is omitted, `Take<T>` will return the first 20 items as default.

```
var rangeStream = Stream.Range(10, 100);
rangeStream.Take(10).Print("First 10 items");
```

`Scale` takes a stream and an integer (factor) and multiplies the factor to each integer in your stream. `Scale` will only work if the items on the original stream have `int` as datatype. (refer to Stream function list numbers section for all the functions that are applicable to numbers only).

`Add` just like `Scale` only works for `int` and it adds each element of the first stream to each of the element of the second stream.

```
var rangeStream = Stream.Range(10, 100);
var multiplesOfTen = rangeStream.Scale(10);
multiplesOfTen.Print("Multiples of ten");
rangeStream.Add(multiplesOfTen).Print();
```

Streams can be used for any datatype supported by the .Net framework.

```
Stream<string> streamStr = new Stream<string>("hello", () => new Stream<string>("world"));
streamStr.Print("First string", streamStr.Head.ToString()); // First string: hello
streamStr.Print("Next string", streamStr.Tail.Head.ToString()); // Next string: world

var textStream = Stream<string>.Make("I'm", "a", "little", "teapot");
textStream.Print();
```

# <a name="infinite"></a>Infinite Streams
Streams don't need to have a finite number of elements. For example, omitting the second parameter to `Stream.Range(low, high)` and write `Stream.Range(low)` in that case, there is no upper bound, and so the stream contains all the natural numbers starting from low and up. Omitting both low and high parameters will return the stream of natural numbers. 

```
Stream.Range(10).Take(5).Print(); // Prints 10 11 12 13 14

var nats = Stream.Range();
var oneToTen = nats.Take(10);
oneToTen.Print(); // Prints 1 2 3 4 5 6 7 8 9 10

nats.Map(x => x * 2).Take(3).Print("Evens"); // Prints 2 4 6
nats.Filter(x => x % 2 != 0).Take(3).Print("Odds"); // Prints 1 3 5   
```

Notice that if `Print<T>` was applied on an infinite stream, it will print forever, eventually running out of memory. Hence it's best use `Take<T>` before `Print<T>`. As a precaution, both `Take<T>` and `Print<T>` has default count of 20, meaning if the user forgot or intentionally omitted the count parameter, `Take<T>` and `Print<T>` will only output the first 20 items of the stream. Using `Length` property on infinite streams is meaningless; doing so will cause an infinite loop (trying to find the end of an endless stream). `Map<T>` and `Filter<T>` can be used on infinite streams. However, `Walk<T>` will also not run properly on infinite streams. So always make sure to use `Take<T>` to make an infinite stream finite.

`Print<T>` has two overloads that accepts:
-	a stream, an optional text description and an optional count which is set to 20 by default
-	a stream, a required description, and a required value

```
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
```

# <a name="manual"></a>Manual Stream Creation
Stream objects can be created using `Stream<T>.Empty` (empty stream), or `new Stream<T>(head, () => Stream<T>.Empty)` (single item, non-empty stream). In case of a non-empty stream, the first parameter is the head of your desired stream, while the second parameter is a function returning the tail (a stream with all the rest of the elements), which could be left out or be explicitly declared as an empty stream.

```
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
                                                () => new Stream<int>())));
chainedStream2.Print("Chained stream 2 (end of stream explicitly included)");
```

This is the long version of `var finiteStream = Stream<int>.Make(10, 20, 30)`. Streams can be created either way.

# <a name="alt-ctor"></a>Alternative Way of Creating Infinite Streams
```
Stream<int> ones = null;
ones = new Stream<int>(1, () => ones);
ones.Take(3).Print();

Stream<int> nats2 = null;
nats2 = new Stream<int>(1, () => nats2.Add(ones));
nats2.Take(5).Print("Alternative way to create natural numbers");
```

A careful reader will now observe the reason why the second parameter to new stream is a thunked tail and not the tail itself. This way we can avoid infinite loops by postponing when the tail is evaluated.

** A thunk is a zero-argument function used to delay evaluation. To “thunk an expression” use `() => exp` instead of `exp` only.

Alternatively, Stream class has built-in functions that create infinite series of ones and natural numbers.

```
var builtInOnes = Stream.MakeOnes();
builtInOnes.Print();

var builtInNats = Stream.MakeNaturalNumbers();
builtInNats.Print();
```

Another way to build an infinite series of a certain type is to use `Stream<T>.Repeat()`,`Stream<T>.Cycle()`, `Stream<T>.Iterate()` or to use one of the different Stream constructor overloads.

```
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
```

# <a name="work"></a>Streams at Work
```
Func<Stream<int>, Stream<int>> sieve = null;
sieve = source => new Stream<int>(source.Head, 
 				             () => Sieve(source.Tail.Filter(n => n % source.Head != 0)));
sieve(Stream.Range(2)).Take(10).Print("First 10 primes");

static Stream<int> SieveReadable(Stream<int> source)
{
    return new Stream<int>(source.Head, 
                        () => SieveReadable(source.Tail.Filter(n => n % source.Head != 0)));
}

SieveReadable(Stream.Range(2)).Take(10).Print("First 10 primes - alternative");
```

Take some time to figure out what `sieve` and `SieveReadable` functions do. Most programmers find it hard to understand unless they have a functional programming background, so don't feel bad if you don't get it immediately. Here's a hint: Try to find what the head of the printed stream will be. And then try to find what the second element of the stream will be (the head of the tail); then the third element, and so forth. The name of the function may also help you.

If you really can't figure out what it does, just run it and see for yourself! It'll be easier to figure out what it does it then.

Another famous usage for streams is Fibonacci.
```
Func<int, int, Stream<int>> fibonacci = null;
fibonacci = (h, n) => new Stream<int>(h, () => fibonacci(n, h + n));
fibonacci(1, 1).Print("First 20 Fibonacci");
```

# <a name="tribute"></a>Tribute
Streams aren't in fact a new idea at all. Many functional languages support them. The name 'stream' is used in Scheme, a LISP dialect that supports these features. Haskell also supports infinite lists. The names 'take', 'tail', 'head', 'map' and 'filter' are all used in Haskell. A different but similar concept also exists in Python and in many other languages; these are called "generators".

Many of the examples and ideas come from the book [Structure and Interpretation of Computer Programs] (http://mitpress.mit.edu/sicp/full-text/book/book.html). If you like the ideas here, it comes highly recommended; it's available online for free. It was the original inspiration for building this library.

# <a name="others"></a>Other Stream Implementations in CSharp
* [C# Infinite Streams] (https://gist.github.com/edalorzo/5015143) - stream library implements [Null Object pattern] (http://en.wikipedia.org/wiki/Null_Object_pattern)
* [Lazy Lists in C#] (http://porg.es/blog/lazy-lists-in-c) - streams as IEnumerable
* [Scala Infinite Streams in C#] (https://gist.github.com/maxgherman/04662d0f263c0f9390ab)
* [Digging deeper into C# Lazy Lists] (http://blogs.msdn.com/b/matt/archive/2008/03/14/digging-deeper-into-lazy-and-functional-c.aspx)
* [More on laziness in C#] (http://joeduffyblog.com/2005/04/23/more-on-laziness-in-c/) - streams implemented as a pair of value and thunk 
