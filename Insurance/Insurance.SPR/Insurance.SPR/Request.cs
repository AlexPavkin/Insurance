using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insurance_SPR
{
   public  class Request
    {

        public static string Pars(string strSource, string strStart, string strEnd)
        {
            var str = "";
            var length = strStart.Length;
            var num1 = strSource.IndexOf(strStart, StringComparison.Ordinal);
            var num2 = strSource.IndexOf(strEnd, num1 + length, StringComparison.Ordinal);
            if (num1 != -1 & num2 != -1)
            {
                str = strSource.Substring(num1 + length, num2 - (num1 + length));
            }
            return str;
        }

        public static string POST(string Url, string Data)
        {
            
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            req.Method = "POST";
            req.Timeout = 100000;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("", "");
            byte[] sentData = Encoding.GetEncoding(1251).GetBytes(Data);
            req.ContentLength = sentData.Length;
            System.IO.Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            System.Net.WebResponse res = req.GetResponse();
            System.IO.Stream ReceiveStream = res.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
            //Кодировка указывается в зависимости от кодировки ответа сервера
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0)
            {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }
            return Out;
        }
        public static string GET(string Url)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }
    }
}
