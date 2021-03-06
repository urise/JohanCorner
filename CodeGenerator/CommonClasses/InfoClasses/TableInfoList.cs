﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonClasses.Helpers;

namespace CommonClasses.InfoClasses
{
    public class TableInfoList: List<TableInfo>
    {
        public TableInfoList()
        {
        }

        public TableInfoList(IEnumerable<string> lines)
        {
            foreach(string line in lines)
            {
                if (String.IsNullOrEmpty(line.Trim())) continue;
                if (line.StartsWith("#")) continue;
                if (line.StartsWith("table "))
                {
                    var tableInfo = new TableInfo();
                    string str = line.Substring(5).Trim();
                    int n = str.IndexOf(':');
                    tableInfo.DatabaseTableName = n == -1 ? str : str.Substring(0, n);
                    tableInfo.ModelTableName = n == -1 ? str : str.Substring(n + 1);
                    Add(tableInfo);
                }
                else
                    this[Count-1].Columns.Add(new ColumnInfo(line));
            }
        }

        public string GetSql()
        {
            var result = new StringBuilder();
            foreach (TableInfo tableInfo in this)
            {
                result.AppendLine(tableInfo.GetSql());
            }
            return result.ToString();
        }

        public string GetImplicitText()
        {
            var result = new StringBuilder();
            result.AddHeaderWarning();
            result.AppendLine("using CommonClasses.DbClasses;");
            result.AppendLine();
            result.AppendLine("namespace " + AppConfiguration.DbModelNamespace);
            result.AppendLine("{");
            foreach (TableInfo tableInfo in this.Where(r => !r.EncodedColumnsExist()))
            {
                result.AppendLine(tableInfo.GetImplicitText());
            }
            result.AppendLine("}");
            return result.ToString();
        }

        public string GetConversionsText()
        {
            var result = new StringBuilder();
            result.AddHeaderWarning();
            result.AppendLine("using CommonClasses.DbClasses;");
            result.AppendLine("using Interfaces.MiscInterfaces;");
            result.AppendLine();
            result.AppendLine("namespace " + AppConfiguration.DbModelNamespace);
            result.AppendLine("{");
            result.AppendLine("\tpublic static class Conversions");
            result.AppendLine("\t{");

            foreach (TableInfo tableInfo in this)
            {
                result.AppendLine(tableInfo.GetToCommonText());
            }
            result.AppendLine("\t}");
            result.AppendLine("}");
            return result.ToString();
        }

        public string GetServiceProxyAutoMethods()
        {
            var result = new StringBuilder();
            result.AddHeaderWarning();
            result.AppendLine("using System.Collections.Generic;");
            result.AppendLine("using " + AppConfiguration.DbClassesNamespace + ";");
            result.AppendLine("using " + AppConfiguration.DbRepositoryNamespace + ";");
            if (AppConfiguration.UseInterfacesForDbClasses)
                result.AppendLine("using " + AppConfiguration.DbInterfacesNamespace + ";");
            result.AppendLine();
            result.AppendLine("namespace " + AppConfiguration.ServiceProxyNamespace);
            result.AppendLine("{");
            result.AppendLine("\t\tpublic partial class ServiceProxySingleton");
            result.AppendLine("\t{");
            foreach (TableInfo tableInfo in this)
            {
                result.AppendLine(tableInfo.GetServiceAutoMethods());
            }
            result.AppendLine("\t}");
            result.AppendLine("}");
            return result.ToString();
        }

        public string GetFilteredContext()
        {
            var result = new StringBuilder();
            result.AddHeaderWarning();
            result.AppendLine("using System.Linq;");
            result.AppendLine("using " + AppConfiguration.DbModelNamespace + ";");
            result.AppendLine();
            result.AppendLine("namespace " + AppConfiguration.FilteredContextNamespace);
            result.AppendLine("{");
            result.AppendLine("\tpublic partial class FilteredContext");
            result.AppendLine("\t{");

            string field = AppConfiguration.FilteringField;
            string paramField = field.Substring(0, 1).ToLower() + field.Substring(1);
            string privateField = "_" + paramField;

            result.AppendLine("\t\t#region Tables");
            result.AppendLine();

            foreach (TableInfo tableInfo in this)
            {
                result.AppendLine(tableInfo.GetFilteredContextTableMethod(field, privateField));
            }
            result.AppendLine("\t\t#endregion");
            
            result.AppendLine();
            result.AppendLine("\t\t#region Add To Tables");
            result.AppendLine();

            foreach (TableInfo tableInfo in this)
            {
                result.AppendLine(tableInfo.GetFilteredContextAddToTableMethod(field, privateField));
            }
            result.AppendLine("\t\t#endregion");

            result.AppendLine("\t}");
            result.AppendLine("}");
            return result.ToString();
        }

        public void CreateResultFiles(string baseFolder)
        {
            //string folderName = FileHelper.CreateDateNameFolder(baseFolder);
            //File.WriteAllText(Path.Combine(folderName, "dbscript.sql"), GetSql());
            File.WriteAllText(AppConfiguration.ImplicitsFileName, GetImplicitText());
            File.WriteAllText(AppConfiguration.ConversionsFileName, GetConversionsText());
            //File.WriteAllText(Path.Combine(folderName, "ServiceProxyAutoMethods.cs"), GetServiceProxyAutoMethods());
            File.WriteAllText(AppConfiguration.FilteredContextAutoMethodsFileName, GetFilteredContext());

            foreach (TableInfo tableInfo in this)
            {
                CodeHelper.AddOrChangeFile(AppConfiguration.CommonClassesDir, 
                    Path.Combine(AppConfiguration.DbClassesDir, tableInfo.ModelTableName + "Db.cs"),
                    tableInfo.GetDbClassText(), AppConfiguration.CommonClassesProjectFileName);

                CodeHelper.AddOrChangeFile(AppConfiguration.DbRepositoryDir,
                    Path.Combine(AppConfiguration.RepositoriesDir, tableInfo.ModelTableName + "Repository.cs"),
                    tableInfo.GetRepositoryText(), AppConfiguration.DbRepositoryProjectFileName);

                if (AppConfiguration.UseInterfacesForDbClasses)
                {
                    CodeHelper.AddOrChangeFile(AppConfiguration.InterfacesDir,
                        Path.Combine(AppConfiguration.DbInterfacesDir, "I" + tableInfo.ModelTableName + "Db.cs"),
                        tableInfo.GetDbInterfaceText(), AppConfiguration.InterfacesProjectFileName);
                }
            }
        }
    }
}
