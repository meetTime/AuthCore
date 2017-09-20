using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthCore.Dal;
using AuthCore.Model;
using Victory.Dao;

namespace AuthCore.Logic
{
    public class AuthLogic
    {
        #region Token
        public static AccountResponse VerifyAccount(int ownerType, int ownerId, string loginName, string password)
        {
            var account = AuthDal.GetAccountByLoginName(loginName, ownerType, ownerId);
            VerifyAccount(account);

            if (account.LoginPassword!=password)
            {
                throw new Exception("账号密码错误！！！");
            }

            return account;
        }
        public static AccountResponse VerifyAccount(string appKey, string appSecret)
        {
            var account= AuthDal.GetAccountByAppKey(appKey, appSecret);
            VerifyAccount(account);

            return account;
        }
        public static AccountResponse VerifyAccount(string signKey)
        {
            var account = AuthDal.GetAccountBySignKey(signKey);
            VerifyAccount(account);

            return account;
        }
        private static void VerifyAccount(AccountResponse account)
        {
            if (account == null)
            {
                throw new Exception("账号不存在！！！");
            }

            if (account.RecordStatus == (int)RecordStatus.Disabled)
            {
                throw new Exception("账号被禁用了！！！");
            }
        }

        public static SessionResponse GetSessionByToken(string token)
        {
            var session= AuthDal.GetSessionByToken(token);
            return session;
        }

        public static void ReleaseToken(string token)
        {
            AuthDal.ReleaseToken(token);
        }
        #endregion

        #region Account
        private object AddAccountLocker = new object();

        public static int AddAccount(AddAccountRequest request)
        {
            var locker = string.Intern(request.OwnerId.ToString());
            lock (locker)
            {
                //check
                CheckModelIsNull(request.LoginName, 50, "登录名称");
                CheckModelIsNull(request.LoginPassword, 50, "登录密码");

                var result = AuthDal.GetAccountByLoginName(request.LoginName, request.OwnerType, request.OwnerId);
                if (result != null)
                {
                    throw new Exception(string.Format("登录名称：【{0}】已经存在！！！", request.LoginName));
                }

                if (!string.IsNullOrEmpty(request.AppKey))
                {
                    if (AuthDal.IsExistsAppKey(request.AppKey) > 0)
                    {
                        throw new Exception(string.Format("AppKey：[{0}] 已存在！！！", request.AppKey));
                    }
                }

                var obj = AuthDal.AddAccount(request);
                return obj;
            }
        }

        public static List<AccountResponse> GetAccountsByIds(List<int> accountIds)
        {
            var obj = AuthDal.GetAccountsByIds(accountIds);
            return obj;
        }

        public static AccountResponse GetAccount(int accountId)
        {
            var list = GetAccountsByIds(new List<int>() { accountId });
            if (list.Count==0)
            {
                return null;
            }

            return list[0];
        }

        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private static AccountResponse IsExistsAccount(int accountId)
        {
            var obj = GetAccount(accountId);
            if (obj==null)
            {
                throw new Exception(string.Format("账号不存在！！！ 【AccountId={0}】", accountId));
            }

            return obj;
        }

        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="accountId"></param>
        public static void DeleteAccount(int accountId)
        {
            AuthDal.DeleteAccount(accountId);
        }

        /// <summary>
        /// 启用账号
        /// </summary>
        /// <param name="accountId"></param>
        public static void EnableAccount(int accountId)
        {
            IsExistsAccount(accountId);

            AuthDal.EditAccountRecordStatus(accountId,RecordStatus.Normal);
        }

