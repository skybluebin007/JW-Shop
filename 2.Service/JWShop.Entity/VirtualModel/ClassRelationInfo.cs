using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    /// <summary>
    /// 文章类别与Id的对应关系对象
    /// </summary>
    public sealed class ClassRelationInfo
    {
        private int virtualId;
        private int realId;
        private string name;

        public int VirtualId
        {
            get { return this.virtualId; }
            set { this.virtualId = value; }
        }
        public int RealId
        {
            get { return this.realId; }
            set { this.realId = value; }
        }
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}
