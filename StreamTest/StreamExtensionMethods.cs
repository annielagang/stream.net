using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Seq;

namespace StreamTests
{
    [TestClass]
    public class StreamExtensionMethods
    {
        #region Stream Functions
        [TestCategory("ExtensionMethods"), TestMethod]
        public void Append_OneNonEmptyAndEmptyStream_NewStreamContainingBothStreams()
        {
            var stream0 = Stream<int>.Empty;
            var stream1 = Stream<int>.Make(2, 3);

            var appendedStream = stream0.Append(stream1);

            var expectedValue1 = 2;
            var actualValue1 = appendedStream.Length;

            var expectedValue2 = 3;
            var actualValue2 = appendedStream.Tail.Head;

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Append_BothStreamsNonEmpty_NewStreamContainingBothStreams()
        {
            var stream1 = Stream<int>.Make(1);
            var stream2 = Stream<int>.Make(2, 3);

            var appendedStream1 = stream1.Append(stream2);

            var expectedValue0 = 3;
            var actualValue0 = appendedStream1.Length;

            var expectedValue1 = 1;
            var actualValue1 = appendedStream1.Head;

            var expectedValue2 = 2;
            var actualValue2 = appendedStream1[1];

            var expectedValue3 = 3;
            var actualValue3 = appendedStream1[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue0, actualValue0),
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Drop_NonEmptyStream_NewStreamContainingTheRemainingItems()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = new Stream<int>(30);
            var actualValue = stream.Drop(2);
            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Drop_EmptyStream_ReturnsEmptyStream()
        {
            var stream = Stream<int>.Empty;
            var actualValue = stream.Drop(2);

            Assert.IsTrue(actualValue.IsEmpty);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Drop_NonEmptyStreamCountIsZero_NewStreamContainingAllItems()
        {
            var stream = Stream<int>.Make(1, 2, 3, 4);

            var expectedValue = Stream<int>.Make(1, 2, 3, 4);
            var actualValue = stream.Drop(0);

            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void DropWhile_NonEmptyStream_NewStreamContainingTheRemainingItemsNotMatchedByThePred()
        {
            var stream = Stream<int>.Make(-5, -8, -2, 34, 10, -2);
            var remaining_nums = stream.DropWhile(x => x < 0);

            var expectedValue1 = 34;
            var actualValue1 = remaining_nums[0];

            var expectedValue2 = 10;
            var actualValue2 = remaining_nums[1];

            var expectedValue3 = -2;
            var actualValue3 = remaining_nums[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void DropWhile_EmptyStream_ReturnsEmptyStream()
        {
            var stream = Stream<int>.Empty;
            var remaining_nums = stream.DropWhile(x => x < 0);

            var actualValue1 = Stream<int>.Empty;

            var expectedValue2 = true;
            var actualValue2 = remaining_nums.IsEmpty;

            AssertAll.Execute(
                () => Assert.IsInstanceOfType(actualValue1, typeof(Stream<int>)),
                () => Assert.AreEqual(expectedValue2, actualValue2));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Equals_EmptyStream_ReturnsTrue()
        {
            var stream1 = Stream<int>.Empty;
            var stream2 = Stream<int>.Empty;

            var expectedValue = true;
            var actualValue = stream1.Equals(stream2);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Equals_BothNonEmptySameItemsSameOrder_ReturnsTrue()
        {
            var stream1 = Stream<int>.Make(1, 2, 3);
            var stream2 = Stream<int>.Make(1, 2, 3);

            var expectedValue = true;
            var actualValue = stream1.Equals<int>(stream2);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Equals_BothNonEmptySameItemsDifferentOrder_ReturnsFalse()
        {
            var stream1 = Stream<int>.Make(1, 2, 3);
            var stream2 = Stream<int>.Make(1, 3, 2);

            var expectedValue = false;
            var actualValue = stream1.Equals<int>(stream2);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Equals_BothNonEmptyDifferentItems_ReturnsFalse()
        {
            var stream1 = Stream<int>.Make(1, 2, 3);
            var stream2 = Stream<int>.Make(1, 2);

            var expectedValue = false;
            var actualValue = stream1.Equals<int>(stream2);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Equals_OneNonEmptyAndEmptyStream_ReturnsFalse()
        {
            var stream1 = Stream<int>.Empty;
            var stream2 = Stream<int>.Make(1, 2, 3);

            var expectedValue = false;
            var actualValue = stream1.Equals<int>(stream2);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void FoldL_ReturnsDifference()
        {
            var stream = Stream<int>.Make(1, 2, 3, 4);

            var expectedValue = 90;
            var actualValue = stream.FoldL((x, y) => x - y, 100);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void FoldR_ReturnsDifference()
        {
            var stream = Stream<int>.Make(1, 2, 3, 4);

            var expectedValue = 98;
            var actualValue = stream.FoldR((x, y) => x - y, 100);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Filter_NonEmptyStream_NewStreamContainingTheItemsThatMatchedThePred()
        {
            var stream = Stream<int>.Make(1, 2, 3, 4);

            var expectedValue = Stream<int>.Make(2, 4);
            var actualValue = stream.Filter(x => x % 2 == 0);

            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Filter_EmptyStream_ReturnsEmptyStream()
        {
            var stream = Stream<int>.Empty;
            var actualValue = stream.Filter(x => x % 2 == 0);

            Assert.IsTrue(actualValue.IsEmpty);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void IndexOf_ItemFoundInStream_ReturnsIndex()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = 2;
            var actualValue = stream.IndexOf<int>(30);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void IndexOf_ItemNotFoundInStream_ReturnsNegOne()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = -1;
            var actualValue = stream.IndexOf<int>(40);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Map_NewStreamWithTheNewProjectedValues()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = Stream<int>.Make(100, 200, 300);
            var actualValue = stream.Map(x => x * 10);

            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Member_NonEmptyStreamItemFoundInStream_ReturnsTrue()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = true;
            var actualValue = stream.Member(30);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Member_NonEmptyStreamItemNotFoundInStream_ReturnsFalse()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = false;
            var actualValue = stream.Member(40);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Prepend_EmptyStream_NewStreamContainingBothStreams()
        {
            var stream = Stream<int>.Empty;
            var prependedStream = stream.Prepend(10);

            var expectedValue = 10;
            var actualValue = prependedStream.Head;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Prepend_NonEmptyStream_NewStreamContainingBothStreams()
        {
            var stream = Stream<int>.Make(10, 20, 30);
            var prependedStream = stream.Prepend(1);

            var expectedValue1 = 1;
            var actualValue1 = prependedStream.Head;

            var expectedValue2 = 10;
            var actualValue2 = prependedStream[1];

            var expectedValue3 = 30;
            var actualValue3 = prependedStream[3];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Reduce_SameAsFoldLExceptWithoutInitialValue()
        {
            var stream = Stream.Range(1, 20);

            var expectedValue = 210;
            var actualValue = stream.Reduce((x, y) => x + y);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Take_DefaultCount_First20Items()
        {
            var stream = Stream.Range();

            var expectedValue = Stream.Range(1, 20);
            var actualValue = stream.Take();

            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Take_GivenCount_FirstNthItems()
        {
            var stream = Stream.Range();

            var expectedValue = Stream.Range(1, 5);
            var actualValue = stream.Take(5);

            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Take_EmptyStream_ReturnsEmptyStream()
        {
            var stream = Stream<int>.Empty;
            var nums = stream.Take(0);

            var actualValue1 = Stream<int>.Empty;

            var expectedValue2 = true;
            var actualValue2 = nums.IsEmpty;

            AssertAll.Execute(
                () => Assert.IsInstanceOfType(actualValue1, typeof(Stream<int>)),
                () => Assert.AreEqual(expectedValue2, actualValue2));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void TakeWhile_NonEmptyStream_NewStreamContainingTheRemainingItemsNotMatchedByThePred()
        {
            var stream = Stream<int>.Make(-5, -8, -2, 34, 10, -2);
            var nums = stream.TakeWhile(x => x < 0);

            var expectedValue1 = -5;
            var actualValue1 = nums[0];

            var expectedValue2 = -8;
            var actualValue2 = nums[1];

            var expectedValue3 = -2;
            var actualValue3 = nums[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void TakeWhile_EmptyStream_ReturnsEmptyStream()
        {
            var stream = Stream<int>.Empty;
            var nums = stream.TakeWhile(x => x > 0);

            var actualValue1 = Stream<int>.Empty;

            var expectedValue2 = true;
            var actualValue2 = nums.IsEmpty;

            AssertAll.Execute(
                () => Assert.IsInstanceOfType(actualValue1, typeof(Stream<int>)),
                () => Assert.AreEqual(expectedValue2, actualValue2));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void ToList_ReturnsANewListContainingItemsFromTheStream()
        {
            var stream = Stream<int>.Make(10, 20, 30);
            var list = stream.ToList();

            var actualValue = new List<int>(){ 10, 20, 30 };

            actualValue.Equals(list);

            AssertAll.Execute(
                () => Assert.IsInstanceOfType(actualValue, typeof(List<int>)),
                () => Assert.IsTrue(actualValue.SequenceEqual(list)));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Zip_BothNonEmptySameLength_NewStreamWithZippedPairs()
        {
            var stream1 = Stream<int>.Make(4, 8, 12, 23, 5);
            var stream2 = Stream<int>.Make(2, 10, 5, 99, 100);

            var biggest_of_the_two = stream1.Zip((x, y) => Math.Max(x, y), stream2);

            var expectedValue1 = 4;
            var actualValue1 = biggest_of_the_two[0];

            var expectedValue2 = 10;
            var actualValue2 = biggest_of_the_two[1];

            var expectedValue3 = 12;
            var actualValue3 = biggest_of_the_two[2];

            var expectedValue4 = 99;
            var actualValue4 = biggest_of_the_two[3];

            var expectedValue5 = 100;
            var actualValue5 = biggest_of_the_two[4];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3),
                () => Assert.AreEqual(expectedValue4, actualValue4),
                () => Assert.AreEqual(expectedValue5, actualValue5));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Zip_BothNonEmptyDifferentLength_NewStreamWithZippedPairs()
        {
            var stream1 = Stream<int>.Make(4, 8, 12, 16);
            var stream2 = Stream<int>.Make(1, 12, 42);

            var biggest_of_the_two1 = stream1.Zip((x, y) => Math.Max(x, y), stream2);
            var biggest_of_the_two2 = stream2.Zip((x, y) => Math.Max(x, y), stream1);

            var expectedValue0 = 4;
            var actualValue0 = biggest_of_the_two1.Length;

            var expectedValue1 = 4;
            var actualValue1 = biggest_of_the_two2.Length;

            var expectedValue2 = 4;
            var actualValue2 = biggest_of_the_two1[0];

            var expectedValue3 = 12;
            var actualValue3 = biggest_of_the_two1[1];

            var expectedValue4 = 42;
            var actualValue4 = biggest_of_the_two1[2];

            var expectedValue5 = 4;
            var actualValue5 = biggest_of_the_two2[0];

            var expectedValue6 = 12;
            var actualValue6 = biggest_of_the_two2[1];

            var expectedValue7 = 42;
            var actualValue7 = biggest_of_the_two2[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue0, actualValue0),
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3),
                () => Assert.AreEqual(expectedValue4, actualValue4),
                () => Assert.AreEqual(expectedValue5, actualValue5),
                () => Assert.AreEqual(expectedValue6, actualValue6),
                () => Assert.AreEqual(expectedValue7, actualValue7));
        }
        #endregion

        #region Special Numeric Stream Functions
        [TestCategory("ExtensionMethods"), TestMethod]
        public void Add_ReturnsANewStreamOfSums()
        {
            var stream1 = Stream<int>.Make(10, 20, 30);
            var stream2 = Stream<int>.Make(1, 2, 3);

            var expectedValue = Stream<int>.Make(11, 22, 33);
            var actualValue = stream1.Add(stream2);
            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Sum_ReturnsSumOfTheStream()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = 60;
            var actualValue = stream.Sum();
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestCategory("ExtensionMethods"), TestMethod]
        public void Scale_ReturnsANewStreamOfProducts()
        {
            var stream = Stream<int>.Make(10, 20, 30);

            var expectedValue = Stream<int>.Make(50, 100, 150);
            var actualValue = stream.Scale(5);
            Assert.IsTrue(expectedValue.Equals<int>(actualValue));
        }
        #endregion
    }      
}
