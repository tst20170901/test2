using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class Edo_ConfigKV
    {
        public int ID { get; set; }
        public string Keys { get; set; }
        public string Vals { get; set; }
    }
}

