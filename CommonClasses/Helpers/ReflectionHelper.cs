#region Directievs

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CommonClasses.DbClasses;
using CommonClasses.InfoClasses;

#endregion

namespace CommonClasses.Helpers
{
    /// <summary>
    /// Helper that provides data for controls on main page
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// SetUp value for property
        /// </summary>
        /// <param name="obj">control</param>
        /// <param name="properties">collection of data</param>
        public static void SetProperties(object obj, Dictionary<string, string> properties)
        {
            foreach (KeyValuePair<string, string> pair in properties)
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(pair.Key);
                if (propertyInfo != null)
                {
                    var type = propertyInfo.PropertyType;
                    object safeValue;
                    if (IsNullable(type))
                        safeValue = string.IsNullOrEmpty(pair.Value) ? null : ConvertToType(pair.Value, type.GetGenericArguments()[0]);
                    else
                        safeValue = ConvertToType(pair.Value, type);
                    propertyInfo.SetValue(obj, safeValue, null);
                }
            }
        }

        public static void CopyAllProperties(object src, object dest)
        {
            CopyAllProperties(src, dest, new List<string>());
        }

        public static void CopyAllProperties(object src, object dest, List<String> notCopyList)
        {
            if (src.GetType() != dest.GetType())
                throw new Exception("src and dest should have the same type");

            PropertyInfo[] propertyInfos;
            propertyInfos = src.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (!propertyInfo.PropertyType.IsValueType && propertyInfo.PropertyType != typeof(string)) continue;
                if (notCopyList.Contains(propertyInfo.Name)) continue;
                try
                {
                    propertyInfo.SetValue(dest, propertyInfo.GetValue(src, null), null);    
                }
                catch(Exception ex) 
                {
                    
                }
            }
        }

        public static bool PropertiesAreEqual(object obj1, object obj2, PropertyInfo propertyInfo)
        {
            object value1 = propertyInfo.GetValue(obj1, null);
            object value2 = propertyInfo.GetValue(obj2, null);
            return ValuesAreEqual(value1, value2, propertyInfo.PropertyType.FullName);
        }

        public static bool ValuesAreEqual(object value1, object value2, string typeFullName)
        {
            if (typeFullName == "System.String")
            {
                return (String.IsNullOrEmpty((string)value1) && String.IsNullOrEmpty((string)value2) ||
                        String.Compare((string)value1, (string)value2) == 0);
            }

            if (value1 == null)
                return value2 == null;
            
            return value1.Equals(value2);
        }

        public static bool CompareAllProperties(object src, object dest)
        {
            if (src.GetType() != dest.GetType())
                throw new Exception("src and dest should have the same type");

            PropertyInfo[] propertyInfos;
            propertyInfos = src.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (!PropertiesAreEqual(src, dest, propertyInfo))
                    return false;
            }
            return true;
        }

        public static bool IsNullable(Type type)
        {
            return type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static object ConvertToType(object value, Type type)
        {
            if (type == typeof(bool))
            {
                value = value.Equals("1");
            }
            return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

    }
}
