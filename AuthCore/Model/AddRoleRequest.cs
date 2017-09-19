using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthCore.Model
{
    public class AddRoleRequest
    {
        /// <summary>
        /// 所属系统类型
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 权限编号（以逗号隔开1,2,3,4）
        /// </summary>
        public string PermissionIds { get; set; }

        /// <summary>
        /// 角色所属于某位商家的，或者某个仓库的等等
        /// </summary>
        public int? DataDimensionId { get; set; }
    }

    public class EditRoleRequest
    {
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 权限编号（以逗号隔开1,2,3,4）
        /// </summary>
        public string PermissionIds { get; set; }
    }

    public class RoleResponse: AddRoleRequest
    {
        public int RoleId { get; set; }

        public DateTime CreatedTime { get; set; }
    }

    public class RoleQuery : PagerQuery
    {
        /// <summary>
        /// 角色名称模糊查询
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 角色所属系统类型
        /// </summary>
        public List<int> OwnerTypes { get; set; }

        /// <summary>
        /// 角色所属对象的Id
        /// </summary>
        public List<int> DataDimensionIds { get; set; }
    }
}
