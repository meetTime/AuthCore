using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthCore.Model
{
    public class AddSessionRequest
    {
        /// <summary>
        /// 账号Id
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string ClientIpAddress { get; set; }

        /// <summary>
        /// 用户代理信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 有效时间 （单位：分钟）
        /// </summary>
        public int VaildTime { get; set; }
    }

    public class SessionResponse
    {
        public int SessionId { get; set; }

        /// <summary>
        /// 账号信息
        /// </summary>
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public int OwnerType { get; set; }

        public int OwnerId { get; set; }

        /// <summary>
        /// IP 地址
        /// </summary>
        public string ClientIpAddress { get; set; }

        /// <summary>
        /// 用户代理信息
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public string VaildTime
        {
            get
            {
                return DateTimeUtls.GetDateTimeFormat(LoginTime, ExpireTime);
            }
        }

        /// <summary>
        /// 剩余有效时间
        /// </summary>
        public string SurplusVaildTime
        {
            get
            {
                return DateTimeUtls.GetDateTimeFormat(DateTime.Now, ExpireTime);
            }
        }
    }

    public class SessionQuery : PagerQuery
    {
        /// <summary>
        /// 用户
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// 账号所属系统类型
        /// </summary>
        public int? OwnerType { get; set; }

        /// <summary>
        /// 模糊查询IP等信息
        /// </summary>
        public string Keywords { get; set; }
    }

    public class DateTimeUtls
    {
        public static string GetDateTimeFormat(DateTime startTime, DateTime endTime)
        {
            TimeSpan ts = endTime - startTime;
            int totalMinutes = (int)ts.TotalMinutes;
            if (totalMinutes <= 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            int days = totalMinutes / (60 * 24);
            if (days > 0)
            {
                sb.AppendFormat("{0}天", days);
                totalMinutes -= days * (60 * 24);
            }

            int hours = totalMinutes / (60);
            if (hours > 0)
            {
                sb.AppendFormat("{0}小时", hours);
                totalMinutes -= hours * (60);
            }
            if (totalMinutes > 0)
            {
                sb.AppendFormat("{0}分", totalMinutes);
            }

            return sb.ToString();
        }
    }
}
