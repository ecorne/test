using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Permissions;
using System.Runtime.Serialization;
using Newtonsoft.Json;


namespace FizzBuzzter
{
    /// <summary>
    /// numberPrinter object outputs numbers from 1 to upperbound (default 100) inclusive
    /// <param>numbers divisible by 3 it will print the div3Msg (default "fizz")</param>
    /// <param>numbers divisible by 5 it will print the div5Msg (default "fizz")</param>
    /// </summary>
    public class PrintNumbers : FizzBuzzter.IPrintNumbers
    {


        //default constants
        private static readonly int MAXNUMBER = 100;
        private static readonly string DIV5 = "buzz";
        private static readonly string DIV3 = "fizz";
        private static readonly int MOD1 = 3;
        private static readonly int MOD2 = 5;

        private List<INumberPrintObject> inputData;
        public List<INumberPrintObject> Data { get { return inputData; } set { inputData = value; } }


        /// <summary>
        /// will initialize the PrintNUmbers obj with the defaults
        /// <param>Maxnumber = 100, div3Msg = fizz, div5Msg = "buzz"</param>
        /// </summary>
        public PrintNumbers()
        {
            BuildInputObject(MAXNUMBER, DIV3, DIV5);
        }

        /// <summary>
        /// will initialize the PrintNUmbers obj with the upperbound that is passed in,
        /// <param>uses the default messages...  div3Msg = fizz, div5Msg = "buzz"</param>
        /// </summary>
        /// <param name="upperBound"> the highest number to print </param>
        public PrintNumbers(int upperBound)
        {
            BuildInputObject(upperBound, DIV3, DIV5);
        }

        /// <summary>
        /// This constructor is for a single run the obj accepts all parameters 
        /// from the caller for a single iteration as simple types 
        /// </summary>
        /// <param name="upperBound"> the highest number to print</param>
        /// <param name="div3Msg">the divisible by 3 message</param>
        /// <param name="div5Msg">the divisible by 5 message</param>
        public PrintNumbers(int upperBound, string div3Msg, string div5Msg)
        {
            BuildInputObject(upperBound, div3Msg, div5Msg);
        }

        /// <summary>
        /// This constructor takes a list of type INumberPrintObject
        /// <param>each object will have the necessary fields to return multiple iterations of the print number object</param>
        /// </summary>
        /// <param name="inputData">a list of INumberPrintObject to initialize the PrintNumbers object </param>
        public PrintNumbers(List<INumberPrintObject> inputData)
        {
            this.inputData = inputData;
        }


        /// <summary>
        /// Used for all the constructors that don't pass in INumberPrintObject types
        /// It converts the values passed to those constructurs to objects of INumberPrintObject
        /// </summary>
        /// <param name="upperBound"></param>
        /// <param name="div3Msg"></param>
        /// <param name="div5Msg"></param>
        private void BuildInputObject(int upperBound, string div3Msg, string div5Msg)
        {
            this.inputData = new List<INumberPrintObject>();
            this.inputData.Add(new MyInputObject()
            {
                UpperBound = upperBound,
                Div3Msg = div3Msg,
                Div5Msg = div5Msg
            });
        }

        /// <summary>
        /// Populates the data section of the INumberPrintObject and returns it to the caller
        /// </summary>
        /// <returns>List of type INumberPrintObject</returns>
        public List<INumberPrintObject> GetDataObject()
        {
            List<INumberPrintObject> retData = new List<INumberPrintObject>();
            retData = this.inputData.Select(inp => GetData(ref inp)).ToList();
            return retData;
        }

        /// <summary>
        /// Populates the data section of the INumberPrintObject and 
        /// returns it to the caller as a Json String.
        /// <param>This may be useful for customers that dont want to implement the INumberPrintObject in the case of the 1st 3 constructors</param>
        /// <param>The output json can be parsed directly toget the Data array</param>
        /// </summary>
        /// <returns>string</returns>
        public string GetDataObjectJson()
        {

            List<INumberPrintObject> retData = new List<INumberPrintObject>();
            retData = this.inputData.Select(inp => GetData(ref inp)).ToList();
            string jsonString;
            try
            {
                jsonString = JsonConvert.SerializeObject(retData);
            }
            catch (Exception)
            {
                return "";
            }

            return jsonString;
        }

        /// <summary>
        /// builds a list of mixed type string and int
        /// with the data property of INumberPrintObject
        /// </summary>
        private INumberPrintObject GetData(ref INumberPrintObject parameters)
        {

            parameters.Data = new List<object>();
            //iterate through all numbers from 0 to uperbound
            StringBuilder fstring = new StringBuilder();
            object intOrString = new object();

            for (int i = 1; i <= parameters.UpperBound; i++)
            {

                if ((i % MOD1) == 0)
                {
                    fstring.Append(parameters.Div3Msg);
                }

                if ((i % MOD2) == 0)
                {
                    fstring.Append(parameters.Div5Msg);
                }

                intOrString = fstring.ToString();

                if (i % MOD2 != 0 && i % MOD1 != 0)
                {
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
        private class MyInputObject : INumberPrintObject
        {

            private int upperBound;
            private string div3Msg;
            private string div5Msg;
            private IList<object> returnData;

            public int UpperBound { get { return this.upperBound; } set { this.upperBound = value; } }
            public string Div3Msg { get { return this.div3Msg; } set { this.div3Msg = value; } }
            public string Div5Msg { get { return this.div5Msg; } set { this.div5Msg = value; } }
            public IList<object> Data { get { return this.returnData; } set { this.returnData = value; } }

            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("UpperBound", upperBound);
                info.AddValue("div3Msg", div3Msg);
                info.AddValue("div5Msg", div5Msg);
                info.AddValue("Data", returnData);
            }

            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                    throw new ArgumentNullException("info");
                GetObjectData(info, context);
            }
        }

    }





}
