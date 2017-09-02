using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class BD_Logs
    {
        public int ID { get; set; }
        private string loginID;

        public string LoginID
        {
            get { return loginID; }
            set { loginID = value.TrimEnd(); }
        }
        private string nickname;
        public string NickName
        {
            get { return nickname; }
            set { nickname = value.TrimEnd(); }
        }
        public string Description { get; set; }
        public string IP { get; set; }
        public DateTime CDate { get; set; }
    }
}

