using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using numberprint;
using Moq;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Linq;


namespace NubmerPrintTest {
    [TestClass]
    public class PrintNumbersTest {

        private static readonly int MAXNUMBER = 100;
        private static readonly string DIV5 = "buzz";
        private static readonly string DIV3 = "fizz";
        //private static readonly int MOD1 = 3;
        //private static readonly int MOD2 = 5;

        [ClassInitialize()]
        public static void PrintNumbersInitialize(TestContext testContext) {

        }

        [TestInitialize]
        public void PrintNumbersTestInitialize() {

        }

        [TestMethod]
        public void EmptyConstructorTest() {
            PrintNumbers pn = new PrintNumbers();
            Assert.IsNotNull(pn.Data);
            Assert.IsTrue(pn.Data[0].UpperBound == MAXNUMBER);
            Assert.IsTrue(pn.Data[0].Div3Msg == DIV3);
            Assert.IsTrue(pn.Data[0].Div5Msg == DIV5);
        }

        [TestMethod]
        public void UpperBoundConstructorTest() {
            int test = 5;
            PrintNumbers pn = new PrintNumbers(5);
            Assert.IsNotNull(pn.Data);
            Assert.IsTrue(pn.Data[0].UpperBound == test);
            Assert.IsTrue(pn.Data[0].Div3Msg == DIV3);
            Assert.IsTrue(pn.Data[0].Div5Msg == DIV5);
        }

        [TestMethod]
        public void SimpleTypeConstructorTest() {
            int upperTest = 5;
            string div3Msg = "divisibleby3";
            string div5Msg = "divisibleby5";

            PrintNumbers pn = new PrintNumbers(upperTest, div3Msg, div5Msg);
            Assert.IsNotNull(pn.Data);
            Assert.IsTrue(pn.Data[0].UpperBound == upperTest);
            Assert.IsTrue(pn.Data[0].Div3Msg.CompareTo(div3Msg) == 0);
            Assert.IsTrue(pn.Data[0].Div5Msg.CompareTo(div5Msg) == 0);
        }

        [TestMethod]
        public void INumberPrintObjectSingleConstructorTest() {

            int upperTest = 11;
            string div3Msg = "3msg";
            string div5Msg = "5msg";

            List<INumberPrintObject> testList = new List<INumberPrintObject>();
            testList.Add(new TestINumberPrintObject() {
                UpperBound = upperTest,
                Div3Msg = div3Msg,
                Div5Msg = div5Msg
            });

            PrintNumbers pn = new PrintNumbers(testList);

            Assert.IsNotNull(pn.Data);
            Assert.IsTrue(pn.Data[0].UpperBound == upperTest);
            Assert.IsTrue(pn.Data[0].Div3Msg.CompareTo(div3Msg) == 0);
            Assert.IsTrue(pn.Data[0].Div5Msg.CompareTo(div5Msg) == 0);
        }

        [TestMethod]
        public void INumberPrintObjectMultiConstructorTest() {

            int upperTest = 20;
            string div3Msg = "msg3";
            string div5Msg = "msg5";

            int upperTest2 = 30;
            string div3Msg2 = "3msg3";
            string div5Msg2 = "5msg5";

            List<INumberPrintObject> testList = new List<INumberPrintObject>();
            testList.Add(new TestINumberPrintObject() {
                UpperBound = upperTest,
                Div3Msg = div3Msg,
                Div5Msg = div5Msg
            });

            testList.Add(new TestINumberPrintObject() {
                UpperBound = upperTest2,
                Div3Msg = div3Msg2,
                Div5Msg = div5Msg2
            });

            PrintNumbers pn = new PrintNumbers(testList);

            Assert.IsNotNull(pn.Data);
            Assert.IsTrue(pn.Data[0].UpperBound == upperTest);
            Assert.IsTrue(pn.Data[0].Div3Msg.CompareTo(div3Msg) == 0);
            Assert.IsTrue(pn.Data[0].Div5Msg.CompareTo(div5Msg) == 0);
            Assert.IsTrue(pn.Data[1].UpperBound == upperTest2);
            Assert.IsTrue(pn.Data[1].Div3Msg.CompareTo(div3Msg2) == 0);
            Assert.IsTrue(pn.Data[1].Div5Msg.CompareTo(div5Msg2) == 0);
        }

        [TestMethod]
        public void GetDatatTest() {
            int upperTest = 20;
            string div3Msg = "3msg";
            string div5Msg = "5msg";

            List<INumberPrintObject> testList = new List<INumberPrintObject>();
            testList.Add(new TestINumberPrintObject() {
                UpperBound = upperTest,
                Div3Msg = div3Msg,
                Div5Msg = div5Msg
            });

            //should hit one of all cases as far as printing out msgs is concerned
            List<object> testData = new List<object>{1,2,"3msg",4,"5msg","3msg",7,8,"3msg","5msg",11,"3msg",13,14,"3msg5msg",16,17,"3msg",19,"5msg"};

            PrintNumbers pn = new PrintNumbers(testList);
            var test = pn.GetDataObject();
            Assert.IsTrue(Enumerable.SequenceEqual(testData, testList[0].Data));
            
        }
    }

     class TestINumberPrintObject : INumberPrintObject {
        private int upperBound;
        private string div3Msg;
        private string div5Msg;
        private IList<object> returnData;

        public int UpperBound { get { return this.upperBound; } set { this.upperBound = value; } }
        public string Div3Msg { get { return this.div3Msg; } set { this.div3Msg = value; } }
        public string Div5Msg { get { return this.div5Msg; } set { this.div5Msg = value; } }
        public IList<object> Data { get { return this.returnData; } set { this.returnData = value; } }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("UpperBound", upperBound);
            info.AddValue("div3Msg", div3Msg);
            info.AddValue("div5Msg", div5Msg);
            info.AddValue("Data", returnData);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null)
                throw new ArgumentNullException("info");

            GetObjectData(info, context);
        }
    }

}
