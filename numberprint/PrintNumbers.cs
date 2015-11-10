using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Permissions;
using System.Runtime.Serialization;


namespace numberprint {
    /// <summary>
    /// print all numbers from 1 to upperbound 
    /// number divisible by 3 print fizz
    /// numbers divisible by 5 print buzz
    /// </summary>
    public class PrintNumbers {


        //default constants
        private static readonly int MAXNUMBER = 100;
        private static readonly string DIV5 = "buzz";
        private static readonly string DIV3 = "fizz";
        private static readonly int MOD1 = 3;
        private static readonly int MOD2 = 5;

        private List<INumberPrintObject> inputData;

        public List<INumberPrintObject> Data { get { return inputData; } }

        /// <summary>
        /// will initialize the PrintNUmbers obj with the defaults
        /// Maxnumber = 100, div3Msg = fizz, div5Msg = "buzz"
        /// </summary>
        public PrintNumbers() {
            BuildInputOpbject(MAXNUMBER, DIV3, DIV5);
        }

        /// <summary>
        /// /// <summary>
        /// will initialize the PrintNUmbers obj with the upperbound that is passed
        /// and uses the default messages...  div3Msg = fizz, div5Msg = "buzz"
        /// </summary>
        /// <param name="upperBound"> the highest number to print </param>
        public PrintNumbers(int upperBound) {
            BuildInputOpbject(upperBound, DIV3, DIV5);
        }

        /// <summary>
        /// This constructor is for a single run the obj excepts all parameters 
        /// from the caller for a single iteration
        /// </summary>
        /// <param name="upperBound"> the highest number to print</param>
        /// <param name="div3Msg">the divisible by 3 message</param>
        /// <param name="div5Msg">the divisible by 5 message</param>
        public PrintNumbers(int upperBound, string div3Msg, string div5Msg) {
            BuildInputOpbject(upperBound, div3Msg, div5Msg);
        }

        /// <summary>
        /// This constructor takes a list of type INumberPrintObject
        /// each object will have the necessary fields to retrun multiple iterations of the 
        /// </summary>
        /// <param name="inputData">a list of INumberPrintObject to initialize the PrintNumbers object </param>
        public PrintNumbers(List<INumberPrintObject> inputData) {
            this.inputData = inputData;
        }


        /// <summary>
        /// Used for all the constructors that don't pass in INumberPrintObject types
        /// It converts the values passed to those constructurs to objects of INumberPrintObject
        /// </summary>
        /// <param name="upperBound"></param>
        /// <param name="div3Msg"></param>
        /// <param name="div5Msg"></param>
        private void BuildInputOpbject(int upperBound, string div3Msg, string div5Msg){
            this.inputData = new List<INumberPrintObject>();
            this.inputData.Add(new MyInputObject() {
                UpperBound = upperBound,
                Div3Msg = div3Msg,
                Div5Msg = div5Msg
            });
        }


        /// <summary>
        /// Populated the data section of the INumberPrintObject passed in and 
        /// returns it to the caller
        /// </summary>
        /// <returns>List<INumberPrintObject></returns>
        public List<INumberPrintObject> GetDataObject(){
            List<INumberPrintObject> retData = new List<INumberPrintObject>();
            retData = this.inputData.Select(inp => GetData(ref inp)).ToList();
            return retData;
        }

        ///// <summary>
        ///// Populated the data section of the INumberPrintObject passed in and 
        ///// returns it to the caller
        ///// </summary>
        ///// <returns>List<INumberPrintObject></returns>
        //public String GetDataObjectJson() {
        //    List<INumberPrintObject> retData = new List<INumberPrintObject>();
        //    retData = this.inputData.Select(inp => GetData(ref inp)).ToList();
        //    return retData;
        //}

        /// <summary>
        /// builds a list of mixed type string and int
        /// with the parameters passed into the constructor
        /// </summary>
        private INumberPrintObject GetData(ref INumberPrintObject parameters) {

            parameters.Data = new List<object>();
            //iterate through all numbers from 0 to 1000
            StringBuilder fstring = new StringBuilder();
            object intOrString = new object();

            for (int i = 1; i <= parameters.UpperBound; i++) {

                if ((i % MOD1) == 0) {
                    //Console.WriteLine(parameters.Div3Msg);
                    fstring.Append(parameters.Div3Msg);
                }

                if ((i % MOD2) == 0) {
                    //Console.WriteLine(parameters.Div5Msg);
                    fstring.Append(parameters.Div5Msg);
                }

                intOrString = fstring.ToString();

                if (i % MOD2 != 0 && i % MOD1 != 0) {
                    //Console.WriteLine(i);
                    intOrString = i;
                }
                
                parameters.Data.Add(intOrString);
                fstring.Clear();

            }
            return parameters;
        }



        /// <summary>
        /// concrete class of the INumberPrintObject
        /// </summary>
        [Serializable]
        private class MyInputObject : INumberPrintObject {

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
