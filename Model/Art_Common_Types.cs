using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class Art_Common_Types
    {
        public int ID { get; set; }
        public string TypeName { get; set; }
        public int ParentID { get; set; }
        public int SortID { get; set; }
        public int Layer { get; set; }
        public int Haschild { get; set; }
        private string path;
        public string Path
        {
            get { return path; }
            set { path = value.TrimEnd(); }
        }
    }
}
