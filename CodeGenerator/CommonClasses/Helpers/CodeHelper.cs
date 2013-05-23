﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommonClasses.Helpers
{
    public static class CodeHelper
    {
        #region Getting Pieces Of Code Methods

        public static List<string> GetInterfaceSignatureForMethod(IMethodData method)
        {
            return new List<string>{ method.FullSignature + ";" };
        }

        public static List<string> GetEmptyMethod(IMethodData method)
        {
            return new List<string>
                       {
                           String.Empty,
                           "public " + method.FullSignature,
                           "{",
                           "    " + GetValidReturnStatement(method.ReturnType),
                           "}"
                       };
        }

        public static List<string> GetMockMethod(IMethodData method, string className)
        {
            return new List<string>
                       {
                           String.Empty,
                           "public " + method.FullSignature,
                           "{",
                           "    return (" + method.ReturnType + ")MockResultSingleton.GetResult(\"" + className + "." + method.MethodName + "\");",
                           "}"
                       };
        }

        public static string GetValidReturnStatement(string type)
        {
            switch (type)
            {
                case "void": 
                    return "return;";
                case "int":
                    return "return 0;";
                default:
                    return "return null;";
            }
        }

        public static string GetSpringConfigLine(string id, string assemblyName, string className, bool singleton)
        {
            string result = "<object id=\"" + id + "\" type=\"" + assemblyName + "." + className + ", " + assemblyName + "\"";
            if (!singleton) result += " singleton=\"false\"";
            return result + "/>";
        }

        public static List<string> GetEmptyInterface(string interfaceName, string ancestorList)
        {
            return new List<string>
                       {
                           "public interface " + interfaceName + ": " + ancestorList,
                           "{",
                           "}"
                       };
        }

        public static List<string> AddNamespace(List<string> code, string namespaceName)
        {
            var result = new List<string>()
                             {
                                 "namespace " + namespaceName,
                                 "{",
                                 "}"
                             };
            result.InsertRange(2, code.IndentAllLines(4));
            return result;
        }

        public static List<string> IndentAllLines(this IEnumerable<string> list, int numberOfSpaces)
        {
            return list.Select(r => String.Empty.PadLeft(numberOfSpaces, ' ') + r).ToList();
        }

        public static List<string> IndentAllLines(this IEnumerable<string> list, string spaces)
        {
            return list.Select(r => spaces + r).ToList();
        }

        #endregion

        #region Finding Places In Code Methods

        public static PlaceInFile FindPlaceForMethod(List<string> classCode)
        {
            int counter = 0;
            for (int index = classCode.Count - 1; index >= 0; index--)
            {
                if (classCode[index].Trim().Equals("}"))
                    counter++;
                if (counter == 2)
                {
                    int n = classCode[index].IndexOf("}");
                    return new PlaceInFile(index, 
                        "    " + classCode[index].Substring(0, n));
                }
            }
            return null;
        }

        public static PlaceInFile FindLastLineThatContainText(List<string> lines, string text)
        {
            for (int index = lines.Count - 1; index >= 0; index--)
            {
                if (lines[index].Contains(text))
                {
                    int n = lines[index].Length - lines[index].TrimStart().Length;
                    return new PlaceInFile(index, lines[index].Substring(0, n));
                }
            }
            return null;
        }

        public static PlaceInFile FindPlaceForIncludeInProject(List<string> projectCode)
        {
            return FindLastLineThatContainText(projectCode, @"<Compile Include=");
        }

        #endregion

        #region Change Files Methods

        public static void IncludeInProject(string projectFileName, string fileToInclude)
        {
            List<string> projectCode = File.ReadAllLines(projectFileName).ToList();
            PlaceInFile place = FindPlaceForIncludeInProject(projectCode);
            if (place == null)
                throw new Exception("Cannot find \"<Compile Include\" section in the " + projectFileName);
            string includeLine = "<Compile Include=\"" + fileToInclude + "\" />";
            projectCode.Insert(place.LineNumber + 1, place.Spaces + includeLine);
            File.WriteAllLines(projectFileName, projectCode);
        }

        public static void IncludeLineInSpringConfig(string configFileName, string textToFind, string lineToInclude)
        {
            List<string> configCode = File.ReadAllLines(configFileName).ToList();
            PlaceInFile place = FindLastLineThatContainText(configCode, textToFind);
            if (place == null)
                throw new Exception("Cannot find place for adding line in " + configFileName);
            configCode.Insert(place.LineNumber + 1, place.Spaces + lineToInclude);
            File.WriteAllLines(configFileName, configCode);
        }

        public static void AddMethodToClass(string classFileName, IEnumerable<string> methodCode)
        {
            List<string> classCode = File.ReadAllLines(classFileName).ToList();
            PlaceInFile place = FindPlaceForMethod(classCode);
            if (place == null)
                throw new Exception("Cannot find place for adding new method in " + classFileName);
            var indentedMethodCode = methodCode.IndentAllLines(place.Spaces);
            classCode.InsertRange(place.LineNumber, indentedMethodCode);
            File.WriteAllLines(classFileName, classCode);
        }

        #endregion

        #region Getting Code From File Patterns

        private static string GetCodePatternFileName(string patternName)
        {
            return @"CodePatterns\" + patternName + ".ptrn";
        }

        private static List<string> GetDbRepositoryCodeFromPattern(string patternName, string dbRepositoryName)
        {
            List<string> code = File.ReadAllLines(GetCodePatternFileName(patternName)).ToList();
            return code.Select(r => r.Replace(@"%DB_REPOSITORY_NAME%", dbRepositoryName)).ToList();
        }

        public static List<string> GetDbRepositoryInterface(string dbRepositoryName)
        {
            return GetDbRepositoryCodeFromPattern("DbRepositoryInterface", dbRepositoryName);
        }

        public static List<string> GetDbRepository(string dbRepositoryName)
        {
            return GetDbRepositoryCodeFromPattern("DbRepository", dbRepositoryName);
        }

        public static List<string> GetMockDbRepository(string dbRepositoryName)
        {
            return GetDbRepositoryCodeFromPattern("MockDbRepository", dbRepositoryName);
        }

        public static List<string> GetDbRepositoryFactoryMethod(string dbRepositoryName)
        {
            return GetDbRepositoryCodeFromPattern("DbRepositoryFactoryMethod", dbRepositoryName);
        }

        public static List<string> GetCommonUsing()
        {
            return File.ReadAllLines(GetCodePatternFileName("CommonUsing")).ToList();
        }

        #endregion

        #region Adding Files

        public static void AddOrChangeFile(string projectFolder, string partialFileName, 
            string content, string projectFileName)
        {
            string fullFileName = Path.Combine(AppConfiguration.ProjectDir, projectFolder, partialFileName);
            if (!File.Exists(fullFileName))
                IncludeInProject(projectFileName, partialFileName);
            File.WriteAllText(fullFileName, content);
        }

        #endregion
    }
}
