using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace AliceDAL
{
    /// <summary>
    /// �󶨿ؼ�������
    /// </summary>
    public class BindControlData
    {
        public BindControlData()
        {

        }
        #region ��List�ؼ�������

        /// <summary>
        /// ���б�ؼ������ݣ�����ԴΪSqlDataReader����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, SqlDataReader dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///������
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
            ///�ر�����Դ
            dataSource.Close();
        }

        /// <summary>
        /// ���б�ؼ������ݣ�����ԴΪDataSet����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, DataSet dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///������
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
        }

        /// <summary>
        /// ���б�ؼ������ݣ�����ԴΪDataTable����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, DataTable dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///������
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
        }

        /// <summary>
        /// ���б�ؼ������ݣ�����ԴΪDataView����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindListData(ListControl list, DataView dataSource,
            string dataTextField, string dataValueField)
        {
            if (dataSource == null) { return; }
            ///������
            list.DataSource = dataSource;
            list.DataTextField = dataTextField;
            list.DataValueField = dataValueField;
            list.DataBind();
        }

        #endregion

        #region ��DataList�ؼ�������

        /// <summary>
        /// ��DataList�ؼ������ݣ�����ԴΪSqlDataReader����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        public static void BindDataListData(DataList list, SqlDataReader dataSource)
        {
            if (dataSource == null) { return; }
            ///������
            list.DataSource = dataSource;
            list.DataBind();
            ///�ر�����Դ
            dataSource.Close();
        }

        /// <summary>
        /// ��DataList�ؼ������ݣ�����ԴΪDataSet����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        public static void BindDataListData(DataList list, DataSet dataSource)
        {
            if (dataSource == null) { return; }
            ///������
            list.DataSource = dataSource;
            list.DataBind();
        }

        /// <summary>
        /// ��DataList�ؼ������ݣ�����ԴΪDataTable����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dataSource"></param>
        public static void BindDataListData(DataList list, DataTable dataSource)
        {
            if (dataSource == null) { return; }
            ///������
            list.DataSource = dataSource;
            list.DataBind();
        }

        #endregion

        #region ��GridView�ؼ�������

        /// <summary>
        /// ��GridView�ؼ������ݣ�����ԴΪSqlDataReader����
        /// </summary>
        /// <param name="gv">GridView�ؼ�</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, SqlDataReader dataSource)
        {
            if (dataSource == null) { return; }
            ///������
            gv.DataSource = dataSource;
            gv.DataBind();
            ///�ر�����Դ
            dataSource.Close();
        }

        /// <summary>
        /// ��GridView�ؼ������ݣ�����ԴΪDataSet����
        /// </summary>
        /// <param name="gv">GridView�ؼ�</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, DataSet dataSource)
        {
            if (dataSource == null) { return; }
            ///������
            gv.DataSource = dataSource;
            gv.DataBind();
        }

        /// <summary>
        /// ��GridView�ؼ������ݣ�����ԴΪDataTable����
        /// </summary>
        /// <param name="gv">GridView�ؼ�</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, DataTable dataSource)
        {
            //if (dataSource == null) { return; }
            ///������
            gv.DataSource = dataSource;
            gv.DataBind();
        }

        /// <summary>
        /// ��GridView�ؼ������ݣ�����ԴΪDataView����
        /// </summary>
        /// <param name="gv">GridView�ؼ�</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, DataView dataSource)
        {
            if (dataSource == null) { return; }
            ///������
            gv.DataSource = dataSource;
            gv.DataBind();
        }
        public static void BindRepeaterData(Repeater rep, DataTable dataSource)
        {
            //if (dataSource == null) { return; }
            ///������
            rep.DataSource = dataSource;
            rep.DataBind();
        }
        /// <summary>
        /// ��GridView�ؼ������ݣ�����ԴΪDataView����
        /// </summary>
        /// <param name="gv">GridView�ؼ�</param>
        /// <param name="dataSource"></param>
        public static void BindGridViewData(GridView gv, object dataSource)
        {
            if (dataSource == null) { return; }
            ///������
            gv.DataSource = dataSource;
            gv.DataBind();
        }

        #endregion
    }
}
