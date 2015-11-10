using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    class Program {
        static void Main(string[] args) {
            int upperTest = 20;
            string div3Msg = "3msg";
            string div5Msg = "5msg";

            List<numberprint.INumberPrintObject> testList = new List<numberprint.INumberPrintObject>();
            testList.Add(new MyInputObject() {
                UpperBound = upperTest,
                Div3Msg = div3Msg,
                Div5Msg = div5Msg
            });


            numberprint.PrintNumbers pn = new numberprint.PrintNumbers(testList);
            List<numberprint.INumberPrintObject> test = pn.GetDataObject();
        }


        /// <summary>
        /// concrete class of the INumberPrintObject
        /// </summary>
        [Serializable]
        private class MyInputObject : numberprint.INumberPrintObject {

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
}
