using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using CommonClasses.Helpers;

namespace CommonClasses.InfoClasses
{
    public class TableInfo
    {
        public string DatabaseTableName { get; set; }
        public string ModelTableName { get; set; }
        public List<ColumnInfo> Columns { get; set; }

        public string InterfacePreffix
        {
            get { return AppConfiguration.UseInterfacesForDbClasses ? "I" : string.Empty; }
        }

        public bool EncodedColumnsExist()
        {
            return Columns.Where(r => r.Encoded).Any();
        }

        public TableInfo()
        {
            Columns = new List<ColumnInfo>();
        }

        public string GetLogMethod()
        {
            return ContainsFiltreringColumn() ? "LogToDb" : "LogUnlinkedToDb";
        }

        public TableInfo(string databaseTableName, string modelTableName)
        {
            DatabaseTableName = databaseTableName;
            ModelTableName = modelTableName;
            Columns = new List<ColumnInfo>();
        }

        public bool IsLogging()
        {
            return AppConfiguration.DatabaseLogging && !DatabaseTableName.InCommaList(AppConfiguration.NoLogTables);
        }

        public string GetSql()
        {
            var result = new StringBuilder();
            result.Append("create table " + DatabaseTableName + "(");
            for (int index = 0; index < Columns.Count; index++)
            {
                var columnInfo = Columns[index];
                result.AppendLine();
                result.Append("\t" + columnInfo.GetSqlStatement());
                if (index != Columns.Count - 1)
                    result.Append(",");
            }
            ColumnInfo primaryKey = GetPrimaryKey();
            if (primaryKey != null)
            {
                result.AppendLine(",");
                result.AppendLine("\tconstraint PK_" + DatabaseTableName);
                result.Append("\t\tprimary key(" + primaryKey.ColumnName + ")");
            }
            foreach (ColumnInfo foreignKey in GetForeignKeys())
            {
                result.AppendLine(",");
                result.AppendLine("\tconstraint FK_" + DatabaseTableName +
                                  (foreignKey.ColumnName.Equals(foreignKey.ForeignKeyColumn)
                                       ? String.Empty
                                       : foreignKey.ColumnName) +
                                  "_" + foreignKey.ForeignKeyTable);
                result.AppendLine("\t\tforeign key(" + foreignKey.ColumnName + ")");
                result.Append("\t\treferences " + foreignKey.ForeignKeyTable + "(" + foreignKey.ForeignKeyColumn + ")");
            }
            result.AppendLine();
            result.AppendLine(")");
            result.AppendLine("print 'Table " + DatabaseTableName + " has been created'");
            return result.ToString();
        }

        public ColumnInfo GetPrimaryKey()
        {
            return Columns.Where(r => r.IsPrimaryKey).FirstOrDefault();
        }

        public IEnumerable<ColumnInfo> GetForeignKeys()
        {
            return Columns.Where(r => !String.IsNullOrEmpty(r.ForeignKeyTable));
        }

        public string GetDbClassText()
        {
            var result = new StringBuilder();
            result.AddHeaderWarning();
            result.AppendLine("using System;");
            if (AppConfiguration.UseInterfacesForDbClasses)
                result.AppendLine("using " + AppConfiguration.DbInterfacesNamespace + ";");
            result.AppendLine();
            result.AppendLine("namespace " + AppConfiguration.DbClassesNamespace);
            result.AppendLine("{");
            result.Append("\tpublic class " + ModelTableName + "Db");
            if (AppConfiguration.UseInterfacesForDbClasses)
                result.Append(": I" + ModelTableName + "Db");
            result.AppendLine();
            result.AppendLine("\t{");
            foreach (ColumnInfo columnInfo in Columns)
            {
                result.AppendLine("\t\tpublic " + columnInfo.CSharpType + " " +
                                  columnInfo.ColumnName + " { get; set; }");
            }
            result.AppendLine("\t}");
            result.AppendLine("}");
            return result.ToString();
        }

