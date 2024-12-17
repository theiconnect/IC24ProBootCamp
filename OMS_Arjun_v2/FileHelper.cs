using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_Arjun_v2
{
    internal class FileHelper:ConConfigHelper
    {
        public static string GetFileNameByFilePath(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static string GetFileNameWithoutExtensionByFilePath(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static string GetDirectoryNameByFilePath(string filePath)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            return Path.GetFileName(dirPath);
        }

        public static string GetDirectoryNameByDirectoryPath(string dirPath)
        {
            return Path.GetFileName(dirPath);
        }

        public static string[] GetAllDirectoriesByDirectoryPath(string dirPath)
        {
            return Directory.GetDirectories(dirPath);
        }

        public static string[] GetAllFilesByDirectoryPath(string dirPath)
        {
            return Directory.GetFiles(dirPath);
        }

        public static string[] GetAllLinesOfFlatFileByPath(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public static DataSet GetExcelFileContent(string filePath, params string[] sheets)
        {
            DataSet ds = new DataSet();
            string connStr = string.Format(ExcelConnString, filePath);
            using (OleDbConnection oleDbConnection = new OleDbConnection(connStr))
            {
                oleDbConnection.Open();
                foreach (var sheet in sheets)
                {
                    using (OleDbDataAdapter da = new OleDbDataAdapter($"select * from [{sheet}$]", oleDbConnection))
                    {
                        da.Fill(ds);

                    }
                }
            }
            return ds;
        }

    }
}
