using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using JWShop.Common;
using JWShop.Entity;
using SkyCES.EntLib;
using System.Linq;
using System.Web.Caching;

namespace JWShop.Common
{
    public static class BadwordFilterHelper
    {
        private static string configCacheKey = CacheKey.ReadCacheKey("Badwords");

        /// <summary>
        /// 从缓存读取脏字字典
        /// </summary>
        /// <returns></returns>
        public static string ReadBadwords()
        {
            if (CacheHelper.Read(configCacheKey) == null)
            {
                RefreshBadwordsCache();
            }
            return (string)CacheHelper.Read(configCacheKey);
        }
        /// <summary>
        /// 刷新脏字字典
        /// </summary>
        private static void RefreshBadwordsCache()
        {
            string fileName = ServerHelper.MapPath("~/Config/Badwords.config");

            string badwords = string.Empty;
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                badwords = xh.ReadNode("Badwords").InnerText;
            }

            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(configCacheKey, badwords, cd);
        }
        /// <summary>
        /// 更新脏字字典
        /// </summary>
        /// <param name="config"></param>
        public static void UpdateBadwords(string badwords)
        {
            string fileName = ServerHelper.MapPath("~/Config/Badwords.config");
            using (XmlHelper xh = new XmlHelper(fileName))
            {
                xh.ReadNode("Badwords").InnerText = badwords;
                xh.Save();
            }

            CacheDependency cd = new CacheDependency(fileName);
            CacheHelper.Write(configCacheKey, badwords, cd);
        }

        /// <summary>
        /// 过滤字符串中的脏字（敏感词）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string FilterBadwords(this string content)
        {
            var badwords = ReadBadwords().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            BadwordFilter badwordFilter = new BadwordFilter(badwords);

            return badwordFilter.FilterBadwords(content);
        }
    }


    /// <summary>
    /// 脏字过滤
    /// Aho-Corasick算法实现
    /// 参考资料：http://www.cnblogs.com/kudy/archive/2011/12/20/2294762.html
    /// </summary>
    internal class BadwordFilter
    {
        /// <summary>
        /// 构造节点
        /// </summary>
        private class Node
        {
            private Dictionary<char, Node> transDict;

            public Node(char c, Node parent)
            {
                this.Char = c;
                this.Parent = parent;
                this.Transitions = new List<Node>();
                this.Results = new List<string>();

                this.transDict = new Dictionary<char, Node>();
            }

            public char Char
            {
                get;
                private set;
            }

            public Node Parent
            {
                get;
                private set;
            }

            public Node Failure
            {
                get;
                set;
            }

            public List<Node> Transitions
            {
                get;
                private set;
            }

            public List<string> Results
            {
                get;
                private set;
            }

            public void AddResult(string result)
            {
                if (!Results.Contains(result))
                {
                    Results.Add(result);
                }
            }

            public void AddTransition(Node node)
            {
                this.transDict.Add(node.Char, node);
                this.Transitions = this.transDict.Values.ToList();
            }

            public Node GetTransition(char c)
            {
                Node node;
                if (this.transDict.TryGetValue(c, out node))
                {
                    return node;
                }

                return null;
            }

            public bool ContainsTransition(char c)
            {
                return GetTransition(c) != null;
            }
        }

        private Node root; // 根节点
        private string[] keywords; // 所有关键词

        public BadwordFilter(IEnumerable<string> keywords)
        {
            this.keywords = keywords.ToArray();
            this.Initialize();
        }

        /// <summary>
        /// 根据关键词来初始化所有节点
        /// </summary>
        private void Initialize()
        {
            this.root = new Node(' ', null);

            // 添加模式
            foreach (string k in this.keywords)
            {
                Node n = this.root;
                foreach (char c in k)
                {
                    Node temp = null;
                    foreach (Node tnode in n.Transitions)
                    {
                        if (tnode.Char == c)
                        {
                            temp = tnode; break;
                        }
                    }

                    if (temp == null)
                    {
                        temp = new Node(c, n);
                        n.AddTransition(temp);
                    }
                    n = temp;
                }
                n.AddResult(k);
            }

            // 第一层失败指向根节点
            List<Node> nodes = new List<Node>();
            foreach (Node node in this.root.Transitions)
            {
                // 失败指向root
                node.Failure = this.root;
                foreach (Node trans in node.Transitions)
                {
                    nodes.Add(trans);
                }
            }
            // 其它节点 BFS
            while (nodes.Count != 0)
            {
                List<Node> newNodes = new List<Node>();
                foreach (Node nd in nodes)
                {
                    Node r = nd.Parent.Failure;
                    char c = nd.Char;

                    while (r != null && !r.ContainsTransition(c))
                    {
                        r = r.Failure;
                    }

                    if (r == null)
                    {
                        // 失败指向root
                        nd.Failure = this.root;
                    }
                    else
                    {
                        nd.Failure = r.GetTransition(c);
                        foreach (string result in nd.Failure.Results)
                        {
                            nd.AddResult(result);
                        }
                    }

                    foreach (Node child in nd.Transitions)
                    {
                        newNodes.Add(child);
                    }
                }
                nodes = newNodes;
            }
            // 根节点的失败指向自己
            this.root.Failure = this.root;
        }

        /// <summary>
        /// 找出所有出现过的关键词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<BadwordFilterResult> FindAllBadwords(string text)
        {
            List<BadwordFilterResult> list = new List<BadwordFilterResult>();

            Node current = this.root;
            for (int index = 0; index < text.Length; ++index)
            {
                Node trans;
                do
                {
                    trans = current.GetTransition(text[index]);

                    if (current == this.root)
                        break;

                    if (trans == null)
                    {
                        current = current.Failure;
                    }
                } while (trans == null);

                if (trans != null)
                {
                    current = trans;
                }

                foreach (string s in current.Results)
                {
                    list.Add(new BadwordFilterResult(index - s.Length + 1, s));
                }
            }

            return list;
        }

        /// <summary>
        /// 简单地过滤关键词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string FilterBadwords(string text)
        {
            StringBuilder sb = new StringBuilder();

            Node current = this.root;
            for (int index = 0; index < text.Length; index++)
            {
                Node trans;
                do
                {
                    trans = current.GetTransition(text[index]);

                    if (current == this.root)
                        break;

                    if (trans == null)
                    {
                        current = current.Failure;
                    }

                } while (trans == null);

                if (trans != null)
                {
                    current = trans;
                }

                // 处理字符
                if (current.Results.Count > 0)
                {
                    string first = current.Results[0];
                    sb.Remove(sb.Length - first.Length + 1, first.Length - 1);// 把匹配到的替换为**
                    sb.Append(new string('*', current.Results[0].Length));
                }
                else
                {
                    sb.Append(text[index]);
                }
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// 表示一个查找结果
    /// </summary>
    internal struct BadwordFilterResult
    {
        private int index;
        private string keyword;
        public static readonly BadwordFilterResult Empty = new BadwordFilterResult(-1, string.Empty);

        public BadwordFilterResult(int index, string keyword)
        {
            this.index = index;
            this.keyword = keyword;
        }

        /// <summary>
        /// 位置
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword
        {
            get { return keyword; }
        }
    }
}