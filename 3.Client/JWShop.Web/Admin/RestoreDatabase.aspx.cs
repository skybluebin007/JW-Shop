using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JWShop.Entity;
using SkyCES.EntLib;
using System.IO;

namespace JWShop.Web.Admin
{
    public partial class RestoreDatabase : JWShop.Page.AdminBasePage
    {
        protected MssqlDAL.BackupRestoreDAL backupDal = new MssqlDAL.BackupRestoreDAL();
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
                ScriptHelper.Alert("数据库已恢复完毕", "RestoreDatabase.aspx");
            }
            else
            {
                ScriptHelper.Alert("数据库恢复失败，请重试", "RestoreDatabase.aspx");
            }
        }

        protected void Delete()
        {
            string filename = RequestHelper.GetQueryString<string>("filename");
            if (backupDal.DeleteBackupFile(filename))
            {
                File.Delete(HttpContext.Current.Request.MapPath("/Upload/data/Backup/" + filename));
                this.BindBackupData();
                ScriptHelper.Alert("成功删除了选择的备份文件", "RestoreDatabase.aspx");
            }
            else
            {
                ScriptHelper.Alert("未知错误", "RestoreDatabase.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.Page.IsPostBack)
            {
                CheckAdminPower("RestoreData", PowerCheckType.Single);

                this.BindBackupData();
                string action = RequestHelper.GetQueryString<string>("Action").ToLower();
                switch (action)
                {
                    case "restore":
                        Restore();
                        break;
                    case "delete":
                        Delete();
                        break;
                }
            }           
        }        
    }
}