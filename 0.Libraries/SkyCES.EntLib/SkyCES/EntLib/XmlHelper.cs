namespace SkyCES.EntLib
{
    using System;
    using System.Xml;

    public class XmlHelper : IDisposable
    {
        private XmlDocument xd;
        private string xmlfile;

        public XmlHelper(string xmlFile)
        {
            try
            {
                this.xd = new XmlDocument();
                this.xd.Load(xmlFile);
            }
            catch
            {
            }
            this.xmlfile = xmlFile;
        }

        public void DeleteNode(string pathNode)
        {
            string xpath = pathNode.Substring(0, pathNode.LastIndexOf("/"));
            this.xd.SelectSingleNode(xpath).RemoveChild(this.xd.SelectSingleNode(pathNode));
        }

        public void Dispose()
        {
            this.xd = null;
        }

        public void InsertElement(string mainNode, string childNode, string content, bool lastChild)
        {
            XmlNode node = null;
            if (lastChild)
            {
                node = this.xd.SelectSingleNode(mainNode).LastChild;
            }
            else
            {
                node = this.xd.SelectSingleNode(mainNode);
            }
            XmlElement newChild = this.xd.CreateElement(childNode);
            newChild.InnerText = content;
            node.AppendChild(newChild);
        }

        public void InsertElement(string mainNode, string childNode, string attrib, string attribContent, string content)
        {
            XmlNode node = this.xd.SelectSingleNode(mainNode);
            XmlElement newChild = this.xd.CreateElement(childNode);
            newChild.SetAttribute(attrib, attribContent);
            newChild.InnerText = content;
            node.AppendChild(newChild);
        }

        public void InsertElement(string mainNode, string childNode, string[] attrib, string[] attribContent, string content)
        {
            XmlNode node = this.xd.SelectSingleNode(mainNode);
            XmlElement newChild = this.xd.CreateElement(childNode);
            for (int i = 0; i < attrib.Length; i++)
            {
                newChild.SetAttribute(attrib[i], attribContent[i]);
            }
            newChild.InnerText = content;
            node.AppendChild(newChild);
        }

        public void InsertNode(string mainNode, string childNode, string content)
        {
            XmlNode node = this.xd.SelectSingleNode(mainNode);
            XmlElement newChild = this.xd.CreateElement(childNode);
            newChild.InnerText = content;
            node.AppendChild(newChild);
        }

        public string ReadAttribute(string pathNode, string attributeName)
        {
            string str = string.Empty;
            try
            {
                str = this.xd.SelectSingleNode(pathNode).Attributes[attributeName].Value;
            }
            catch
            {
                ResponseHelper.Write(pathNode);
                ResponseHelper.End();
            }
            return str;
        }

        public string ReadAttribute(string pathNode, string attributeName, string attributeValue)
        {
            string innerText = string.Empty;
            XmlNodeList childNodes = this.ReadNode(pathNode).ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                if (node.Attributes[attributeName].Value == attributeValue)
                {
                    innerText = node.InnerText;
                }
            }
            return innerText;
        }

        public string ReadInnerText(string pathNode)
        {
            return this.xd.SelectSingleNode(pathNode).InnerText;
        }

        public XmlNode ReadNode(string pathNode)
        {
            return this.xd.SelectSingleNode(pathNode);
        }

        public void Save()
        {
            try
            {
                this.xd.Save(this.xmlfile);
            }
            catch
            {
            }
            this.xd = null;
        }

        public void UpdateAttribute(string pathNode, string attributeName, int attributeValue)
        {
            this.xd.SelectSingleNode(pathNode).Attributes[attributeName].Value = attributeValue.ToString();
        }

        public void UpdateAttribute(string pathNode, string attributeName, string attributeValue)
        {
            this.xd.SelectSingleNode(pathNode).Attributes[attributeName].Value = attributeValue;
        }

        public void UpdateAttribute(string pathNode, string attributeName, string attributeValue, string innerText)
        {
            XmlNodeList childNodes = this.ReadNode(pathNode).ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                if (node.Attributes[attributeName].Value == attributeValue)
                {
                    node.InnerText = innerText;
                }
            }
        }

        public void UpdateInnerText(string pathNode, string innerText)
        {
            this.xd.SelectSingleNode(pathNode).InnerText = innerText;
        }
    }
}