        /// <summary>
        /// 禁用账号
        /// </summary>
        /// <param name="accountId"></param>
        public static void DisableAccount(int accountId)
        {
            IsExistsAccount(accountId);

            AuthDal.EditAccountRecordStatus(accountId, RecordStatus.Disabled);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newPassword"></param>
        public static void EditLoginPassword(int accountId, string newPassword,string oldPassword)
        {
            var account= IsExistsAccount(accountId);
            CheckModelIsNull(oldPassword, 50, "旧密码");
            CheckModelIsNull(newPassword, 50, "新密码");

            if (account.LoginPassword!=oldPassword)
            {
                throw new Exception("旧密码错误！！！");
            }
            AccountIsCanOperate(account.RecordStatus, "修改密码");

            AuthDal.EditLoginPassword(accountId, newPassword);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="loginPassword"></param>
        public static void ResetLoginPassword(int accountId, string loginPassword)
        {
            var account=IsExistsAccount(accountId);
            CheckModelIsNull(loginPassword, 50, "密码");
            AccountIsCanOperate(account.RecordStatus, "重置密码");

            AuthDal.EditLoginPassword(accountId, loginPassword);
        }

        /// <summary>
        /// 编辑App 秘钥
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        public static void EditAppKeyandAppSecret(int accountId, string appKey, string appSecret)
        {
            var account=IsExistsAccount(accountId);
            CheckModelIsNull(appKey, 50, "AppKey");
            CheckModelIsNull(appSecret, 50, "AppSecret");

            if (AuthDal.IsExistsAppKey(appKey, accountId) > 0)
            {
                throw new Exception(string.Format("AppKey：[{0}] 已存在！！！",appKey));
            }

            AccountIsCanOperate(account.RecordStatus, "编辑AppKey 与 AppSecret");

            AuthDal.EditAppKeyandAppSecret(accountId, appKey, appSecret);
        }

        /// <summary>
        /// 编辑IP白名单
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ipWhiteList">多个IP地址用逗号隔开</param>
        public static void EditIpWhiteList(int accountId, string ipWhiteList)
        {
            var account=IsExistsAccount(accountId);
            CheckModelIsNull(ipWhiteList, 50, "IP白名单");
            AccountIsCanOperate(account.RecordStatus, "编辑IP白名单");

            AuthDal.EditIpWhiteList(accountId, ipWhiteList);
        }

        private static void AccountIsCanOperate(int recordStatus,string remark)
        {
            if (recordStatus==(int)RecordStatus.Disabled)
            {
                throw new Exception(string.Format("账号被禁用了,不能{0}！！！",remark));
            }
        }

        /// <summary>
        /// 自动生成不重复随机的 8位AppKey 和 32位AppSecret
        /// </summary>
        /// <returns></returns>
        public static AppKeyAndSecret CreateAppKeyAndSecret()
        {
            AppKeyAndSecret app = new AppKeyAndSecret();

            Random ro = new Random();
            for (int i = 0; i < 100; i++)
            {
                app.AppKey = ro.Next(9999999, 99999999).ToString();
                if (AuthDal.IsExistsAppKey(app.AppKey) <= 0)
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(app.AppKey))
            {
                throw new Exception(string.Format("AppKey 与 AppSecret 生成失败，请重试！！！"));
            }

            app.AppSecret = Guid.NewGuid().ToString().Replace("-", "");

            return app;
        }

        /// <summary>
        /// 自动生成加密的SignKey并保存到数据库 格式：OwnerType=0&OwnerId=0&LoginName=ceshi&LoginPassword=123123
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static string CreateSignKey(int accountId)
        {
            var account = IsExistsAccount(accountId);

            var str = string.Format("OwnerType={0}&OwnerId={1}&LoginName={2}&LoginPassword={3}", account.OwnerType, account.OwnerId, account.LoginName, account.LoginPassword);

            var signKey = MD5Encrypt32(str);

            //修改SignKey
            AuthDal.EditSignKey(accountId, signKey);

            return signKey;
        }

        /// <summary>
        /// 32位的MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string MD5Encrypt32(string str)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string signKey = "";
            for (int i = 0; i < s.Length; i++)
            {
                signKey = signKey + s[i].ToString("X");
            }

            return signKey;
        }

        #endregion

        #region RoleAccount
        public static int AddRoleAccount(AddRoleAccountRequest request)
        {
            IsExistsAccount(request.AccountId);
            if (request.RoleIds==null || request.RoleIds.Count==0)
            {
                throw new Exception("请给账号分配角色！！！");
            }

            var obj = AuthDal.AddRoleAccount(request);
            return obj;
        }
        #endregion

        #region Role
        public static int AddRole(AddRoleRequest request)
        {
            //check
            CheckModelIsNull(request.Name, 50, "角色名称");

            var result = AuthDal.IsExistsRoleByName(request.Name, request.OwnerType, request.DataDimensionId);
            if (result>0)
            {
                throw new Exception(string.Format("角色名称：【{0}】已经存在！！！", request.Name));
            }

            var obj = AuthDal.AddRole(request);
            return obj;
        }

        public static void EditRole(EditRoleRequest request)
        {
            CheckModelIsNull(request.Name, 50, "角色名称");
            var role = GetRole(request.RoleId);
            if (role==null)
            {
                throw new Exception(string.Format("角色不存在！！！【RoleId={0}】",request.RoleId));
            }

            var result = AuthDal.IsExistsRoleByName(request.Name,role.OwnerType,role.DataDimensionId,request.RoleId);
            if (result>0)
            {
                throw new Exception(string.Format("角色名称：【{0}】已经存在！！！", request.Name));
            }

            AuthDal.EditRole(request);
        }

        public static PagerModelCollection<RoleResponse> GetRoles(RoleQuery query)
        {
            var obj = AuthDal.GetRoles(query);
            return obj;
        }

        public static RoleResponse GetRole(int roleId)
        {
            var obj = AuthDal.GetRole(roleId);
            return obj;
        }

        public static List<RoleResponse> GetRolesByAccountId(int accountId)
        {
            var obj = AuthDal.GetRolesByAccountId(accountId);
            return obj;
        }
        #endregion

        #region Session
        public static SessionResponse AddAndGetSession(AddSessionRequest request)
        {
            var sessionId = AddSession(request);
            return AuthDal.GetSession(sessionId);
        }
        public static int AddSession(AddSessionRequest request)
        {
            IsExistsAccount(request.AccountId);
            CheckModelIsNull(request.ClientIpAddress,50,"IP地址");
            CheckModelIsNull(request.VaildTime, "有效时间");

            var obj = AuthDal.AddSession(request);
            return obj;
        }

        public static PagerModelCollection<SessionResponse> GetSessions(SessionQuery query)
        {
            var obj = AuthDal.GetSessions(query);
            return obj;
        }
        #endregion

        #region checkModel
        private static void CheckModelIsNull(string value, int strMaxLenght, string modelTag)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception(string.Format("【{0}】不能为空！！！", modelTag));
            }

            if (value.Length > strMaxLenght)
            {
                throw new Exception(string.Format("【{0}】的长度不能超过：{1}！！！", modelTag, strMaxLenght));
            }
        }

        private static void CheckModelIsNull(int value, string modelTag)
        {
            if (value <= 0)
            {
                throw new Exception(string.Format("【{0}】不能为空！！！", modelTag));
            }
        }
        #endregion
    }
}
