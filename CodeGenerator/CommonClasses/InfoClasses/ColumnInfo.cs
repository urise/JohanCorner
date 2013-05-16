using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonClasses.Helpers;

namespace CommonClasses.InfoClasses
{
    public class ColumnInfo
    {
        public string ColumnName { get; set; }
        public DataType ColumnType { get; set; }
        public string Size { get; set; }
        public string Precision { get; set; }
        public bool CanBeNull { get; set; }
        public string DefaultValue { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string ForeignKeyTable { get; set; }
        public string ForeignKeyColumn { get; set; }
        public bool Encoded { get; set; }

        public ColumnInfo(){}

        public ColumnInfo(string line)
        {
            line = line.Trim();
            if (line.EndsWith("ENCODED"))
            {
                Encoded = true;
                line = line.Remove(line.Length - " ENCODED".Length);
            }
            if (String.IsNullOrEmpty(line)) return;
            string[] items = line.Split(' ');
            ColumnName = items[0];
            if (items.Length == 1) return;
            SetColumnType(items[1]);
            CanBeNull = !line.ToLower().Contains("not null");
            IsPrimaryKey = items.Contains("PK");
            IsIdentity = items.Contains("identity");
            int n = line.IndexOf("=>");
            if (n != -1)
            {
                string str = line.Substring(n + 2).Trim();
                int n1 = str.IndexOf("(");
                if (n1 == -1)
                {
                    ForeignKeyTable = str;
                    ForeignKeyColumn = ColumnName;
                }
                else
                {
                    ForeignKeyTable = str.Substring(0, n1);
                    int n2 = str.IndexOf(')');
                    ForeignKeyColumn = str.Substring(n1 + 1, n2 - n1 - 1);
                }
            }
            n = items.ToList().IndexOf("default");
            if (n != -1) DefaultValue = items[n + 1];
        }

        private void SetColumnType(string strType)
        {
            int n = strType.IndexOf('(');
            if (n == -1)
            {
                ColumnType = DataTypeHelper.StrToDataType(strType);
                Size = string.Empty;
                Precision = string.Empty;
            }
            else
            {
                ColumnType = DataTypeHelper.StrToDataType(strType.Substring(0, n));
                int n1 = strType.IndexOf(')');
                string str = strType.Substring(n + 1, n1 - n - 1);
                n1 = str.IndexOf(',');
                Size = n1 == -1 ? str : str.Substring(0, n1);
                Precision = n1 == -1 ? string.Empty : str.Substring(n1 + 1);
            }
        }

        public string GetSqlStatement()
        {
            var result = new StringBuilder();
            result.Append(ColumnName);
            result.Append(" " + ColumnType.ToString().ToLower());
            if (!string.IsNullOrEmpty(Size)) result.Append("(" + Size + ")");
            if (!CanBeNull) result.Append(" not null");
            if (IsIdentity) result.Append(" identity");
            if (!String.IsNullOrEmpty(DefaultValue))
                result.Append(" default " + DefaultValue);
            return result.ToString();
        }

        public string CSharpType
        {
            get
            {
                return Encoded ? DataTypeHelper.GetCSharpType(DataType.Decimal, CanBeNull) : 
                    DataTypeHelper.GetCSharpType(ColumnType, CanBeNull);
            }
        }
    }
}
