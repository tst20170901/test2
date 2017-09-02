using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class AB_SinglePage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string TitleWeb { get; set; }
        public string KeyWords { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Img { get; set; }
        public int SortID { get; set; }
        public int Lan { get; set; }
        public int TypeID { get; set; }
    }
}

