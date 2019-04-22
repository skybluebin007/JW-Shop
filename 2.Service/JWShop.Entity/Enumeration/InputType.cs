using System;
using System.Collections.Generic;
using System.Text;
using SkyCES.EntLib;

namespace JWShop.Entity
{
    /// <summary>
    /// 输入类型
    /// </summary> 
    public enum InputType
    {
        /// <summary>
        /// 单行文本
        /// </summary>  
        [Enum("单行文本")]
        Text = 1,
        /// <summary>
        /// 多行文本
        /// </summary>
        [Enum("多行文本")]
        Textarea,
        /// <summary>
        /// 关键字
        /// </summary>
        [Enum("关键字")]
        KeyWord,
        /// <summary>
        /// 下拉选择
        /// </summary>
        [Enum("下拉选择")] 
        Select,
        /// <summary>
        /// 单选
        /// </summary>
        [Enum("单选")]
        Radio,
        /// <summary>
        /// 多选
        /// </summary>
        [Enum("多选")]
        CheckBox        
    }
}