using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class BD_Business
    {
        public int ID { get; set; }
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string BusinessName { get; set; }
        public decimal Wallet { get; set; }
        public int SortID { get; set; }
        public int TypeID { get; set; }
        public int BusinessState { get; set; }
        public int BranchID { get; set; }
        public DateTime CDate { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        public string Data7 { get; set; }
        public string Data8 { get; set; }
        public string Data9 { get; set; }
        public string Data10 { get; set; }
    }
}