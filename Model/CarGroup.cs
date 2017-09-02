using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class CarGroup
    {
        public int ID { get; set; }
        public string TypeName { get; set; }
        public int ParentID { get; set; }
        public int SortID { get; set; }
        public int UserID { get; set; }
    }
}