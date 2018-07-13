using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web;


namespace ZZUtils.Util
{
    public class IPUtil
    {

        public static string Ip
        {
            get
            {
                var result = string.Empty;
                if (HttpContext.Current != null)
                    result = GetWebClientIp();
                if (string.IsNullOrEmpty(result))
                    result = GetLanIp();
                return result;
            }
        }

        /// <summary>
        /// 获取web客户端ip
        /// </summary>
        /// <returns></returns>
        private static string GetWebClientIp()
        {
            var ip = GetWebRemoteIp();
            foreach (var hostAddress in Dns.GetHostAddresses(ip))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取web远程ip
        /// </summary>
        /// <returns></returns>
        private static string GetWebRemoteIp()
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// 获取局域网ip
        /// </summary>
        /// <returns></returns>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断所给IP是否在IP范围内
        /// </summary>
        /// <param name="strStartIP">起始IP地址</param>
        /// <param name="strEndIP">结束IP地址</param>
        /// <param name="strHostIP">要判断的IP地址</param>
        /// <returns>在IP范围内返回true，否则返回false</returns>
        public static bool IsInIPRange(string strStartIP, string strEndIP, string strHostIP)
        {
            if (string.IsNullOrWhiteSpace(strStartIP) || string.IsNullOrWhiteSpace(strEndIP) || string.IsNullOrWhiteSpace(strHostIP))
                return false;
            long iStartIP = GetIPValue(strStartIP);
            long iEndIP = GetIPValue(strEndIP);
            long iHostIP = GetIPValue(strHostIP);
            if (iHostIP >= iStartIP && iHostIP <= iEndIP)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 计算出IP具体的值
        /// </summary>
        /// <param name="strHostIP"></param>
        /// <returns></returns>
        private static long GetIPValue(string strHostIP)
        {
            long nRetValue = 0;
            long nCurValue = 0;
            try
            {
                IPAddress userIP = IPAddress.Parse(strHostIP);
                Byte[] bytes = userIP.GetAddressBytes();
                for (int i = 0; i < bytes.Length; i++)
                {
                    nCurValue = bytes[i];
                    if (i == 0)
                        nCurValue = nCurValue << 24;
                    else
                        if (i == 1)
                        nCurValue = nCurValue << 16;
                    else
                            if (i == 2)
                        nCurValue = nCurValue << 8;
                    nRetValue += nCurValue;
                }
            }
            catch
            {
            }
            return nRetValue;
        }

        /// <summary>
        /// 判断是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}
