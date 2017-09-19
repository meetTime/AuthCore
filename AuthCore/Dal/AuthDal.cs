using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthCore.Model;
using Victory.Dao;

namespace AuthCore.Dal
{
    public class AuthDal
    {
        #region Account
        public static int AddAccount(AddAccountRequest request)
        {
            var session = DataSources.Default.CreateSession();
            session.BeginTransaction();
            try
            {
                var accountId = AddAccount(request, session);
                if (request.RoleIds!=null && request.RoleIds.Count>0)
                {
                    AddRoleAccount(accountId, request.RoleIds, session);
                }
                session.CommitTranscation();
                return accountId;
            }
            catch (Exception e)
            {
                session.RollbackTranscation();
                throw e;
            }
            finally
            {
                session.Dispose();
            }
        }
        private static int AddAccount(AddAccountRequest request, IDataAccessSession session)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Name", request.Name);
            ht.Add("OwnerType", request.OwnerType);
            ht.Add("OwnerId", request.OwnerId);
            ht.Add("LoginName", request.LoginName);
            ht.Add("LoginPassword", request.LoginPassword);
            ht.Add("AppKey", request.AppKey);
            ht.Add("AppSecret", request.AppSecret);
            ht.Add("SignKey", request.SignKey);
            ht.Add("IpWhiteList", request.IpWhiteList);
            ht.Add("RecordStatus", (int)RecordStatus.Normal);

            return Convert.ToInt32(DataSources.Default.ExecuteScalar("AddAccount", session, null,ht));
        }

        public static List<AccountResponse> GetAccountsByIds(List<int> accountIds)
        {
            if (accountIds==null || accountIds.Count==0)
            {
                return new List<AccountResponse>();
            }

            return DataSources.Default.QueryCollection<AccountResponse>("GetAccountsByIds",string.Join(",", accountIds)).ToList();
        }

        public static AccountResponse GetAccountByLoginName(string loginName, int ownerType,int ownerId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("LoginName", loginName);
            ht.Add("OwnerType", ownerType);
            ht.Add("OwnerId", ownerId);

            return DataSources.Default.Query<AccountResponse>("GetAccountByLoginName", ht);
        }

        public static void DeleteAccount(int accountId)
        {
            DataSources.Default.ExecuteNonQuery("DeleteAccount", accountId);
        }

        public static int EditAccountRecordStatus(int accountId,RecordStatus status)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AccountId", accountId);
            ht.Add("RecordStatus", (int)status);

