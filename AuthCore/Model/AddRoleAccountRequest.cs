using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthCore.Model
{
    public class AddRoleAccountRequest
    {
        public int AccountId { get; set; }

        public List<int> RoleIds { get; set; }
    }
}
