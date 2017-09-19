using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthCore.Model
{
    public class AccountCommon
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属系统类型
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        /// 所属系统Id
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPassword { get; set; }

        /// <summary>
        /// api账号
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// api密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string SignKey { get; set; }

        /// <summary>
        /// IP白名单
        /// </summary>
        public string IpWhiteList { get; set; }
    }

    public class AddAccountRequest: AccountCommon
    {
        /// <summary>
        /// 分配角色
        /// </summary>
        public List<int> RoleIds { get; set; }
    }

    public class AccountResponse : AccountCommon
    {
        public int AccountId { get; set; }
    }

    public enum RecordStatus
    {
        /// <summary>
        /// 正常的
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 禁用的
        /// </summary>
        Disabled = 1
    }
}
