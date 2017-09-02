using System.Web;
using System.Web.UI;
namespace AliceDAL
{
    public class MessageBox : System.Web.UI.Page
    {
        public static string ShowDialog(string message, string url)
        {
            string result = "";
            if (url == "") result = "<script type=\"text/javascript\">alert(\"" + message + "\");</script>";
            else if (url == "#") result = "<script type=\"text/javascript\">alert(\"" + message + "\");self.location=self.location;</script>";
            else result = "<script type=\"text/javascript\">alert(\"" + message + "\");self.location=\"" + url + "\";</script>";
            return result;
        }
    }
}
