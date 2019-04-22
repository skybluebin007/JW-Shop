namespace SkyCES.EntLib
{
    using System;

    public class AccessCondition
    {
        private string conditionString = string.Empty;

        public void Add(string condition)
        {
            if (condition != string.Empty)
            {
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = condition;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + condition;
                }
            }
        }

        public void Add(string fieldName, DateTime value, ConditionType conditionType)
        {
            if (value != DateTime.MinValue)
            {
                string str = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.Equal:
                        str = "=#" + value.ToString() + "#";
                        break;

                    case ConditionType.More:
                        str = ">#" + value.ToString() + "#";
                        break;

                    case ConditionType.Less:
                        str = "<#" + value.ToString() + "#";
                        break;

                    case ConditionType.MoreOrEqual:
                        str = ">=#" + value.ToString() + "#";
                        break;

                    case ConditionType.LessOrEqual:
                        str = "<=#" + value.ToString() + "#";
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str;
                }
            }
        }

        public void Add(string fieldName, decimal value, ConditionType conditionType)
        {
            if (value != -79228162514264337593543950335M)
            {
                string str = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.More:
                        str = ">" + value.ToString();
                        break;

                    case ConditionType.Less:
                        str = "<" + value.ToString();
                        break;

                    case ConditionType.MoreOrEqual:
                        str = ">=" + value.ToString();
                        break;

                    case ConditionType.LessOrEqual:
                        str = "<=" + value.ToString();
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str;
                }
            }
        }

        public void Add(string fieldName, int value, ConditionType conditionType)
        {
            if (value != -2147483648)
            {
                string str = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.Equal:
                        str = "=" + value;
                        break;

                    case ConditionType.More:
                        str = ">" + value;
                        break;

                    case ConditionType.Less:
                        str = "<" + value;
                        break;

                    case ConditionType.In:
                        str = " IN (" + value + ")";
                        break;

                    case ConditionType.MoreOrEqual:
                        str = ">=" + value;
                        break;

                    case ConditionType.LessOrEqual:
                        str = "<=" + value;
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str;
                }
            }
        }

        public void Add(string fieldName, string value, ConditionType conditionType)
        {
            if (value != string.Empty)
            {
                string str = string.Empty;
                switch (conditionType)
                {
                    case ConditionType.Equal:
                        str = "='" + StringHelper.SearchSafe(value) + "'";
                        break;

                    case ConditionType.Like:
                        str = " LIKE '%" + StringHelper.SearchSafe(value) + "%'";
                        break;

                    case ConditionType.In:
                        str = " IN (" + StringHelper.SearchSafe(value) + ")";
                        break;

                    case ConditionType.NotIn:
                        str = " NOT IN (" + StringHelper.SearchSafe(value) + ")";
                        break;
                }
                if (this.conditionString == string.Empty)
                {
                    this.conditionString = fieldName + str;
                }
                else
                {
                    this.conditionString = this.conditionString + " AND " + fieldName + str;
                }
            }
        }

        public string ToString()
        {
            return this.conditionString;
        }
    }
}

