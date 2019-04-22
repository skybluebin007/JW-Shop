namespace SkyCES.EntLib
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class RequiredFieldTypeControlsConverter : StringConverter
    {
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ArrayList values = new ArrayList();
            values.Add("暂无校验");
            values.Add("数据校验");
            values.Add("电子邮箱");
            values.Add("移动手机");
            values.Add("家用电话");
            values.Add("身份证号码");
            values.Add("网页地址");
            values.Add("日期");
            values.Add("日期时间");
            values.Add("金额");
            values.Add("IP地址");
            values.Add("IP地址带端口");
            values.Add("自定义验证表达式");
            values.Add("正整数");
            values.Add("百分比");
            values.Add("整数");
            return new TypeConverter.StandardValuesCollection(values);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}

