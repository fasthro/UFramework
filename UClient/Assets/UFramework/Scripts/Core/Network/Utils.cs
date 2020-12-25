/*
 * @Author: fasthro
 * @Date: 2020-12-16 16:41:29
 * @Description: utils
 */
using System.Net;

namespace UFramework.Network
{
    public class Utils
    {
        public static string GetIPHost(string domain)
        {
            domain = domain.Replace("http://", "").Replace("https://", "");
            IPHostEntry hostEntry = Dns.GetHostEntry(domain);
            IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
            return ipEndPoint.Address.ToString();
        }
    }
}