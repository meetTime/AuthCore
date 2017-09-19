using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthCore.Model
{
    public class PagerQuery
    {
        public PagerQuery()
        {
            this.StartIndex = 1;
            this.EndIndex = 10;
        }

        public int StartIndex
        {
            get;
            set;
        }

        public int EndIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 设置页数，页数从1开始计数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public void SetPage(int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            StartIndex = (pageIndex - 1) * pageSize + 1;
            EndIndex = pageIndex * pageSize;
        }


        /// <summary>
        /// 由每个调用的接口自己实现规则
        /// </summary>
        public string SortField { get; set; }

        //True=升序，False=降序
        public bool? SortAscending { get; set; }
    }
}
