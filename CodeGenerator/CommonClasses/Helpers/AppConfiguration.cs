using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonClasses.Helpers
{
    public static class AppConfiguration
    {
        public static string DbClassesNamespace
        {
            get { return ConfigurationManager.AppSettings["DbClassesNamespace"]; }
        }

        public static string DbRepositoryNamespace
        {
            get { return ConfigurationManager.AppSettings["DbRepositoryNamespace"]; }
        }

        public static string DbModelNamespace
        {
            get { return ConfigurationManager.AppSettings["DbModelNamespace"]; }
        }

        public static string ImplicitNamespace
        {
            get { return ConfigurationManager.AppSettings["ImplicitNamespace"]; }
        }

        public static string ServiceProxyNamespace
        {
            get { return ConfigurationManager.AppSettings["ServiceProxyNamespace"]; }
        }

        public static string DbContextName
        {
            get { return ConfigurationManager.AppSettings["DbContextName"]; }
        }

        public static bool DbRepositoryIsPartialClass
        {
            get { return ConfigurationManager.AppSettings["DbRepositoryIsPartialClass"] == "true"; }
        }

        public static bool UseInterfacesForDbClasses
        {
            get { return ConfigurationManager.AppSettings["UseInterfacesForDbClasses"] == "true"; }
        }

        public static string DbInterfacesNamespace
        {
            get { return ConfigurationManager.AppSettings["DbInterfacesNamespace"]; }
        }

        public static string FilteredContextNamespace
        {
            get { return ConfigurationManager.AppSettings["FilteredContextNamespace"]; }
        }

        public static string FilteringField
        {
            get { return ConfigurationManager.AppSettings["FilteringField"]; }
        }

        public static string DataContextName
        {
            get { return ConfigurationManager.AppSettings["DataContextName"]; }
        }

        public static bool DatabaseLogging
        {
            get { return ConfigurationManager.AppSettings["DatabaseLogging"] == "true"; }
        }

        public static string NoLogTables
        {
            get { return ConfigurationManager.AppSettings["NoLogTables"]; }
        }

        public static string ProjectDir
        {
            get { return ConfigurationManager.AppSettings["ProjectDir"]; }
        }

        public static string CommonClassesDir
        {
            get { return ConfigurationManager.AppSettings["CommonClassesDir"]; }
        }

        public static string DbClassesDir
        {
            get { return ConfigurationManager.AppSettings["DbClassesDir"]; }
        }

        public static string CommonClassesProjectFileName
        {
            get { return Path.Combine(ProjectDir, CommonClassesDir, ConfigurationManager.AppSettings["CommonClassesProjectFileName"]); }
        }

        public static string InterfacesDir
        {
            get { return ConfigurationManager.AppSettings["InterfacesDir"]; }
        }

        public static string DbInterfacesDir
        {
            get { return ConfigurationManager.AppSettings["DbInterfacesDir"]; }
        }

        public static string InterfacesProjectFileName
        {
            get { return Path.Combine(ProjectDir, InterfacesDir, ConfigurationManager.AppSettings["InterfacesProjectFileName"]); }
        }

        public static string DbRepositoryDir
        {
            get { return ConfigurationManager.AppSettings["DbRepositoryDir"]; }
        }

        public static string RepositoriesDir
        {
            get { return ConfigurationManager.AppSettings["RepositoriesDir"]; }
        }

        public static string DbRepositoryProjectFileName
        {
            get { return Path.Combine(ProjectDir, DbRepositoryDir, ConfigurationManager.AppSettings["DbRepositoryProjectFileName"]); }
        }

        public static string ConversionsFileName
        {
            get { return Path.Combine(ProjectDir, DbRepositoryDir, ConfigurationManager.AppSettings["ConversionsFileName"]); }
        }

        public static string ImplicitsFileName
        {
            get { return Path.Combine(ProjectDir, DbRepositoryDir, ConfigurationManager.AppSettings["ImplicitsFileName"]); }
        }

        public static string FilteredContextAutoMethodsFileName
        {
            get { return Path.Combine(ProjectDir, DbRepositoryDir, ConfigurationManager.AppSettings["FilteredContextAutoMethodsFileName"]); }
        }
    }
}
