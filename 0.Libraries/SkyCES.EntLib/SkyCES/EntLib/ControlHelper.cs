namespace SkyCES.EntLib
{
    using System;
    using System.Web.UI.WebControls;

    public sealed class ControlHelper
    {
        public static string GetCheckBoxListValue(CheckBoxList control)
        {
            string str = string.Empty;
            foreach (ListItem item in control.Items)
            {
                if (item.Selected)
                {
                    if (str == string.Empty)
                    {
                        str = item.Value;
                    }
                    else
                    {
                        str = str + "," + item.Value;
                    }
                }
            }
            return str;
        }

        public static void SetCheckBoxListValue(CheckBoxList control, string Value)
        {
            if (Value != string.Empty)
            {
                Value = "|" + Value.Replace(",", "|") + "|";
                foreach (ListItem item in control.Items)
                {
                    if (Value.IndexOf("|" + item.Value + "|") > -1)
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
            }
            else
            {
                foreach (ListItem item in control.Items)
                {
                    item.Selected = false;
                }
            }
        }
    }
}

