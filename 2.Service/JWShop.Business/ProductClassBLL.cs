using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Linq;
using System.Text;

namespace JWShop.Business
{
    public sealed class ProductClassBLL : BaseBLL
    {
        private static readonly IProductClass dal = FactoryHelper.Instance<IProductClass>(Global.DataProvider, "ProductClassDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ProductClass");

        public static int Add(ProductClassInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ProductClassInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }

        public static ProductClassInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<ProductClassInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new ProductClassInfo();
        }

        public static List<ProductClassInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ProductClassInfo>)CacheHelper.Read(cacheKey);
        }
        /// <summary>
        /// 修改分类排序
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void ChangeProductClassOrder(int pid, int oid)
        {
            dal.ChangeProductClassOrder(pid, oid);
            CacheHelper.Remove(cacheKey);
        }

        public static void ChangeProductCount(int[] classIds, ChangeAction action)
        {
            dal.ChangeProductCount(classIds, action);
        }

        public static List<ProductClassInfo> ReadRootList()
        {
            var classes = ReadList();
            return classes.Where(k => k.ParentId == 0).ToList();
        }

        /// <summary>
        /// 读取名字已经缩进好的分类列表
        /// </summary>
        /// <returns>商品分类数据列表</returns>
        public static List<ProductClassInfo> ReadNamedList()
        {
            List<ProductClassInfo> result = new List<ProductClassInfo>();
            var classes = ReadList();
            foreach (ProductClassInfo productClass in classes.Where(k => k.ParentId == 0))
            {
                result.Add(productClass);
                result.AddRange(ReadChilds(productClass.Id, 2));
            }
            return result;
        }

        /// <summary>
        /// 产品详情页，读取产品导航路径
        /// </summary>
        public static List<string[]> ReadNavigationPath(string classId)
        {
            List<string[]> paths = new List<string[]>();
            var classIds = Array.ConvertAll<string, int>(classId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries), k => Convert.ToInt32(k)).ToList();
            foreach (var _classId in classIds)
            {
                var _class = Read(_classId);

                string[] ss = new string[3];
                ss[0] = "<span class=\"path_item\"><a href=\"/list.html?cat={0}\">{1}</a></span> > ";
                ss[1] = string.Join(",", classIds.Where(k => k <= _classId).ToArray());
                ss[2] = _class.Name;

                paths.Add(ss);
            }

            return paths;
        }

        public static List<ProductClassInfo> ReadChilds(int parentId)
        {
            List<ProductClassInfo> result = new List<ProductClassInfo>();
            var classes = ReadList();
            return classes.Where(k => k.ParentId == parentId).ToList();
        }
        /// <summary>
        /// 搜索产品分类列表数据
        /// </summary>
        /// <param name="id">产品分类列表的主键值</param>
        /// <returns>产品分类列表数据模型</returns>
        public static string SearchProductClassList(int fatherID)
        {
            string result = string.Empty;
            List<ProductClassInfo> productClassList = ReadList();
            foreach (ProductClassInfo productClass in productClassList)
            {
                if (productClass.ParentId == fatherID)
                {
                    if (result == string.Empty)
                    {
                        result = productClass.Id.ToString() + "," + productClass.Name;
                    }
                    else
                    {
                        result += "|" + productClass.Id.ToString() + "," + productClass.Name;
                    }
                }
            }
            return result;
        }
        public static List<ProductClassInfo> ReadChilds(int parentId, int depth)
        {
            List<ProductClassInfo> result = new List<ProductClassInfo>();
            var classes = ReadList().Where(k => k.ParentId == parentId);

            foreach (var entity in classes)
            {
                var temp = (ProductClassInfo)ServerHelper.CopyClass(entity);
                string tempString = string.Empty;
                for (int i = 1; i < depth; i++)
                {
                    tempString += HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                }
                temp.Name = tempString + temp.Name;
                result.Add(temp);
                result.AddRange(ReadChilds(temp.Id, depth + 1));
            }

            return result;
        }

        /// <summary>
        /// 读取产品的无限级分类
        /// </summary>
        /// <returns></returns>
        public static List<UnlimitClassInfo> ReadUnlimitClassList()
        {
            List<ProductClassInfo> productClassList = ReadList();
            List<UnlimitClassInfo> unlimitClassList = new List<UnlimitClassInfo>();
            foreach (ProductClassInfo productClass in productClassList)
            {
                UnlimitClassInfo unlimitClass = new UnlimitClassInfo();
                unlimitClass.ClassID = productClass.Id;
                unlimitClass.ClassName = productClass.Name;
                unlimitClass.FatherID = productClass.ParentId;
                unlimitClassList.Add(unlimitClass);
            }
            return unlimitClassList;
        }

        public static void Move(int id, ChangeAction action)
        {
            dal.Move(id, action);
            CacheHelper.Remove(cacheKey);
        }

        public static int GetLastClassID(string classID)
        {
            int backID = 0;
            if (classID.IndexOf("|") >= 0)
            {
                classID = classID.Substring(0, classID.Length - 1);

                backID = Convert.ToInt32(classID.Substring(classID.LastIndexOf("|") + 1, classID.Length - classID.LastIndexOf("|") - 1));
                return backID;
            }
            else
            {
                return backID;
            }
        }

        public static int GetProductClassType(int productClassID)
        {
            return dal.GetProductClassType(productClassID);
        }
        /// <summary>
        /// 读取编排好的产品分类---前端展示
        /// </summary>
        /// <param name="idList"></param>
        public static string ProductClassNameList(string idList)
        {
            string content = string.Empty;
            if (!string.IsNullOrEmpty(idList))
            {
                idList = idList.Substring(1, idList.Length - 2);
            }
            idList = idList.Replace("||", "#");
            if (idList.Length > 0)
            {
                foreach (string temp in idList.Split('#'))
                {
                    string tempProductClassName = string.Empty;
                    foreach (string id in temp.Split('|'))
                    {
                        ProductClassInfo proClass = ProductClassBLL.Read(Convert.ToInt32(id));
                        if (string.IsNullOrEmpty(tempProductClassName))
                        {
                            tempProductClassName = "<a href=\"/List.html?cat=" + ReadFullParentId(proClass.Id) + "\" >" + proClass.Name + "</a>";
                        }
                        else
                        {
                            tempProductClassName += " > <a href=\"/List.html?cat=" + ReadFullParentId(proClass.Id) + "\">" + proClass.Name + "</a>";
                        }
                    }
                    if (!string.IsNullOrEmpty(tempProductClassName))
                    {
                        if (string.IsNullOrEmpty(content))
                        {
                            content = tempProductClassName;
                        }
                        else
                        {
                            content += "，" + tempProductClassName;
                        }
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 读取编排好的产品分类---后台展示
        /// </summary>
        /// <param name="idList"></param>
        public static string ProductClassNameListAdmin(string idList)
        {
            string content = string.Empty;
            if (!string.IsNullOrEmpty(idList))
            {
                idList = idList.Substring(1, idList.Length - 2);
            }
            idList = idList.Replace("||", "#");
            if (idList.Length > 0)
            {
                foreach (string temp in idList.Split('#'))
                {
                    string tempProductClassName = string.Empty;
                    foreach (string id in temp.Split('|'))
                    {
                        ProductClassInfo proClass = ProductClassBLL.Read(Convert.ToInt32(id));
                        if (string.IsNullOrEmpty(tempProductClassName))
                        {
                            //tempProductClassName = "<a href=\"/List.html?cat=" + ReadFullParentId(proClass.Id) + "\" target=\"_blank\" title=\"" + proClass.Name + "\">" + StringHelper.Substring(proClass.Name, 10) + "</a>";
                            tempProductClassName = StringHelper.Substring(proClass.Name, 10) ;
                        }
                        else
                        {
                            //tempProductClassName += " > <a href=\"/List.html?cat=" + ReadFullParentId(proClass.Id) + "\"  target=\"_blank\" title=\"" + proClass.Name + "\">" + StringHelper.Substring(proClass.Name, 10) + "</a>";
                            tempProductClassName +=" > "+StringHelper.Substring(proClass.Name, 10) ;
                        }
                    }
                    if (!string.IsNullOrEmpty(tempProductClassName))
                    {
                        if (string.IsNullOrEmpty(content))
                        {
                            content = tempProductClassName;
                        }
                        else
                        {
                            content += "，" + tempProductClassName;
                        }
                    }
                }
            }
            return content;
        }
        
        /// <summary>
        /// 读取完整的上级分类Id(|1|2|3|)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>完整的分类ID</returns>
        public static string ReadFullParentId(int id)
        {
            string result = id.ToString();
            var pclass=Read(id);
            while(pclass.ParentId>0){
                result = pclass.ParentId + "," + result;
                pclass = Read(pclass.ParentId);
            }
            return result; 
        }

    }
}