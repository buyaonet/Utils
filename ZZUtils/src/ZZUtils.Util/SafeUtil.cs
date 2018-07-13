using System.Text.RegularExpressions;
using System.Web;

namespace ZZUtils.Util
{
    public class SafeUtil
    {
        private const string StrRegex = "['\"]+|" +
                                 @"\b(alert|confirm|prompt)\b|^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";

        /// <summary>
        /// 检查传递的参数是否合法(post)
        /// </summary>
        /// <returns></returns>
        public static bool PostData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Form[i].ToString());
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 检查传递的参数是否合法(get)
        /// </summary>
        /// <returns></returns>
        public static bool GetData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.QueryString[i].ToString());
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 检查cookie是否合法
        /// </summary>
        /// <returns></returns>
        public static bool CookieData()
        {
            bool result = false;
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Cookies[i].Value.ToLower());
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 检查refer数据是否合法
        /// </summary>
        /// <returns></returns>
        public static bool Referer()
        {
            bool result = false;
            return result = CheckData(HttpContext.Current.Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// 检查是否合法
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool CheckData(string inputData)
        {
            return Regex.IsMatch(inputData,StrRegex);
        }
    }
}
