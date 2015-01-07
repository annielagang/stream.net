using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Seq;

namespace StreamTests
{
    [TestClass]
    public class StreamGenericClassTest
    {
        #region Constructors
        [TestCategory("Constructors"), TestMethod]
        public void EmptyCtor_ReturnsTrueForIsEmptyProperty()
        {
            var stream = Stream<string>.Empty;

            var expectedValue = true;
            var actualValue = stream.IsEmpty;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("Constructors"), TestMethod]
        public void SingleItemCtor_NewStreamWithHeadOnly()
        {
            var stream = new Stream<string>("hello");

            var expectedValue = "hello";
            var actualValue = stream.Head;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("Constructors"), TestMethod]
        public void ManualCreationCtor_NewFiniteStream()
        {
            var stream = new Stream<int>(10, () => new Stream<int>(20));

            var expectedValue1 = 10;
            var actualValue1 = stream.Head;

            var expectedValue2 = 20;
            var actualValue2 = stream.Tail.Head;

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2));
        }

        [TestCategory("Constructors"), TestMethod]
        public void OneParamAndFuncCtor_NewInfiniteStream()
        {
            var stream = new Stream<int>(3, x => x + 3);

            var expectedValue1 = 3;
            var actualValue1 = stream[0];

            var expectedValue2 = 6;
            var actualValue2 = stream[1];

            var expectedValue3 = 60;
            var actualValue3 = stream[19];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));            
        }

        [TestCategory("Constructors"), TestMethod]
        public void TwoParamAndFuncCtor_NewInfiniteStream()
        {
            var stream = new Stream<int>(1, 2, (x, y) => x + y);

            var expectedValue1 = 1;
            var actualValue1 = stream[0];

            var expectedValue2 = 9;
            var actualValue2 = stream[4];

            var expectedValue3 = 37;
            var actualValue3 = stream[18];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Constructors"), TestCategory("StaticMethods"), TestMethod]
        public void MakeCtor_NewFiniteStream()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue1 = 10;
            var actualValue1 = stream[0];

            var expectedValue2 = 20;
            var actualValue2 = stream[1];

            var expectedValue3 = 30;
            var actualValue3 = stream[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Constructors"), TestCategory("StaticMethods"), TestMethod]
        public void MakeIEnumerableCtor_NewFiniteStream()
        {
            var stream = Stream<string>.Make(new List<string>() { "A", "B", "Hi" });

            var expectedValue1 = "A";
            var actualValue1 = stream[0];

            var expectedValue2 = "B";
            var actualValue2 = stream[1];

            var expectedValue3 = "Hi";
            var actualValue3 = stream[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Constructors"), TestCategory("StaticMethods"), TestMethod]
        public void CycleIEnumerableCtor_NewInfiniteStreamContainingGivenParamsRepeatedly()
        {
            var stream = Stream<string>.Cycle("hello", "world", "welcome to stream.net");

            var expectedValue1 = "hello";
            var actualValue1 = stream[0];

            var expectedValue2 = "world";
            var actualValue2 = stream[1];

            var expectedValue3 = "welcome to stream.net";
            var actualValue3 = stream[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Constructors"), TestCategory("StaticMethods"), TestMethod]
        public void CycleCtor_NewInfiniteStreamContainingIEnumerableItemsRepeatedly()
        {
            var list = new List<string>() { "hello", "world", "welcome to stream.net" };
            var stream = Stream<string>.Cycle("hello", "world", "welcome to stream.net");

            var expectedValue1 = "hello";
            var actualValue1 = stream[0];

            var expectedValue2 = "world";
            var actualValue2 = stream[1];

            var expectedValue3 = "welcome to stream.net";
            var actualValue3 = stream[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Constructors"), TestCategory("StaticMethods"), TestMethod]
        public void IterateCtor_NewInfiniteStreamSameBehaviorWithOneParamAndFuncCtor()
        {
            var stream = Stream<int>.Iterate(1, x => x * 2);

            var expectedValue1 = 2;
            var actualValue1 = stream[0];

            var expectedValue2 = 4;
            var actualValue2 = stream[1];

            var expectedValue3 = 8;
            var actualValue3 = stream[2];

            var expectedValue4 = 1024;
            var actualValue4 = stream[9];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3),
                () => Assert.AreEqual(expectedValue4, actualValue4));            
        }

        [TestCategory("Constructors"), TestCategory("StaticMethods"), TestMethod]
        public void RepeatCtor_NewInfiniteStreamContainingSingleItemRepeatedly()
        {
            var stream = Stream<string>.Repeat("hello");

            var expectedValue1 = "hello";
            var actualValue1 = stream[0];

            var expectedValue2 = "hello";
            var actualValue2 = stream[19];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2));  
        }

        [TestCategory("Constructors"), TestCategory("StaticMethods"), TestMethod]
        public void ReplicateCtor_NewFiniteStreamContainingSingleItemRepeatedly()
        {
            var stream = Stream<string>.Replicate("hi", 5);

            var expectedValue1 = "hi";
            var actualValue1 = stream[0];

            var expectedValue2 = "hi";
            var actualValue2 = stream[1];

            var expectedValue3 = "hi";
            var actualValue3 = stream[2];

            var expectedValue4 = "hi";
            var actualValue4 = stream[3];

            var expectedValue5 = "hi";
            var actualValue5 = stream[4];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3),
                () => Assert.AreEqual(expectedValue4, actualValue4),
                () => Assert.AreEqual(expectedValue5, actualValue5));
        }
        #endregion

        #region Properties
        [TestCategory("Properties"), TestMethod]
        public void HeadProp_ReturnValue()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = 10;
            var actualValue = stream.Head;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("Properties"), TestMethod]
        public void TailProp_ReturnValueAndTypeMustBeAThunkedStream()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue1 = 20;
            var actualValue1 = stream.Tail.Head;

            Func<Stream<int>> actualValue2 = () => new Stream<int>(20);

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.IsInstanceOfType(actualValue2, typeof(Func<Stream<int>>)));
        }

        [TestCategory("Properties"), TestMethod]
        public void LengthProp_ReturnLengthValue()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = 3;
            var actualValue = stream.Length;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("Properties"), TestMethod]
        public void IndexerProp_ReturnIndex()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = 30;
            var actualValue = stream[2];
            Assert.AreEqual(expectedValue, actualValue);
        }
        #endregion

        #region Exceptions
        [TestCategory("Constructors"), TestCategory("Exceptions"), TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
          "Cannot get the head of an empty stream.")]
        public void HeadPropOfEmptyStream_RaiseException()
        {
            var actualValue = Stream<string>.Empty.Head;
        }

        [TestCategory("Constructors"), TestCategory("Exceptions"), TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
          "Cannot get the tail of an empty stream.")]
        public void TailPropOfEmptyStream_RaiseException()
        {
            var actualValue = Stream<string>.Empty.Tail;
        }
        #endregion

        [TestCategory("OverridenCoreMethods"), TestMethod]
        public void ToStringOverride_ReturnStringifiedVersionOfStream()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = "Head: 10 Next Head: 20 Next Head: 30 Next - End of Stream -";
            var actualValue = stream.ToString();
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