        public string GetDbInterfaceText()
        {
            var result = new StringBuilder();
            result.AddHeaderWarning();
            result.AppendLine("using System;");
            result.AppendLine();
            result.AppendLine("namespace " + AppConfiguration.DbInterfacesNamespace);
            result.AppendLine("{");
            result.AppendLine("\tpublic interface I" + ModelTableName + "Db");
            result.AppendLine("\t{");
            foreach (ColumnInfo columnInfo in Columns)
            {
                result.AppendLine("\t\t" + columnInfo.CSharpType + " " +
                                  columnInfo.ColumnName + " { get; set; }");
            }
            result.AppendLine("\t}");
            result.AppendLine("}");
            return result.ToString();
        }

        private void AppendConversionBody(StringBuilder result)
        {
            string paramName = ModelTableName.Substring(0, 1).ToLower() + ModelTableName.Substring(1);
            result.AppendLine("\t\t{");
            result.AppendLine("\t\t\tif (" + paramName + " == null) return null;");
            result.AppendLine();
            result.AppendLine("\t\t\treturn new " + ModelTableName + "Db");
            result.AppendLine("\t\t\t{");
            for (int index = 0; index < Columns.Count; index++)
            {
                ColumnInfo columnInfo = Columns[index];
                result.Append("\t\t\t\t" + columnInfo.ColumnName + " = ");
                if (columnInfo.Encoded)
                    result.Append("encryptor.DecryptDecimal(" + paramName + "." + columnInfo.ColumnName + ")");
                else
                    result.Append(paramName + "." + columnInfo.ColumnName);

                if (index != Columns.Count - 1) result.Append(",");
                result.AppendLine();
            }
            result.AppendLine("\t\t\t};");
            result.AppendLine("\t\t}");
        }

        public string GetImplicitText()
        {
            var result = new StringBuilder();
            string paramName = ModelTableName.Substring(0, 1).ToLower() + ModelTableName.Substring(1);
            result.AppendLine("\tpublic partial class " + ModelTableName);
            result.AppendLine("\t{");

            result.AppendLine("\t\tpublic static implicit operator " +
                              ModelTableName + "Db(" + ModelTableName + " " +
                              paramName + ")");
            AppendConversionBody(result);
            result.AppendLine("\t}");
            return result.ToString();
        }

        public string GetToCommonText()
        {
            var result = new StringBuilder();
            string paramName = ModelTableName.Substring(0, 1).ToLower() + ModelTableName.Substring(1);
            result.AppendLine("\t\tpublic static " + ModelTableName + "Db ToCommon(this " + ModelTableName + " " + paramName + ", IEncryptor encryptor)");
            AppendConversionBody(result);
            return result.ToString();
        }

        private void PutRepositoryUsing(StringBuilder str)
        {
            str.AppendLine("using System;");
            str.AppendLine("using System.Collections.Generic;");
            str.AppendLine("using System.Linq;");
            if (IsLogging())
                str.AppendLine("using CommonClasses.Helpers;");
            str.AppendLine("using " + AppConfiguration.DbClassesNamespace + ";");
            if (!string.IsNullOrEmpty(AppConfiguration.DbModelNamespace))
                str.AppendLine("using " + AppConfiguration.DbModelNamespace + ";");
            if (AppConfiguration.UseInterfacesForDbClasses)
                str.AppendLine("using " + AppConfiguration.DbInterfacesNamespace + ";");
        }

