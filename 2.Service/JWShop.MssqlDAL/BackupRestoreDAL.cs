using System;
//using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using System.Configuration;
using Dapper;
using System.Linq;
using System.IO;
using System.Xml;

namespace JWShop.MssqlDAL
{
    public  class BackupRestoreDAL
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];
        //private Database databaseDB = DatabaseFactory.CreateDatabase();

        public string BackupData(string path)
        {
            #region 使用Microsoft.Practices.EnterpriseLibrary.Data
            //string database;
            //using (DbConnection connection = databaseDB.CreateConnection())
            //{
            //    database = connection.Database;
            //}
            //string str2 = database + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
            //DbCommand sqlStringCommand = databaseDB.GetSqlStringCommand(string.Format("backup database [{0}] to disk='{1}'", database, path + str2));
            //try
            //{
            //    databaseDB.ExecuteNonQuery(sqlStringCommand);
            //    return str2;
            //}
            //catch
            //{
            //    return string.Empty;
            //}
            #endregion
            using (var conn = new SqlConnection(connectString))
            {
                try
                {
                    string fileName = conn.Database + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";

                    conn.Execute(string.Format("backup database [{0}] to disk='{1}'", conn.Database, path + fileName));
                    return fileName;
                }
                catch { return string.Empty; }
            }
        }

        //public  void Restor()
        //{
        //    try
        //    {
        //        DbCommand sqlStringCommand = databaseDB.GetSqlStringCommand(" ");
        //        databaseDB.ExecuteNonQuery(sqlStringCommand);
        //    }
        //    catch
        //    {
        //    }
        //}

        public  bool RestoreData(string bakFullName)
        {
            #region 使用Microsoft.Practices.EnterpriseLibrary.Data
            //string dataSource;
            //string database;
            //bool flag;
            //using (DbConnection connection = databaseDB.CreateConnection())
            //{
            //    database = connection.Database;
            //    dataSource = connection.DataSource;
            //}
            //SqlConnection connection2 = new SqlConnection(string.Format("Data Source={0};Initial Catalog=master;uid=sa;pwd=sa123", dataSource));
            //try
            //{
            //    connection2.Open();
            //    SqlCommand command = new SqlCommand(string.Format("SELECT spid FROM sys.sysprocesses ,sys.sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", database), connection2);
            //    ArrayList list = new ArrayList();
            //    using (IDataReader reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            list.Add(reader.GetInt16(0));
            //        }
            //    }
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        new SqlCommand(string.Format("KILL {0}", list[i].ToString()), connection2).ExecuteNonQuery();
            //    }
            //    new SqlCommand(string.Format("RESTORE DATABASE [{0}]  FROM DISK = '{1}' WITH REPLACE", database, bakFullName), connection2).ExecuteNonQuery();
            //    flag = true;
            //}
            //catch
            //{
            //    flag = false;
            //}
            //finally
            //{
            //    connection2.Close();
            //}
            //return flag;
            #endregion
            using (var conn = new SqlConnection(connectString))
            {
                try
                {
                    string databaseName = conn.Database;

                    conn.Open();
                    conn.ChangeDatabase("master");
                    var ids = conn.Query<int>(string.Format("SELECT spid FROM sys.sysprocesses ,sys.sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", databaseName));

                    foreach (int id in ids)
                    {
                        conn.Execute(string.Format("KILL {0}", id));
                    }

                    conn.Execute(string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE", databaseName, bakFullName));

                    return true;
                }
                catch { return false; }
            }
        }

        private  string StringCut(string str, string bg, string ed)
        {
            string str2 = str.Substring(str.IndexOf(bg) + bg.Length);
            return str2.Substring(0, str2.IndexOf(ed));
        }

        public  bool InserBackInfo(string fileName, string version, long fileSize)
        {
            string filename = ServerHelper.MapPath("/config/BackupFiles.config");
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNode node = document.SelectSingleNode("root");
                XmlElement newChild = document.CreateElement("backupfile");
                newChild.SetAttribute("BackupName", fileName);
                newChild.SetAttribute("Version", version.ToString());
                newChild.SetAttribute("FileSize", fileSize.ToString());
                newChild.SetAttribute("BackupTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                node.AppendChild(newChild);
                document.Save(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public  DataTable GetBackupFiles()
        {
            DataTable table = new DataTable();
            table.Columns.Add("BackupName", typeof(string));
            table.Columns.Add("Version", typeof(string));
            table.Columns.Add("FileSize", typeof(string));
            table.Columns.Add("BackupTime", typeof(string));
            string filename =ServerHelper.MapPath("/config/BackupFiles.config");
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                XmlElement element = (XmlElement)node;
                DataRow row = table.NewRow();
                row["BackupName"] = element.GetAttribute("BackupName");
                row["Version"] = element.GetAttribute("Version");
                row["FileSize"] = element.GetAttribute("FileSize");
                row["BackupTime"] = element.GetAttribute("BackupTime");
                table.Rows.Add(row);
            }
            return table;
        }

        public bool DeleteBackupFile(string backupName)
        {
            string filename =ServerHelper.MapPath("/config/BackupFiles.config");
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                XmlNodeList childNodes = document.SelectSingleNode("root").ChildNodes;
                foreach (XmlNode node in childNodes)
                {
                    XmlElement element = (XmlElement)node;
                    if (element.GetAttribute("BackupName") == backupName)
                    {
                        document.SelectSingleNode("root").RemoveChild(node);
                    }
                }
                document.Save(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
