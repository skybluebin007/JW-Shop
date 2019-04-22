
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using JWShop.Common;
using JWShop.Business;
using JWShop.Entity;
using SkyCES.EntLib;

namespace JWShop.Web.Admin
{
    public partial class BackUp : JWShop.Page.AdminBasePage
    {
        protected MssqlDAL.BackupRestoreDAL backupDal = new MssqlDAL.BackupRestoreDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdminPower("BackupData", PowerCheckType.Single);
            this.BindBackupData();
            string action = RequestHelper.GetQueryString<string>("Action").ToLower();
            switch (action)
            {
                case "restore":
                    CheckAdminPower("RestoreData", PowerCheckType.Single);
                    Restore();
                    break;
                case "delete":
                    Delete();
                    break;
            }
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            MssqlDAL.BackupRestoreDAL backDAL = new MssqlDAL.BackupRestoreDAL();
            string str = backDAL.BackupData(HttpContext.Current.Request.MapPath("/Upload/data/Backup/"));
            if (!string.IsNullOrEmpty(str))
            {
                string fileName = HttpContext.Current.Request.MapPath("/Upload/data/Backup/" + str);
                FileInfo info = new FileInfo(fileName);
                if (backDAL.InserBackInfo(str, Common.Global.Version, info.Length))
                {
                    ScriptHelper.Alert("备份数据成功", RequestHelper.RawUrl);
                }
                else
                {
                    File.Delete(fileName);
                    ScriptHelper.Alert("备份数据失败，可能是同时备份的人太多，请重试", RequestHelper.RawUrl);
                }
            }
            else
            {
                ScriptHelper.Alert("备份数据失败，可能是您的数据库服务器和web服务器不是同一台服务器", RequestHelper.RawUrl);
            }
        }

        protected void BindBackupData()
        {
            RecordList.DataSource = backupDal.GetBackupFiles();
            RecordList.DataBind();
        }

        protected void Restore()
        {
            string filename = RequestHelper.GetQueryString<string>("filename");
            if (backupDal.RestoreData(HttpContext.Current.Request.MapPath("/Upload/data/Backup/" + filename)))
            {
                ScriptHelper.Alert("数据库已还原完毕", "BackUp.aspx");
            }
            else
            {
                ScriptHelper.Alert("数据库还原失败，请重试", "BackUp.aspx");
            }
        }

        protected void Delete()
        {
            string filename = RequestHelper.GetQueryString<string>("filename");
            if (backupDal.DeleteBackupFile(filename))
            {
                File.Delete(HttpContext.Current.Request.MapPath("/Upload/data/Backup/" + filename));
                this.BindBackupData();
                ScriptHelper.Alert("成功删除了选择的备份文件", "BackUp.aspx");
            }
            else
            {
                ScriptHelper.Alert("未知错误", "RestoreDatabase.aspx");
            }
        }
    }
}