using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Seq;

namespace StreamTests
{
    [TestClass]
    public class StreamStaticClassIntTest
    {
        [TestCategory("Static Int Constructors"), TestMethod]
        public void MakeNatsCtor_NewInfiniteStreamOfNaturalNumbers()
        {
            var stream = Stream.MakeNaturalNumbers();            

            var expectedValue1 = 1;
            var actualValue1 = stream[0];

            var expectedValue2 = 2;
            var actualValue2 = stream[1];

            var expectedValue3 = 20;
            var actualValue3 = stream[19];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Static Int Constructors"), TestMethod]
        public void MakeOnesCtor_NewInfiniteStreamOfRepeatedOnes()
        {
            var stream = Stream.MakeOnes();

            var expectedValue1 = 1;
            var actualValue1 = stream[0];

            var expectedValue2 = 1;
            var actualValue2 = stream[1];

            var expectedValue3 = 1;
            var actualValue3 = stream[19];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Static Int Constructors"), TestMethod]
        public void RangeCtor_NewInfiniteStreamOfNumbersAsDefinedByRange()
        {
            var stream = Stream.Range(3, 7);

            var expectedValue0 = 5;
            var actualValue0 = stream.Length;

            var expectedValue1 = 3;
            var actualValue1 = stream[0];

            var expectedValue2 = 4;
            var actualValue2 = stream[1];

            var expectedValue3 = 5;
            var actualValue3 = stream[2];

            var expectedValue4 = 6;
            var actualValue4 = stream[3];

            var expectedValue5 = 7;
            var actualValue5 = stream[4];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue0, actualValue0),
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3),
                () => Assert.AreEqual(expectedValue4, actualValue4),
                () => Assert.AreEqual(expectedValue5, actualValue5));
        }

        [TestCategory("Static Int Constructors"), TestMethod]
        public void RangeCtor_NewInfiniteStreamLowerBoundDefinedOnly()
        {
            var stream = Stream.Range(10);

            var expectedValue1 = 10;
            var actualValue1 = stream[0];

            var expectedValue2 = 11;
            var actualValue2 = stream[1];

            var expectedValue3 = 12;
            var actualValue3 = stream[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }

        [TestCategory("Static Int Constructors"), TestMethod]
        public void RangeCtor_NewInfiniteStreamNoBoundDefined()
        {
            // defaults to natural numbers
            var stream = Stream.Range();

            var expectedValue1 = 1;
            var actualValue1 = stream[0];

            var expectedValue2 = 2;
            var actualValue2 = stream[1];

            var expectedValue3 = 3;
            var actualValue3 = stream[2];

            AssertAll.Execute(
                () => Assert.AreEqual(expectedValue1, actualValue1),
                () => Assert.AreEqual(expectedValue2, actualValue2),
                () => Assert.AreEqual(expectedValue3, actualValue3));
        }
    }
}
