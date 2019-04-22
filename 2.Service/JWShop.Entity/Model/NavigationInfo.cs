using System;

namespace JWShop.Entity {
    /// <summary>
    /// 网站导航实体模型
    /// </summary>
    [Serializable]
    public sealed class NavigationInfo {
        private int id;
        private int parentId;
        private int navigationType;
        private string name;
        private string remark;
        private int orderId;
        private string url;
        private int classType;
        private int classId;
        private bool isSingle;
        private int showType;
        private bool isVisible;

        public int Id {
            get { return id; }
            set { id = value; }
        }
        public int ParentId {
            get { return parentId; }
            set { parentId = value; }
        }
        public int NavigationType {
            get { return navigationType; }
            set { navigationType = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        public int OrderId {
            get { return orderId; }
            set { orderId = value; }
        }
        public string Url {
            get { return url; }
            set { url = value; }
        }
        public int ClassType {
            get { return classType; }
            set { classType = value; }
        }
        public int ClassId {
            get { return classId; }
            set { classId = value; }
        }
        public bool IsSingle {
            get { return isSingle; }
            set { isSingle = value; }
        }
        public int ShowType {
            get { return showType; }
            set { showType = value; }
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
    }
}