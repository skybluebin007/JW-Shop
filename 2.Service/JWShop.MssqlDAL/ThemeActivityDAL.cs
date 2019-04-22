using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JWShop.Common;
using JWShop.Entity;
using JWShop.IDAL;
using SkyCES.EntLib;
using Dapper;
using System.Linq;

namespace JWShop.MssqlDAL
{
    public sealed class ThemeActivityDAL : IThemeActivity
    {
        private readonly string connectString = ConfigurationManager.AppSettings["ConnectionString"];

        public int Add(ThemeActivityInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"INSERT INTO ThemeActivity( Name,Photo,[Description],Css,CssMobile,ProductGroup,Style) VALUES(@Name,@Photo,@Description,@Css,@CssMobile,@ProductGroup,@Style);
                            select SCOPE_IDENTITY()";

                return conn.Query<int>(sql, entity).Single();
            }
        }

        public void Update(ThemeActivityInfo entity)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = @"UPDATE ThemeActivity SET Name = @Name, Photo = @Photo, [Description] = @Description, Css = @Css,CssMobile=@CssMobile, ProductGroup = @ProductGroup, Style = @Style
                            where Id=@Id";

                conn.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ThemeActivity where id=@id";

                conn.Execute(sql, new { id = id });
            }
        }

        public void Delete(int[] ids)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "delete from ThemeActivity where id in @ids";

                conn.Execute(sql, new { ids = ids });
            }
        }

        public ThemeActivityInfo Read(int id)
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ThemeActivity where id=@id";

                var data = conn.Query<ThemeActivityInfo>(sql, new { id = id }).SingleOrDefault();
                return data ?? new ThemeActivityInfo();
            }
        }

        public List<ThemeActivityInfo> ReadList()
        {
            using (var conn = new SqlConnection(connectString))
            {
                string sql = "select * from ThemeActivity ";

                return conn.Query<ThemeActivityInfo>(sql).ToList();
            }
        }

    }
}