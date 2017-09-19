using Microsoft.VisualStudio.TestTools.UnitTesting;
using AuthCore.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthCore.Model;

namespace AuthCore.Logic.Tests
{
    [TestClass()]
    public class AuthLogicTests
    {
        #region Account
        [TestMethod()]
        public void AddAccountTest()
        {
            AddAccountRequest request = new AddAccountRequest()
            {
                Name="ceshi",
                OwnerType=0,
                OwnerId=0,
                LoginName= "ceshi",
                LoginPassword="123123",
                AppKey="bagkangalg512121",
                AppSecret="123123",
                SignKey= "8621d02c384fe7f0373c6104fcd27ed1",
                IpWhiteList= "192.168.0.152,192.168.0.153,192.168.0.154",
                RoleIds=new List<int>() { 1,2,3}
            };

            var result = AuthLogic.AddAccount(request);
        }

        [TestMethod()]
        public void GetAccountsByIdsTest()
        {
            var result = AuthLogic.GetAccountsByIds(new List<int>() {1,2});
        }

        [TestMethod()]
        public void GetAccountTest()
        {
            var obj = AuthLogic.GetAccount(1);
        }

        [TestMethod()]
        public void IsExistsAccountTest()
        {
            var result = AuthLogic.IsExistsAccount(3);
        }

        [TestMethod()]
        public void GetAccountByLoginNameTest()
        {
            var result = AuthLogic.GetAccountByLoginName("test",0,1);
        }

        [TestMethod()]
        public void DeleteAccountTest()
        {
            AuthLogic.DeleteAccount(3);
        }

        [TestMethod()]
        public void EnableAccountTest()
        {
            AuthLogic.EnableAccount(1);
        }

        [TestMethod()]
        public void DisableAccountTest()
        {
            AuthLogic.DisableAccount(1);
        }

        [TestMethod()]
        public void EditLoginPasswordTest()
        {
            AuthLogic.EditLoginPassword(1, "12341234");
        }

        [TestMethod()]
        public void EditAppKeyandAppSecretTest()
        {
            AuthLogic.EditAppKeyandAppSecret(1,"gagagg899555","12341234");
        }

        [TestMethod()]
        public void EditSignKeyTest()
        {
            AuthLogic.EditSignKey(1, "84511gag48ega13ag8a1ge");
        }

        [TestMethod()]
        public void EditIpWhiteListTest()
        {
            AuthLogic.EditIpWhiteList(1,"192.168.0.163");
        }
        #endregion

        #region AccountRole
        [TestMethod()]
        public void AddRoleAccountTest()
        {
            AddRoleAccountRequest request = new AddRoleAccountRequest()
            {
                AccountId=1,
                RoleIds=new List<int>() { 4,5,6}
            };

            var result = AuthLogic.AddRoleAccount(request);
        }
        #endregion

        #region Role
        [TestMethod()]
        public void AddRoleTest()
        {
            AddRoleRequest request = new AddRoleRequest()
            {
                OwnerType=1,
                Name="操作员",
                Remark="测试",
                PermissionIds="9852,9802",
                DataDimensionId=2
            };

            var result = AuthLogic.AddRole(request);
        }

        [TestMethod()]
        public void EditRoleTest()
        {
            EditRoleRequest request = new EditRoleRequest()
            {
                RoleId=1,
                Name="管理员01",
                Remark="测试0011",
                PermissionIds= "9852,9853,9854,9855,9856,9857,9858,9859"
            };

            AuthLogic.EditRole(request);
        }

        [TestMethod()]
        public void GetRolesTest()
        {
            RoleQuery query = new RoleQuery()
            {
                //Keywords="操作",
                //OwnerTypes=new List<int>() {1},
                DataDimensionIds=new List<int>() {2}

            };
            var result = AuthLogic.GetRoles(query);
        }

        [TestMethod()]
        public void GetRoleTest()
        {
            var result = AuthLogic.GetRole(1);
        }

        [TestMethod()]
        public void GetRolesByAccountIdTest()
        {
            var result = AuthLogic.GetRolesByAccountId(2);
        }
        #endregion

        #region Session
        [TestMethod()]
        public void AddSessionTest()
        {
            AddSessionRequest request = new AddSessionRequest()
            {
                AccountId=2,
                ClientIpAddress="192.168.0.152",
                UserAgent= "Mozilla/5.0 (Windows; U; Windows NT 5.1) Gecko/20070309 Firefox/2.0.0.3",
                Token= "8d1e8c02-20d6-42ec-82ca-8238c8b66e3b",
                ExpireTime=DateTime.Now.AddDays(1)
            };

            var result = AuthLogic.AddSession(request);
        }

        [TestMethod()]
        public void GetSessionsTest()
        {
            SessionQuery query = new SessionQuery()
            {
                //AccountId=1,
                //OwnerType=1,
                Keywords="192"
            };

            var result = AuthLogic.GetSessions(query);
        }
        #endregion
    }
}