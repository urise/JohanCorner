using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonClasses.Helpers
{
    public static class FileHelper
    {
        public static string GetUniqueName(string name)
        {
            int i = 1;
            string result = name;
            while (Directory.Exists(result) || File.Exists(result))
                result = name + " (" + i++ + ")";
            return result;
        }

        public static string CreateDateNameFolder(string path)
        {
            string folderName = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            if (Directory.Exists(folderName))
                folderName = FileHelper.GetUniqueName(folderName);
            Directory.CreateDirectory(folderName);
            return folderName;
        }

        static public string GetFolderName(string fullName)
        {
            string delimiter = fullName.Contains(@"\") ? @"\" : @"/";
            int n = fullName.LastIndexOf(delimiter);
            return fullName.Substring(0, n);
        }

        static public string GetFileName(string fullName)
        {
            string delimiter = fullName.Contains(@"\") ? @"\" : @"/";
            int n = fullName.LastIndexOf(delimiter);
            return fullName.Substring(n + 1);
        }
    }
}