        private void PutSaveMethod(StringBuilder str)
        {
            string paramName = ModelTableName.Substring(0, 1).ToLower() + ModelTableName.Substring(1) + "Db";
            ColumnInfo primaryKey = GetPrimaryKey();
            if (primaryKey == null)
            {
                str.AppendLine("There is no primary key in this table");
                return;
            }
            str.AppendLine("\t\tpublic int Save" + ModelTableName + "(" + InterfacePreffix + ModelTableName + "Db " + paramName + ", int? transactionNumber = null)");
            str.AppendLine("\t\t{");
            if (Columns.Any(r => r.ColumnName == AppConfiguration.FilteringField))
            {
                str.AppendLine("\t\t\tif (" + paramName + "." + AppConfiguration.FilteringField + " != _" +
                               AppConfiguration.FilteringField.FirstToLower() + ")");
                str.AppendLine("\t\t\t\tthrow new Exception(\"Attempt to save " + ModelTableName + " with wrong " +
                               AppConfiguration.FilteringField + "\");");
                str.AppendLine();
            }
            str.AppendLine("\t\t\t" + ModelTableName + " record;");
            if (IsLogging())
            {
                str.AppendLine("\t\t\tvar recordOld = new " + ModelTableName + "();");
            }
            str.AppendLine("\t\t\tif (" + paramName + "." + primaryKey.ColumnName + " == 0)");
            str.AppendLine("\t\t\t{");
            str.AppendLine("\t\t\t\trecord = new " + ModelTableName + "();");
            str.AppendLine("\t\t\t\t" + AppConfiguration.DbContextName + ".AddTo" + DatabaseTableName + "(record);");
            str.AppendLine("\t\t\t}");
            str.AppendLine("\t\t\telse");
            str.AppendLine("\t\t\t{");
            str.AppendLine("\t\t\t\trecord = " + AppConfiguration.DbContextName + "." + DatabaseTableName +
                                       ".Where(r => r." + primaryKey.ColumnName + " == " +
                                       paramName + "." + primaryKey.ColumnName + ").First();"); 
            if (IsLogging())
            {
                str.AppendLine("\t\t\t\tReflectionHelper.CopyAllProperties(record, recordOld);");
            }
            
            str.AppendLine("\t\t\t}");
            str.AppendLine();
            foreach (ColumnInfo columnInfo in Columns)
            {
                if (columnInfo == primaryKey) continue;
                str.Append("\t\t\trecord." + columnInfo.ColumnName + " = ");
                if (columnInfo.ColumnName == AppConfiguration.FilteringField)
                {
                    str.AppendLine("_" + columnInfo.ColumnName.FirstToLower() + ";");
                }
                else
                {
                    str.AppendLine(columnInfo.Encoded ? 
                        "_authInfo.EncryptDecimal(" + paramName + "." + columnInfo.ColumnName + ");" :
                        paramName + "." + columnInfo.ColumnName + ";");
                }
            }
            str.AppendLine();
            str.AppendLine("\t\t\t" + AppConfiguration.DbContextName + ".SaveChanges();");
            str.AppendLine("\t\t\tif (" + paramName + "." + primaryKey.ColumnName + " == 0)");
            str.AppendLine("\t\t\t{");
            str.AppendLine("\t\t\t\t" + paramName + "." + primaryKey.ColumnName + " = " +
                           "record." + primaryKey.ColumnName + ";");
            if (IsLogging())
            {
                str.AppendLine("\t\t\t\t" + GetLogMethod() + "(UserId, \"" + DatabaseTableName + "\", record." +
                               primaryKey.ColumnName + ", \"I\", XmlHelper.GetObjectXml(record), transactionNumber);");
            }
            str.AppendLine("\t\t\t}");
            if (IsLogging())
            {
                str.AppendLine("\t\t\telse");
                str.AppendLine("\t\t\t{");
                str.AppendLine("\t\t\t\t" + GetLogMethod() + "(UserId, \"" + DatabaseTableName + "\", record." +
                               primaryKey.ColumnName + ", \"U\", XmlHelper.GetDifferenceXml(recordOld, record), transactionNumber);");
                str.AppendLine("\t\t\t}");
            }
            str.AppendLine();
            
            str.AppendLine("\t\t\treturn " + paramName + "." + primaryKey.ColumnName + ";");
            str.AppendLine("\t\t}");
            str.AppendLine();
        }

        private bool ContainsFiltreringColumn()
        {
            return Columns.Any(r => r.ColumnName == AppConfiguration.FilteringField);
        }

