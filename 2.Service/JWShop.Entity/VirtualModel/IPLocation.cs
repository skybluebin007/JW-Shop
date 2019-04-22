using System;
using System.Collections.Generic;
using System.Text;

namespace JWShop.Entity
{
    public class IPLocationValidResult
    {
        public int code;
        public object data;
    }
    public class IPLocationResult
    {
        public int code;
        public IPLocation data;
    }
    public class IPLocation
    {
        public string country;
        public string country_id;
        public string area;
        public string area_id;
        public string region;
        public string region_id;
        public string city;
        public string city_id;
        public string county;
        public string county_id;
        public string isp;
        public string isp_id;
        public string ip;
    }
}
