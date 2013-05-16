using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonClasses.Helpers
{
    public static class DataTypeHelper
    {
        public static DataType StrToDataType(string str)
        {
            switch (str.ToLower())
            {
                case "bit":
                    return DataType.Bit;
                case "int":
                case "integer":
                    return DataType.Int;
                case "decimal":
                    return DataType.Decimal;
                case "money":
                    return DataType.Money;
                case "float":
                    return DataType.Float;
                case "datetime":
                    return DataType.DateTime;
                case "varchar":
                    return DataType.Varchar;
                case "nvarchar":
                    return DataType.Nvarchar;
                case "varbinary":
                    return DataType.Varbinary;
                default:
                    return DataType.None;
            }
        }

        public static string GetCSharpType(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Bit:
                    return "bool";
                case DataType.Int:
                    return "int";
                case DataType.Varchar:
                case DataType.Nvarchar:
                    return "string";
                case DataType.DateTime:
                    return "DateTime";
                case DataType.Decimal:
                case DataType.Money:
                    return "decimal";
                case DataType.Float:
                    return "double";
                case DataType.Varbinary:
                    return "byte[]";
                default:
                    return String.Empty;
            }
        }

        public static string GetCSharpType(DataType dataType, bool canBeNull)
        {
            string result = GetCSharpType(dataType);
            if (canBeNull && !result.Equals("string"))
                result += "?";
            return result;
        }
    }
}
