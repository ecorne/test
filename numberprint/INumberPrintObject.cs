using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace numberprint
{
    /// <summary>
    /// Interface for objects that should be sent into Print Numbers constructor
    /// <para>upperbound:  int the max nubmer it will print starting from 1 </para>
    /// <para>Div3Msg:  the message it will print for numbers divisible by 3</para>
    /// <para>Div5Msg:  the message it will print for numbers divisible by 5</para>
    /// <para>Data:  the list of data should be null on construction the GetData function will 
    /// fill out the "generic" object data array and return it to the caller</para>
    /// </summary>
    public interface INumberPrintObject : ISerializable
    {
        int UpperBound {get; set;}
        string Div3Msg { get; set; }
        string Div5Msg { get; set; }
        IList<object> Data { get; set; }

    }

}
