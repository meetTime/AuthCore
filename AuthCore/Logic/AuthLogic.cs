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
        #region Account
        public static int AddAccount(AddAccountRequest request)
        {
            //check
            CheckModelIsNull(request.OwnerType,"所属系统类型");
            CheckModelIsNull(request.LoginName,50,"登录名称");
            CheckModelIsNull(request.LoginPassword, 50, "登录密码");

            var result = GetAccountByLoginName(request.LoginName,request.OwnerType,request.OwnerId);
            if (result!=null)
            {
                throw new Exception(string.Format("登录名称：【{0}】已经存在！！！", request.LoginName));
            }

            var obj = AuthDal.AddAccount(request);
            return obj;
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
        public static AccountResponse IsExistsAccount(int accountId)
        {
            var obj = GetAccount(accountId);
            if (obj==null)
            {
                throw new Exception(string.Format("账号不存在！！！ 【AccountId={0}】", accountId));
            }

            return obj;
        }

        public static AccountResponse GetAccountByLoginName(string loginName, int ownerType, int ownerId)
        {
            var obj = AuthDal.GetAccountByLoginName(loginName, ownerType,ownerId);
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
        /// 禁用张哈
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
        /// <param name="loginPassword"></param>
        public static void EditLoginPassword(int accountId, string loginPassword)
        {
            IsExistsAccount(accountId);
            CheckModelIsNull(loginPassword, 50, "登录密码");

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
            IsExistsAccount(accountId);
            CheckModelIsNull(appKey, 50, "AppKey");
            CheckModelIsNull(appSecret, 50, "AppSecret");

            var obj = AuthDal.IsExistsAppKey(accountId, appKey);
            if (obj>0)
            {
                throw new Exception(string.Format("AppKey：[{0}] 已存在！！！",appKey));
            }

            AuthDal.EditAppKeyandAppSecret(accountId, appKey, appSecret);
        }

        /// <summary>
        /// 编辑签名秘钥
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="signKey"></param>
        public static void EditSignKey(int accountId, string signKey)
        {
            IsExistsAccount(accountId);
            CheckModelIsNull(signKey, 50, "签名秘钥");

            AuthDal.EditSignKey(accountId, signKey);
        }

        /// <summary>
        /// 编辑IP白名单
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ipWhiteList"></param>
        public static void EditIpWhiteList(int accountId, string ipWhiteList)
        {
            IsExistsAccount(accountId);
            CheckModelIsNull(ipWhiteList, 50, "IP白名单");

            AuthDal.EditIpWhiteList(accountId, ipWhiteList);
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
            CheckModelIsNull(request.OwnerType, "所属系统类型");
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
        public static int AddSession(AddSessionRequest request)
        {
            CheckModelIsNull(request.ClientIpAddress,50,"IP地址");
            CheckModelIsNull(request.Token, 50, "Token授权信息");

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
        public static void CheckModelIsNull(string value, int strMaxLenght, string modelTag)
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

        public static void CheckModelIsNull(int value, string modelTag)
        {
            if (value <= 0)
            {
                throw new Exception(string.Format("【{0}】不能为空！！！", modelTag));
            }
        }
        #endregion
    }
}
