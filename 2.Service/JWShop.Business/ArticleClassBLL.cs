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

namespace JWShop.Business
{
    /// <summary>
    /// 文章分类业务逻辑。
    /// </summary>
    public sealed class ArticleClassBLL : BaseBLL
    {
        private static readonly IArticleClass dal = FactoryHelper.Instance<IArticleClass>(Global.DataProvider, "ArticleClassDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("ArticleClass");

        public static int Add(ArticleClassInfo entity)
        {
            entity.Id = dal.Add(entity);
            CacheHelper.Remove(cacheKey);
            return entity.Id;
        }

        public static void Update(ArticleClassInfo entity)
        {
            dal.Update(entity);
            CacheHelper.Remove(cacheKey);
        }

        public static void Delete(int id)
        {
            dal.Delete(id);
            CacheHelper.Remove(cacheKey);
        }

        public static List<ArticleClassInfo> ReadList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadList());
            }
            return (List<ArticleClassInfo>)CacheHelper.Read(cacheKey);
        }

        public static ArticleClassInfo Read(int id)
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                return dal.Read(id);
            }
            return ((List<ArticleClassInfo>)CacheHelper.Read(cacheKey)).FirstOrDefault(k => k.Id == id) ?? new ArticleClassInfo();
        }

        public static List<ArticleClassInfo> ReadRootList()
        {
            return ReadList().Where(k => k.ParentId == 0).ToList();
        }

        public static List<ArticleClassInfo> ReadChilds(int parentId)
        {
            return ReadList().Where(k => k.ParentId == parentId).ToList();
        }

        private static List<ArticleClassInfo> ReadChilds(int parentId, int depth)
        {
            List<ArticleClassInfo> result = new List<ArticleClassInfo>();
            var classes = ReadList().Where(k => k.ParentId == parentId);

            foreach (var entity in classes)
            {
                var temp = (ArticleClassInfo)ServerHelper.CopyClass(entity);
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

        public static List<ArticleClassInfo> ReadNamedList()
        {
            List<ArticleClassInfo> result = new List<ArticleClassInfo>();
            foreach (var articleClass in ReadRootList())
            {
                result.Add(articleClass);
                result.AddRange(ReadChilds(articleClass.Id, 2));
            }
            return result;
        }
        public static void ChangeArticleClassOrder(int id, int orderId) {
            dal.ChangeArticleClassOrder(id, orderId);
            CacheHelper.Remove(cacheKey);
        }
        public static void Move(int id, ChangeAction action)
        {
            dal.Move(id, action);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 文章分类列表数据
        /// </summary>
        /// <param name="id">文章分类列表的主键值</param>
        /// <returns>文章分类列表数据模型</returns>
        public static string NameList(string idList)
        {
            List<string> contentList = new List<string>();
            if (!string.IsNullOrEmpty(idList))
            {
                idList = idList.Substring(1, idList.Length - 2);
            }
            idList = idList.Replace("||", "#");

            if (idList.Length > 0)
            {
                foreach (string temp in idList.Split('#'))
                {
                    List<string> nameList = new List<string>();
                    foreach (string id in temp.Split('|'))
                    {
                        nameList.Add(ArticleClassBLL.Read(Convert.ToInt32(id)).Name);
                    }
                    if (nameList.Count > 0)
                    {
                        contentList.Add(string.Join(" > ", nameList));
                    }
                }
            }
            return string.Join("，", contentList);
        }

        /// <summary>
        /// 读取文章完整的上级分类Id(|1|2|3|)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>完整的分类ID</returns>
        public static string ReadFullParentId(int id)
        {
            return ReadParentId(id) + "|" + id.ToString() + "|"; ;
        }

        /// <summary>
        /// 读取文章上级分类Id(|1)
        /// </summary>
        private static string ReadParentId(int id)
        {
            string result = string.Empty;
            int parentId = Read(id).ParentId;
            if (parentId > 0)
            {
                result = ReadParentId(parentId) + "|" + parentId;
            }
            return result;
        }

        /// <summary>
        /// 根据分类类型获取URL
        /// </summary>
        /// <param name="articleClass"></param>
        /// <returns></returns>
        public static string GetClassUrl(ArticleClassInfo articleClass)
        {
            string url = "";
            ArticleClassInfo tmpCLass = new ArticleClassInfo();
            if (ArticleClassBLL.ReadChilds(articleClass.Id).Count > 0)
            {
                tmpCLass = ArticleClassBLL.ReadChilds(articleClass.Id)[0]; //如果是父级分类则取第一个子分类的URL
            }
            else
            {
                tmpCLass = articleClass;//如果不是父级分类则取当前分类的URL 
            }
            switch (tmpCLass.ShowType)
            {
                case 5://链接URL
                    url = string.IsNullOrEmpty(tmpCLass.Description) ? "/" : tmpCLass.Description;
                    break;
                case 4://父级分类
                    {
                        List<ArticleClassInfo> childlist = ReadChilds(articleClass.Id);
                        if (childlist.Count > 0)
                        {
                            url = GetClassUrl(childlist[0]);
                        }
                        else
                        {
                            url = "/";
                        }
                    }
                    break;

                case 2://文章列表
                    url = "/Article-C" + tmpCLass.Id + ".html";
                    break;
                case 3:
                    url = "/Picture-C" + tmpCLass.Id + ".html";
                    break;
                case 1://单文章
                    {
                        ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                        articleSearch.ClassId = "|" + tmpCLass.Id + "|";
                        List<ArticleInfo> articleList = ArticleBLL.SearchList(articleSearch);
                        if (articleList.Count > 0)
                        {
                            url = "/ArticleDetail-I" + articleList[0].Id + ".html";
                        }
                        else
                        {
                            url = "/Article-C" + tmpCLass.Id + ".html";
                        }
                    }
                    break;

                default:
                    url = "/";
                    break;
            }
            return url;
        }
        /// <summary>
        /// 根据帮助分类类型获取URL
        /// </summary>
        /// <param name="articleClass"></param>
        /// <returns></returns>
        public static string GetHelpClassUrl(ArticleClassInfo articleClass)
        {
            string url = "";
            ArticleClassInfo tmpCLass = new ArticleClassInfo();
            if (ArticleClassBLL.ReadChilds(articleClass.Id).Count > 0 && Read(articleClass.Id).ShowType!=5)
            {
                tmpCLass = ArticleClassBLL.ReadChilds(articleClass.Id)[0]; //如果是父级分类则取第一个子分类的URL
            }
            else
            {
                tmpCLass = articleClass;//如果不是父级分类则取当前分类的URL 
            }
            switch (tmpCLass.ShowType)
            {
                case 5://链接URL
                    url = string.IsNullOrEmpty(tmpCLass.Description) ? "/" : tmpCLass.Description;
                    break;
                case 4://父级分类
                    {
                        List<ArticleClassInfo> childlist = ReadChilds(articleClass.Id);
                        if (childlist.Count > 0)
                        {
                            url = GetHelpClassUrl(childlist[0]);
                        }
                        else
                        {
                            url = "/";
                        }
                    }
                    break;

                case 2://文章列表
                    url = "/Help-C" + tmpCLass.Id + ".html";
                    break;           
                case 1://单文章
                default:
                    {
                        ArticleSearchInfo articleSearch = new ArticleSearchInfo();
                        articleSearch.ClassId = "|" + tmpCLass.Id + "|";
                        List<ArticleInfo> articleList = ArticleBLL.SearchList(articleSearch);
                        if (articleList.Count > 0)
                        {
                            url = "/HelpDetail-I" + articleList[0].Id + ".html";
                        }
                        else
                        {
                            url = "/Help-C" + tmpCLass.Id + ".html";
                        }
                    }
                    break;           
            }
            return url;
        }
        public static void GetTopClassID(int fatherID, ref int resultStr)
        {
            if (fatherID != 0)
            {
                int currentFatherID = 0;
                List<ArticleClassInfo> articleClassList = ReadList();
                foreach (ArticleClassInfo articleClass in articleClassList)
                {
                    if (articleClass.Id == fatherID)
                    {
                        resultStr = articleClass.Id;
                        currentFatherID = articleClass.ParentId;
                        break;
                    }
                }
                GetTopClassID(currentFatherID, ref resultStr);
            }
        }
        public static int GetTopClassID(string classID)
        {
            if (!string.IsNullOrEmpty(classID))
            {
                char[] splitChar = { '|' };
                string[] classArr = classID.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                if (classArr.Length > 0)
                {
                    return Convert.ToInt32(classArr[0]);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 读取文章完整的上级分类ID(|1|2|3|)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>完整的分类ID</returns>
        public static string ReadArticleClassFullFatherID(int id)
        {
            return ReadArticleClassFatherID(id) + "|" + id.ToString() + "|"; ;
        }
        /// <summary>
        /// 读取文章上级分类ID(|1)
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>上级的分类ID</returns>
        private static string ReadArticleClassFatherID(int id)
        {
            string result = string.Empty;
            int fatherID = Read(id).ParentId;
            if (fatherID > 0)
            {
                result = ReadArticleClassFatherID(fatherID) + "|" + fatherID;
            }
            return result;
        }
        /// <summary>
        /// 文章详情面包屑导航
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="targetUrl"></param>
        /// <returns></returns>
        public static string ArticleClassNavigationList(string idList, string targetUrl)
        {
            string content = string.Empty;
            if (idList != string.Empty)
            {
                idList = idList.Substring(1, idList.Length - 2);
            }
            idList = idList.Replace("||", "#");
            if (idList.Length > 0)
            {
                foreach (string temp in idList.Split('#'))
                {
                    string tempArticleClassName = string.Empty;
                    int linkCount = 1;
                    foreach (string id in temp.Split('|'))
                    {
                        if (tempArticleClassName == string.Empty)
                        {
                            if (linkCount < temp.Split('|').Length)
                            {
                                tempArticleClassName = "<a href=\"/" + targetUrl + id + ".html\" >" + ArticleClassBLL.Read(Convert.ToInt32(id)).Name + "</a>";
                            }
                            else
                            {
                                tempArticleClassName = ArticleClassBLL.Read(Convert.ToInt32(id)).Name;
                            }
                        }
                        else
                        {
                            if (linkCount < temp.Split('|').Length)
                            {
                                tempArticleClassName += " > <a href=\"/" + targetUrl + id + ".html\" >" + ArticleClassBLL.Read(Convert.ToInt32(id)).Name + "</a>";
                            }
                            else
                            {
                                tempArticleClassName += " > " + ArticleClassBLL.Read(Convert.ToInt32(id)).Name + "";
                            }
                        }
                        linkCount++;
                    }
                    if (tempArticleClassName != string.Empty)
                    {
                        if (content == string.Empty)
                        {
                            content = tempArticleClassName;
                        }
                        else
                        {
                            content += "，" + tempArticleClassName;
                        }
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 面包屑导航改进版，根据分类自动显示对应链接
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static string ArticleClassNavigationList(string idList)
        {
            string content = string.Empty;
            if (idList != string.Empty)
            {
                idList = idList.Substring(1, idList.Length - 2);
            }
            idList = idList.Replace("||", "#");
            if (idList.Length > 0)
            {
                foreach (string temp in idList.Split('#'))
                {
                    string tempArticleClassName = string.Empty;
                    int linkCount = 1;
                    foreach (string id in temp.Split('|'))
                    {
                        if (tempArticleClassName == string.Empty)
                        {
                            if (linkCount < temp.Split('|').Length)
                            {
                                tempArticleClassName = "<a href=\"" + GetClassUrl(Read(Convert.ToInt32(id))) + "\" >" + Read(Convert.ToInt32(id)).Name + "</a>";
                            }
                            else
                            {
                                tempArticleClassName = ArticleClassBLL.Read(Convert.ToInt32(id)).Name;

                            }
                        }
                        else
                        {
                            if (linkCount < temp.Split('|').Length)
                            {
                                tempArticleClassName += " > <a href=\"" + GetClassUrl(Read(Convert.ToInt32(id))) + "\" >" + ArticleClassBLL.Read(Convert.ToInt32(id)).Name + "</a>";
                            }
                            else
                            {
                                tempArticleClassName += " > " + ArticleClassBLL.Read(Convert.ToInt32(id)).Name + "";
                            }
                        }
                        linkCount++;
                    }
                    if (tempArticleClassName != string.Empty)
                    {
                        if (content == string.Empty)
                        {
                            content = tempArticleClassName;
                        }
                        else
                        {
                            content += "，" + tempArticleClassName;
                        }
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 帮助中心面包屑导航
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="targetUrl"></param>
        /// <returns></returns>
        public static string HelpArticleClassNavigationList(string idList)
        {
            string content = string.Empty;
            if (idList != string.Empty)
            {
                idList = idList.Substring(1, idList.Length - 2);
            }
            idList = idList.Replace("||", "#");
            if (idList.Length > 0)
            {
                foreach (string temp in idList.Split(new char[]{'#'},StringSplitOptions.RemoveEmptyEntries))
                {
                 
                    string tempArticleClassName = string.Empty;
                    int linkCount = 1;
                    foreach (string id in temp.Split('|'))
                    {
                        ArticleClassInfo tmpClass = Read(int.Parse(id));
                        if (tempArticleClassName == string.Empty)
                        {
                            if (linkCount < temp.Split('|').Length)
                            {
                                tempArticleClassName = "<a href="+GetHelpClassUrl(tmpClass)+" >" + Read(Convert.ToInt32(id)).Name + "</a>";
                            }
                            else
                            {
                                tempArticleClassName = Read(Convert.ToInt32(id)).Name;
                            }
                        }
                        else
                        {
                            if (linkCount < temp.Split('|').Length)
                            {
                                tempArticleClassName += " > <a href=" + GetHelpClassUrl(tmpClass) + " >" + Read(Convert.ToInt32(id)).Name + "</a>";
                            }
                            else
                            {
                                tempArticleClassName += " > " + Read(Convert.ToInt32(id)).Name + "";
                            }
                        }
                        linkCount++;
                    }
                    if (tempArticleClassName != string.Empty)
                    {
                        if (content == string.Empty)
                        {
                            content = tempArticleClassName;
                        }
                        else
                        {
                            content += "，" + tempArticleClassName;
                        }
                    }
                }
            }
            return content;
        }
        /// <summary>
        /// 获取最近一级分类
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static int GetLastClassID(string classID)
        {
            if (!string.IsNullOrEmpty(classID))
            {
                char[] splitChar = { '|' };
                string[] classArr = classID.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                if (classArr.Length > 0)
                {
                    return Convert.ToInt32(classArr[classArr.Length - 1]);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}