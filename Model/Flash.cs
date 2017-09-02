using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class Flash
    {
        public int ID { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int SortID { get; set; }
        public int TypeID { get; set; }
    }
}

