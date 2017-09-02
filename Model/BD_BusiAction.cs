using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace Models
{
    [Serializable]
    public class BD_BusiAction
    {
        public int ID { get; set; }
        public string ActionName { get; set; }
        public string Body { get; set; }
        public int SortID { get; set; }
        public int BranchID { get; set; }
        /// <summary>
        /// 10正常 30禁用
        /// </summary>
        public int ActionState { get; set; }
        /// <summary>
        /// 0所有用户，10新用户
        /// </summary>
        public int IsNewUser { get; set; }
        /// <summary>
        /// 0普通领券，只填手机号，10填入车辆信息
        /// </summary>
        public int DataType { get; set; }
        public string SMSContent { get; set; }
        public DateTime CDate { get; set; }
    }
}