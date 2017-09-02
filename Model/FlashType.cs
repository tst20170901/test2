using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class FlashType
    {
        public int ID { get; set; }
        public string TypeName { get; set; }
        public int ParentID { get; set; }
        public int SortID { get; set; }
    }
}