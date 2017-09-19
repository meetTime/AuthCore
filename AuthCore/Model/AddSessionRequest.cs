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
        /// 授权信息
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }

    public class SessionResponse: AddSessionRequest
    {
        public int SessionId { get; set; }

        public DateTime LoginTime { get; set; }
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
}
