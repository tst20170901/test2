using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class Supplier
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ActArea { get; set; }
        public int SortID { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public int IsShow { get; set; }
        public DateTime CDate { get; set; }
    }
}

