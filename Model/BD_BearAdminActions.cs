using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable]
    public class BD_BearAdminActions
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Controller
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// ActionImg
        /// </summary>
        public string ActionImg { get; set; }
        /// <summary>
        /// ParentID
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// SortID
        /// </summary>
        public int SortID { get; set; }
        /// <summary>
        /// 1为菜单显示
        /// </summary>
        public int IsMenu { get; set; }
    }
}