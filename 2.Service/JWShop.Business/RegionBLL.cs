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
    /// 地区业务逻辑。
    /// </summary>
    public sealed class RegionBLL
    {

        private static readonly IRegion dal = FactoryHelper.Instance<IRegion>(Global.DataProvider, "RegionDAL");
        private static readonly string cacheKey = CacheKey.ReadCacheKey("Region");

        /// <summary>
        /// 增加一条地区数据
        /// </summary>
        /// <param name="region">地区模型变量</param>
        public static int AddRegion(RegionInfo region)
        {
            region.ID = dal.AddRegion(region);
            CacheHelper.Remove(cacheKey);
            return region.ID;
        }

        /// <summary>
        /// 更新一条地区数据
        /// </summary>
        /// <param name="region">地区模型变量</param>
        public static void UpdateRegion(RegionInfo region)
        {
            dal.UpdateRegion(region);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 删除一条地区数据
        /// </summary>
        /// <param name="id">地区的主键值</param>
        public static void DeleteRegion(int id)
        {
            dal.DeleteRegion(id);
            CacheHelper.Remove(cacheKey);
        }


        /// <summary>
        /// 从缓存中读取地区所有数据列表
        /// </summary>
        /// <returns>地区数据模型</returns>
        public static List<RegionInfo> ReadRegionCacheList()
        {
            if (CacheHelper.Read(cacheKey) == null)
            {
                CacheHelper.Write(cacheKey, dal.ReadRegionAllList());
            }
            return (List<RegionInfo>)CacheHelper.Read(cacheKey);
        }

        /// <summary>
        /// 从缓存中读取一条地区数据
        /// </summary>
        /// <param name="id">地区的主键值</param>
        /// <returns>地区数据模型</returns>
        public static RegionInfo ReadRegionCache(int id)
        {
            RegionInfo region = new RegionInfo();
            List<RegionInfo> regionList = ReadRegionCacheList();
            foreach (RegionInfo temp in regionList)
            {
                if (temp.ID == id)
                {
                    region = temp;
                    break;
                }
            }
            return region;
        }

        /// <summary>
        /// 读取第一级地区分类列表
        /// </summary>
        /// <returns>地区数据列表</returns>
        public static List<RegionInfo> ReadRegionRootList()
        {
            List<RegionInfo> result = new List<RegionInfo>();
            List<RegionInfo> regionList = ReadRegionCacheList();
            foreach (RegionInfo region in regionList)
            {
                if (region.FatherID == 0)
                {
                    result.Add(region);
                }
            }
            return result;
        }

        /// <summary>
        /// 读取某大类的二级子分类
        /// </summary>
        /// <param name="fatherID">父类ID</param>
        /// <returns>地区数据列表</returns>
        public static List<RegionInfo> ReadRegionChildList(int fatherID)
        {
            List<RegionInfo> result = new List<RegionInfo>();
            List<RegionInfo> regionList = ReadRegionCacheList();
            foreach (RegionInfo region in regionList)
            {
                if (region.FatherID == fatherID)
                {
                    result.Add(region);
                }
            }
            return result;
        }

        /// <summary>
        /// 读取某大类的二级子分类
        /// </summary>
        /// <param name="fatherID">父类ID</param>
        /// <param name="depth">层级</param>
        /// <returns>地区数据列表</returns>
        private static List<RegionInfo> ReadRegionChildList(int fatherID, int depth)
        {
            List<RegionInfo> result = new List<RegionInfo>();
            List<RegionInfo> regionList = ReadRegionCacheList();
            foreach (RegionInfo region in regionList)
            {
                if (region.FatherID == fatherID)
                {
                    RegionInfo temp = (RegionInfo)ServerHelper.CopyClass(region);
                    string tempString = string.Empty;
                    for (int i = 1; i < depth; i++)
                    {
                        tempString += HttpContext.Current.Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                    temp.RegionName = tempString + temp.RegionName;
                    result.Add(temp);
                    result.AddRange(ReadRegionChildList(temp.ID, depth + 1));
                }
            }
            return result;
        }


        /// <summary>
        /// 读取名字已经缩进好的分类列表
        /// </summary>
        /// <returns>地区数据列表</returns>
        public static List<RegionInfo> ReadRegionNamedList()
        {
            List<RegionInfo> result = new List<RegionInfo>();
            List<RegionInfo> regionList = ReadRegionRootList();
            foreach (RegionInfo region in regionList)
            {
                result.Add(region);
                result.AddRange(ReadRegionChildList(region.ID, 2));
            }
            return result;
        }


        /// <summary>
        /// 上移地区分类
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveUpRegion(int id)
        {
            dal.MoveUpRegion(id);
            CacheHelper.Remove(cacheKey);
        }

        /// <summary>
        /// 下移地区分类
        /// </summary>
        /// <param name="id">要移动的id</param>
        public static void MoveDownRegion(int id)
        {
            dal.MoveDownRegion(id);
            CacheHelper.Remove(cacheKey);
        }


        /// <summary>
        /// 搜索地区列表数据
        /// </summary>
        /// <param name="id">地区列表的主键值</param>
        /// <returns>地区列表数据模型</returns>
        public static string SearchRegionList(int fatherID)
        {
            string result = string.Empty;
            List<RegionInfo> regionList = ReadRegionCacheList();
            foreach (RegionInfo region in regionList)
            {
                if (region.FatherID == fatherID)
                {
                    if (result == string.Empty)
                    {
                        result = region.ID.ToString() + "," + region.RegionName;
                    }
                    else
                    {
                        result += "|" + region.ID.ToString() + "," + region.RegionName;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 读取配送表数据
        /// </summary>
        /// <param name="strID">配送表的主键值</param>
        /// <returns>配送表数据模型</returns>
        public static List<RegionInfo> ReadRegionList(string strID)
        {
            strID = "," + strID + ",";
            List<RegionInfo> result = new List<RegionInfo>();
            List<RegionInfo> regionList = ReadRegionCacheList();
            foreach (RegionInfo region in regionList)
            {
                if (strID.IndexOf("," + region.ID + ",") > -1)
                {
                    result.Add(region);
                }
            }
            return result;
        }


        /// <summary>
        /// 读取编排好的地区名称
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static string RegionNameList(string idList)
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
                    string tempRegionName = string.Empty;
                    foreach (string id in temp.Split('|'))
                    {
                        if (tempRegionName == string.Empty)
                        {
                            tempRegionName = RegionBLL.ReadRegionCache(Convert.ToInt32(id)).RegionName;
                        }
                        else
                        {
                            tempRegionName += "   " + RegionBLL.ReadRegionCache(Convert.ToInt32(id)).RegionName;
                        }
                    }
                    if (tempRegionName != string.Empty)
                    {
                        if (content == string.Empty)
                        {
                            content = tempRegionName;
                        }
                        else
                        {
                            content += "，" + tempRegionName;
                        }
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 读取地区的无限级分类
        /// </summary>
        /// <returns></returns>
        public static List<UnlimitClassInfo> ReadRegionUnlimitClass()
        {
            List<RegionInfo> regionList = ReadRegionCacheList();
            List<UnlimitClassInfo> unlimitClassList = new List<UnlimitClassInfo>();
            foreach (RegionInfo region in regionList)
            {
                UnlimitClassInfo unlimitClass = new UnlimitClassInfo();
                unlimitClass.ClassID = region.ID;
                unlimitClass.ClassName = region.RegionName;
                unlimitClass.FatherID = region.FatherID;
                unlimitClassList.Add(unlimitClass);
            }
            return unlimitClassList;
        }

        /// <summary>
        /// 读取地区的湖南级分类
        /// </summary>
        /// <returns></returns>
        public static List<UnlimitClassInfo> ReadRegionUnlimitClassHN()
        {
            List<RegionInfo> regionList = new List<RegionInfo>();

            List<UnlimitClassInfo> unlimitClassList = new List<UnlimitClassInfo>();
            UnlimitClassInfo unlimitClass = new UnlimitClassInfo();
            unlimitClass.ClassID = 27;
            unlimitClass.ClassName = RegionBLL.ReadRegionCache(27).RegionName;
            unlimitClass.FatherID = RegionBLL.ReadRegionCache(27).FatherID;
            unlimitClassList.Add(unlimitClass);
            regionList = RegionBLL.ReadRegionChildList(27);

            foreach (RegionInfo region in regionList)
            {
                unlimitClass = new UnlimitClassInfo();
                unlimitClass.ClassID = region.ID;
                unlimitClass.ClassName = region.RegionName;
                unlimitClass.FatherID = region.FatherID;
                unlimitClassList.Add(unlimitClass);
            }

            return unlimitClassList;
        }


        /// <summary>
        /// 根据城市名称取得IdList字符串（|1|10|100|）
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static string ReadRegionIdList(string province, string cityName, string townname)
        {
            //取得所有的市
            List<RegionInfo> regionCityList = new List<RegionInfo>();
            List<RegionInfo> regionList = ReadRegionCacheList();
            var regionProvinceList = regionList.Where(k => (k.FatherID == 1 && k.RegionName.Contains(province))).ToList();
            foreach (RegionInfo region in regionProvinceList)
            {
                regionCityList.AddRange(regionList.Where(k => k.FatherID == region.ID).ToList());
            }

            //取得符合城市名称的市
            var regionCity = regionCityList.FirstOrDefault(k => k.RegionName.Contains(cityName)) ?? new RegionInfo();
            if (regionCity.ID > 0)
            {
                RegionInfo regionTown = RegionBLL.ReadRegionChildList(regionCity.ID).FirstOrDefault(k => k.RegionName == townname);
                if (regionTown == null)
                {
                    regionTown = new RegionInfo();
                    regionTown.RegionName = townname;
                    regionTown.FatherID = regionCity.ID;
                    RegionBLL.AddRegion(regionTown);
                    CacheHelper.Remove(cacheKey);
                }                
                return string.Format("|1|{0}|{1}|{2}|", regionCity.FatherID, regionCity.ID, regionTown.ID);
            }
            else
            {
                var newregion = new RegionInfo();
                newregion.RegionName = cityName;
                newregion.FatherID = regionProvinceList[0].ID;
                int newcityid = RegionBLL.AddRegion(newregion);
                newregion = new RegionInfo();
                newregion.RegionName = townname;
                newregion.FatherID = newcityid;
                int newtownid = RegionBLL.AddRegion(newregion);
                CacheHelper.Remove(cacheKey);
                return "|1|" + regionProvinceList[0].ID + "|" + newcityid + "|" + newtownid + "|";
            }
            
        }

        public static string ReadRegionIdList(string cityName)
        {
            //取得所有的市
            List<RegionInfo> regionCityList = new List<RegionInfo>();
            List<RegionInfo> regionList = ReadRegionCacheList();
            var regionProvinceList = regionList.Where(k => k.FatherID == 1).ToList();
            foreach (RegionInfo region in regionProvinceList)
            {
                regionCityList.AddRange(regionList.Where(k => k.FatherID == region.ID).ToList());
            }

            //取得符合城市名称的市
            var regionCity = regionCityList.FirstOrDefault(k => k.RegionName.Contains(cityName)) ?? new RegionInfo();
            return string.Format("|1|{0}|{1}|", regionCity.FatherID, regionCity.ID);
        }
        /// <summary>
        /// 根据regionId获取省市名称
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public static string ReadCityName(string regionId) {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(regionId)) {
                foreach (string rid in regionId.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)) {
                    int _rid = ShopCommon.ConvertToT<Int32>(rid);
                    if (_rid > 0) result += RegionBLL.ReadRegionCache(_rid).RegionName;
                }
            }
            return result;
        }
    }
}