        private void PutGetByIdMethod(StringBuilder str)
        {
            str.AppendLine("\t\tpublic " + ModelTableName +
                           "Db Get" + ModelTableName + "ById(int id)");
            str.AppendLine("\t\t{");
            str.AppendLine("\t\t\treturn (from r in " + AppConfiguration.DbContextName + "." + DatabaseTableName);
            str.AppendLine("\t\t\t\twhere r." + GetPrimaryKey().ColumnName + " == id");
            str.AppendLine("\t\t\t\tselect r).AsEnumerable()");
            str.AppendLine("\t\t\t\t.Select(r => r.ToCommon(_authInfo)).FirstOrDefault();");
            str.AppendLine("\t\t}");
            str.AppendLine();
        }

        private void PutGetAllMethod(StringBuilder str)
        {
            str.AppendLine("\t\tpublic List<" + ModelTableName + "Db> " +
                           "GetAll" + DatabaseTableName + "()");
            str.AppendLine("\t\t{");
            str.AppendLine("\t\t\treturn (from r in " + AppConfiguration.DbContextName + "." + DatabaseTableName);
            str.AppendLine("\t\t\t\tselect r).AsEnumerable()");
            str.AppendLine("\t\t\t\t.Select(r => r.ToCommon(_authInfo)).ToList();");
            str.AppendLine("\t\t}");
            str.AppendLine();
        }

        private void PutDeleteMethod(StringBuilder str)
        {
            str.Append("\t\tpublic void Delete" + ModelTableName + "(int id, string reason = null, int? transactionNumber = null)");
            str.AppendLine("\t\t{");
            str.AppendLine("\t\t\tvar record = " + AppConfiguration.DbContextName + "." + DatabaseTableName +
                           ".First(r => r." + GetPrimaryKey().ColumnName + " == id);");
            if (IsLogging())
            {
                str.AppendLine("\t\t\t\t" + GetLogMethod() + "(UserId, \"" + DatabaseTableName + "\", record." +
                               GetPrimaryKey().ColumnName + ", \"D\", XmlHelper.GetObjectXml(record, reason), transactionNumber);");
            }
            str.AppendLine("\t\t\t" + AppConfiguration.DbContextName + ".DeleteObject(record);");
            str.AppendLine("\t\t\t" + AppConfiguration.DbContextName + ".SaveChanges();");
            str.AppendLine("\t\t}");
            str.AppendLine();
        }

        private void PutRepositoryConstructors(StringBuilder str)
        {
            str.AppendLine("\t\tpublic " + ModelTableName + "Repository(int " + AppConfiguration.FilteringField.FirstToLower() + ")" +
                ":base(" + AppConfiguration.FilteringField.FirstToLower() + ") {}");
            str.AppendLine();
            str.AppendLine("\t\tpublic " + ModelTableName + "Repository(FilteredContext context): base(context) {}");
            str.AppendLine();
        }

        public string GetRepositoryText()
        {
            var result = new StringBuilder();
            result.AddHeaderWarning();
            PutRepositoryUsing(result);
            result.AppendLine();
            result.AppendLine("namespace " + AppConfiguration.DbRepositoryNamespace);
            result.AppendLine("{");

            if (AppConfiguration.DbRepositoryIsPartialClass)
            {
                result.AppendLine("\tpublic partial class DbRepository");
            }
            else
            {
                result.AppendLine("\tpublic class " + ModelTableName + "Repository: DbRepository");
            }

            result.AppendLine("\t{");
            if (!AppConfiguration.DbRepositoryIsPartialClass) PutRepositoryConstructors(result);
            PutSaveMethod(result);
            PutGetByIdMethod(result);
            PutGetAllMethod(result);
            PutDeleteMethod(result);
            result.AppendLine("\t}");
            result.AppendLine("}");
            return result.ToString();
        }

