using System;
using System.Collections.Generic;
using System.Configuration;
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
    }

}
