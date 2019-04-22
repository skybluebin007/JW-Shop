using System;
using System.Xml;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;

namespace JWShop.Common
{
    /// <summary>
    /// 文章类别与Id的对应关系
    /// </summary>
    public sealed class ClassRelation
    {
        private static string fileName = ServerHelper.MapPath("~/Config/ClassRelation.config");
        private static string classRelationCacheKey = CacheKey.ReadCacheKey("ClassRelation");
        
        /// <summary>
        /// 获取文章分类的Id值
        /// </summary>
        /// <param name="virtualId">虚拟Id</param>
        /// <returns>分类Id值</returns>
        public string this[int virtualId]
        {
            get
            {
                if (CacheHelper.Read(classRelationCacheKey) == null)
                {
                    RefreshClassRelactionCache();
                }
                var rels = (List<ClassRelationInfo>)CacheHelper.Read(classRelationCacheKey);

                var rel = rels.OrderByDescending(k => k.RealId).FirstOrDefault(k => k.VirtualId == virtualId) ?? new ClassRelationInfo();
                return rel.RealId.ToString();
            }
        }

        public static List<ClassRelationInfo> Read()
        {
            if (CacheHelper.Read(classRelationCacheKey) == null)
            {
                RefreshClassRelactionCache();
            }
            return (List<ClassRelationInfo>)CacheHelper.Read(classRelationCacheKey);
        }

        public static void RefreshClassRelactionCache()
        {
            List<ClassRelationInfo> rels = new List<ClassRelationInfo>();
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                foreach (XmlNode xn in xh.ReadNode("list").ChildNodes)
                {
                    rels.Add(new ClassRelationInfo
                    {
                        VirtualId = int.Parse(xn.Attributes["virtualId"].Value),
                        RealId = int.Parse(xn.Attributes["realId"].Value),
                        Name = xn.Attributes["name"].Value
                    });
                }
            }
            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(classRelationCacheKey, rels, cd);
        }

        public static void Add(ClassRelationInfo entity)
        {
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                xh.InsertElement("list", "item", new string[] { "virtualId", "realId", "name" }, new string[] { entity.VirtualId.ToString(), entity.RealId.ToString(), entity.Name }, string.Empty);
                xh.Save();
                RefreshClassRelactionCache();
            }
        }

    }
}