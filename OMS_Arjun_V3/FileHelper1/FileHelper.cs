using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using OMS_Arjun_V3;
using ConnectionConfig;
using FileTypes;

namespace FileHelper
{
    public class FileHelper:ConnectionConfig.ConnectionConfig1
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
        public static void MoveFile(string FilePath,FileTypes.FileTypes1 failure)
        {
            string combinedName = string.Empty;
            string desinationPath = string.Empty;
            switch (failure)
            {
                case FileTypes.FileTypes1.Success:
                    combinedName = "Processed";
                    break;
                case FileTypes.FileTypes1.Failure:
                    combinedName = "ErrorFile";
                    break;
                case FileTypes.FileTypes1.Archieve:
                    combinedName = "Archieve";
                    break;

            }

            desinationPath = Path.Combine(Path.GetDirectoryName(FilePath), combinedName, GetFileNameByFilePath(FilePath));
            File.Move(FilePath, desinationPath);
        }
    }
}
