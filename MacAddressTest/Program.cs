using Nancy.Json;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace MacAddressTest
{

    class Program
    {
        static void Main()
        {
            string GetMacaddress()
            {
                string addr = "";
                foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (n.OperationalStatus == OperationalStatus.Up)
                    {
                        addr += n.GetPhysicalAddress().ToString();
                        break;
                    }
                }
                return addr;
            }

            string MacData = GetMacaddress();

            string PostJson()
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://APIURL");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        MacAddress = MacData
                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }

            string response = PostJson();
        }
    }

  
}
