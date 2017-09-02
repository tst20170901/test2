using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace AliceDAL
{
    /// <summary>
    /// 绑定控件的数据
    /// </summary>
    public class BindControlData
    {
        public BindControlData()
        {

        }
        #region 绑定List控件的数据

        /// <summary>
        /// 绑定列表控件的数据，数据源为SqlDataReader对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, SqlDataReader dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
            ///关闭数据源
            dataSource.Close();
        }

        /// <summary>
        /// 绑定列表控件的数据，数据源为DataSet对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, DataSet dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
        }

        /// <summary>
        /// 绑定列表控件的数据，数据源为DataTable对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, DataTable dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
        }

        /// <summary>
        /// 绑定列表控件的数据，数据源为DataView对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, DataView dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
        }

        #endregion

        #region 绑定DataList控件的数据

        /// <summary>
        /// 绑定DataList控件的数据，数据源为SqlDataReader对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        public static void BindDataListData(DataList list, SqlDataReader dataSource)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            list.DataSource = dataSource;
            list.DataBind();
            ///关闭数据源
            dataSource.Close();
        }

        /// <summary>
        /// 绑定DataList控件的数据，数据源为DataSet对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        public static void BindDataListData(DataList list, DataSet dataSource)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            list.DataSource = dataSource;
            list.DataBind();
        }

        /// <summary>
        /// 绑定DataList控件的数据，数据源为DataTable对象
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        public static void BindDataListData(DataList list, DataTable dataSource)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            list.DataSource = dataSource;
            list.DataBind();
        }

        #endregion

        #region 绑定GridView控件的数据

        /// <summary>
        /// 绑定GridView控件的数据，数据源为SqlDataReader对象
        /// </summary>
        /// <param name="gv">GridView控件</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, SqlDataReader dataSource)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            gv.DataSource = dataSource;
            gv.DataBind();
            ///关闭数据源
            dataSource.Close();
        }

        /// <summary>
        /// 绑定GridView控件的数据，数据源为DataSet对象
        /// </summary>
        /// <param name="gv">GridView控件</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, DataSet dataSource)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            gv.DataSource = dataSource;
            gv.DataBind();
        }

        /// <summary>
        /// 绑定GridView控件的数据，数据源为DataTable对象
        /// </summary>
        /// <param name="gv">GridView控件</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, DataTable dataSource)
        {
            //if (dataSource == null) { return; }
            ///绑定数据
            gv.DataSource = dataSource;
            gv.DataBind();
        }

        /// <summary>
        /// 绑定GridView控件的数据，数据源为DataView对象
        /// </summary>
        /// <param name="gv">GridView控件</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, DataView dataSource)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            gv.DataSource = dataSource;
            gv.DataBind();
        }
        public static void BindRepeaterData(Repeater rep, DataTable dataSource)
        {
            //if (dataSource == null) { return; }
            ///绑定数据
            rep.DataSource = dataSource;
            rep.DataBind();
        }
        /// <summary>
        /// 绑定GridView控件的数据，数据源为DataView对象
        /// </summary>
        /// <param name="gv">GridView控件</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, object dataSource)
        {
            if (dataSource == null) { return; }
            ///绑定数据
            gv.DataSource = dataSource;
            gv.DataBind();
        }

        #endregion
    }
}
