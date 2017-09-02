using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace AliceDAL
{
    public class ImageBase64Helper
    {
        //图片转为base64编码的文本  
        public static string ImgToBase64String(string Imagefilename)
        {
            Bitmap bmp = (Bitmap)Image.FromFile(Imagefilename);
            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            stream.Position = 0;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            stream.Close();
            string base64String = string.Empty;
            try
            {
                base64String = System.Convert.ToBase64String(data, 0, data.Length);
                return base64String;
            }
            catch (Exception ex)
            {
                Error.WriteErrorLog(ex);
                return null;
            }
            finally
            {
                bmp.Dispose();
            }
        }
        public static string Base64StringToImage(string base64)
        {
            try
            {
                if (base64.Length % 4 != 0)
                {
                    base64 = base64.Substring(0, base64.Length - (base64.Length % 4));
                }
                byte[] bitmapData = new byte[base64.Length];
                bitmapData = Convert.FromBase64String(base64);
                MemoryStream streamBitmap = new MemoryStream(bitmapData);
                string path = Common.GetMapPath("/uploadfiles/worker/");
                string name = "idodo" + DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + ".jpg";
                string newname = "/uploadfiles/worker/" + name;

                Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
                bitImage.Save(path + name, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitImage.Dispose();
                return newname;
            }
            catch (Exception ex)
            {
                Error.WriteErrorLog(ex);
                Error.WriteErrorLog(base64);
                return null;
            }
        }
    }
}