            return DataSources.Default.ExecuteNonQuery("EditAccountRecordStatus", null,null,ht);
        }

        public static int EditLoginPassword(int accountId,string loginPassword)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AccountId", accountId);
            ht.Add("LoginPassword", loginPassword);

            return DataSources.Default.ExecuteNonQuery("EditLoginPassword", null, null, ht);
        }

        public static int EditAppKeyandAppSecret(int accountId, string appKey,string appSecret)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AccountId", accountId);
            ht.Add("AppKey", appKey);
            ht.Add("AppSecret", appSecret);

            return DataSources.Default.ExecuteNonQuery("EditAppKeyandAppSecret", null, null, ht);
        }

        public static int EditSignKey(int accountId, string signKey)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AccountId", accountId);
            ht.Add("SignKey", signKey);

            return DataSources.Default.ExecuteNonQuery("EditSignKey", null, null, ht);
        }

        public static int EditIpWhiteList(int accountId, string ipWhiteList)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AccountId", accountId);
            ht.Add("IpWhiteList", ipWhiteList);

            return DataSources.Default.ExecuteNonQuery("EditIpWhiteList", null, null, ht);
        }

        public static int IsExistsAppKey(int accountId, string appKey)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AccountId", accountId);
            ht.Add("AppKey", appKey);

            return Convert.ToInt32(DataSources.Default.ExecuteScalar("IsExistsAppKey", null,null,ht));
        }
        #endregion

        #region RoleAccount
        public static int AddRoleAccount(AddRoleAccountRequest request)
        {
            var session = DataSources.Default.CreateSession();
            session.BeginTransaction();
            try
            {
                var result= AddRoleAccount(request.AccountId, request.RoleIds, session);
                session.CommitTranscation();
                return result;
            }
            catch (Exception e)
            {
                session.RollbackTranscation();
                throw e;
            }
            finally
            {
                session.Dispose();
            }
        }
        private static int AddRoleAccount(int accountId,List<int> roleIds,IDataAccessSession session)
        {
            DeleteRoleAccount(accountId, session);

            Hashtable ht;
            int resultRow = 0;
            foreach (var roleId in roleIds)
            {
                ht = new Hashtable();
                ht.Add("AccountId", accountId);
                ht.Add("RoleId", roleId);
                resultRow += Convert.ToInt32(DataSources.Default.ExecuteScalar("AddRoleAccount", session,null,ht));
            }

            return resultRow;
        }
        private static int DeleteRoleAccount(int accountId,IDataAccessSession session)
        {
            Hashtable ht=new Hashtable();
            ht.Add("AccountId", accountId);

            return DataSources.Default.ExecuteNonQuery("DeleteRoleAccount", session,null, ht);
        }
        #endregion

        #region Role
        public static int AddRole(AddRoleRequest request)
        {
            Hashtable ht = new Hashtable();
            ht.Add("OwnerType", request.OwnerType);
            ht.Add("Name", request.Name);
            ht.Add("Remark", request.Remark);
            ht.Add("PermissionIds", request.PermissionIds);
            ht.Add("DataDimensionId", request.DataDimensionId);
            ht.Add("CreatedTime", DateTime.Now);

            return Convert.ToInt32(DataSources.Default.ExecuteScalar("AddRole", null,null,ht));
        }

        public static int EditRole(EditRoleRequest request)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Name", request.Name);
            ht.Add("Remark", request.Remark);
            ht.Add("PermissionIds", request.PermissionIds);
            ht.Add("RoleId", request.RoleId);

            return DataSources.Default.ExecuteNonQuery("EditRole", null,null,ht);
        }

        public static int DeleteRole(int roleId)
        {
            return DataSources.Default.ExecuteNonQuery("DeleteRole", roleId);
        }

        public static PagerModelCollection<RoleResponse> GetRoles(RoleQuery query)
        {
            Hashtable ht = new Hashtable();
            List<StatementCondition> conditions = new List<StatementCondition>();
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                ht.Add("Keywords", query.Keywords);
                conditions.Add(new StatementCondition("Keywords", true));
            }

            if (query.OwnerTypes!=null && query.OwnerTypes.Count>0)
            {
                ht.Add("OwnerTypes", string.Join(",", query.OwnerTypes));
                conditions.Add(new StatementCondition("OwnerTypes", true));
            }

            if (query.DataDimensionIds != null && query.DataDimensionIds.Count > 0)
            {
                ht.Add("DataDimensionIds", string.Join(",", query.DataDimensionIds));
                conditions.Add(new StatementCondition("DataDimensionIds", true));
            }

            return DataSources.Default.QueryCollection<RoleResponse>("GetRoles", null, conditions, query.StartIndex,query.EndIndex,ht);
        }

        public static RoleResponse GetRole(int roleId)
        {
            return DataSources.Default.Query<RoleResponse>("GetRole", roleId);
        }

        public static int IsExistsRoleByName(string name, int ownerType,int? dataDimensionId, int? roleId=null)
        {
            Hashtable ht = new Hashtable();
            ht.Add("Name", name);
            ht.Add("OwnerType", ownerType);
            ht.Add("DataDimensionId", dataDimensionId);
            ht.Add("RoleId", roleId);

            return Convert.ToInt32(DataSources.Default.ExecuteScalar("IsExistsRoleByName", null,null,ht));
        }

        public static List<RoleResponse> GetRolesByAccountId(int accountId)
        {
            return DataSources.Default.QueryCollection<RoleResponse>("GetRolesByAccountId", accountId).ToList();
        }
        #endregion

        #region Session
        public static int AddSession(AddSessionRequest request)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AccountId", request.AccountId);
            ht.Add("ClientIpAddress", request.ClientIpAddress);
            ht.Add("UserAgent", request.UserAgent);
            ht.Add("Token", request.Token);
            ht.Add("LoginTime", DateTime.Now);
            ht.Add("ExpireTime", request.ExpireTime);

            return Convert.ToInt32(DataSources.Default.ExecuteScalar("AddSession", null,null,ht));
        }

        public static PagerModelCollection<SessionResponse> GetSessions(SessionQuery query)
        {
            Hashtable ht = new Hashtable();
            List<StatementCondition> conditions = new List<StatementCondition>();
            if (query.AccountId!=null)
            {
                ht.Add("AccountId", query.AccountId);
                conditions.Add(new StatementCondition("AccountId", true));
            }

            if (query.OwnerType!=null)
            {
                ht.Add("OwnerType", query.OwnerType);
                conditions.Add(new StatementCondition("OwnerType", true));
            }

            if (!string.IsNullOrEmpty(query.Keywords))
            {
                ht.Add("Keywords", query.Keywords);
                conditions.Add(new StatementCondition("Keywords", true));
            }

            return DataSources.Default.QueryCollection<SessionResponse>("GetSessions", null,conditions,query.StartIndex,query.EndIndex,ht);
        }
        #endregion
    }
}