        public string GetServiceAutoMethods()
        {
            var result = new StringBuilder();
            result.AppendLine("\t\t#region " + ModelTableName + " Methods");
            result.AppendLine();

            string paramName = ModelTableName.Substring(0, 1).ToLower() + ModelTableName.Substring(1) + "Db";
            string repositoryName = (AppConfiguration.DbRepositoryIsPartialClass ? "Db" : ModelTableName) + "Repository";
            result.AppendLine("\t\tpublic int Save" + ModelTableName + "(" + InterfacePreffix + ModelTableName + "Db " + paramName + ")");
            result.AppendLine("\t\t{");
            result.AppendLine("\t\t\tDelay();");
            result.AppendLine("\t\t\tusing (var db = new " + repositoryName + "(" + AppConfiguration.FilteringField + "))");
            result.AppendLine("\t\t\t{");
            result.AppendLine("\t\t\t\treturn db.Save" + ModelTableName + "(" + paramName + 
                (AppConfiguration.DatabaseLogging ? ", UserId);" : ");"));
            result.AppendLine("\t\t\t}");
            result.AppendLine("\t\t}");
            result.AppendLine();

            result.AppendLine("\t\tpublic " + ModelTableName + "Db Get" + ModelTableName + "ById(int id)");
            result.AppendLine("\t\t{");
            result.AppendLine("\t\t\tDelay();");
            result.AppendLine("\t\t\tusing (var db = new " + repositoryName + "(" + AppConfiguration.FilteringField + "))");
            result.AppendLine("\t\t\t{");
            result.AppendLine("\t\t\t\treturn db.Get" + ModelTableName + "ById(id);");
            result.AppendLine("\t\t\t}");
            result.AppendLine("\t\t}");
            result.AppendLine();

            result.AppendLine("\t\tpublic IList<" + ModelTableName + "Db> GetAll" + DatabaseTableName + "()");
            result.AppendLine("\t\t{");
            result.AppendLine("\t\t\tDelay();");
            result.AppendLine("\t\t\tusing (var db = new " + repositoryName + "(" + AppConfiguration.FilteringField + "))");
            result.AppendLine("\t\t\t{");
            result.AppendLine("\t\t\t\treturn db.GetAll" + DatabaseTableName + "();");
            result.AppendLine("\t\t\t}");
            result.AppendLine("\t\t}");
            result.AppendLine();

            result.AppendLine("\t\tpublic void Delete" + ModelTableName + "(int id)");
            result.AppendLine("\t\t{");
            result.AppendLine("\t\t\tDelay();");
            result.AppendLine("\t\t\tusing (var db = new " + repositoryName + "(" + AppConfiguration.FilteringField + "))");
            result.AppendLine("\t\t\t{");
            result.AppendLine("\t\t\t\tdb.Delete" + ModelTableName + "(id" + 
                (AppConfiguration.DatabaseLogging ? ", UserId);" : ");"));
            result.AppendLine("\t\t\t}");
            result.AppendLine("\t\t}");
            result.AppendLine();

            result.AppendLine("\t\t#endregion");
            
            return result.ToString();
        }

        public string GetFilteredContextTableMethod(string field, string privateField)
        {
            var result = new StringBuilder();
            result.AppendLine("\t\tpublic IQueryable<" + ModelTableName + "> " + DatabaseTableName);
            result.AppendLine("\t\t{");
            result.Append("\t\t\tget { return _context." + DatabaseTableName);
            if (Columns.Any(r => r.ColumnName.Equals(AppConfiguration.FilteringField)))
                result.Append(".Where(r => r." + field + " == " + privateField + ")");
            result.AppendLine("; }");
            result.AppendLine("\t\t}");
            return result.ToString();
        }

        public string GetFilteredContextAddToTableMethod(string field, string privateField)
        {
            var result = new StringBuilder();
            result.AppendLine("\t\tpublic void AddTo" + DatabaseTableName + "(" + ModelTableName + " record)");
            result.AppendLine("\t\t{");
            result.AppendLine("\t\t\t_context.AddTo" + DatabaseTableName + "(record);");
            result.AppendLine("\t\t}");
            return result.ToString();
        }
    }
}
