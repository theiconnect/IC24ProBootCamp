using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FileModel;
using Enum;
using Configuration;

namespace ProjectHelpers
{
    public class FileHelper : ConfigHelper
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
            try
            {
                return File.ReadAllLines(filePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static DataSet GetExcelFileContent(string filePath, params string[] sheets)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static DataSet GetXMLFileContent(string returnFilePath)
        {
            try
            {
                DataSet dataSet = new DataSet();
                // Read the XML file into the DataSet

                dataSet.ReadXml(returnFilePath);

                return dataSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static void MoiveFile(string FilePath, FileStatus failure)
        {
            string combinedName=string.Empty;
            string desinationPath=string.Empty;
            switch (failure) 
            {
                case FileStatus.Success:
                    combinedName = "Processed";
                    break;
                case FileStatus.Failure:
                    combinedName = "ErrorFile";
                    break;
                default:
                    combinedName=string.Empty ;
                    break;

            }
            desinationPath=Path.Combine(Path.GetDirectoryName(FilePath),combinedName, GetFileNameByFilePath(FilePath));
            File.Move(FilePath, desinationPath); 



        }


        public static string GetFileNameByFileType(string folder, FileTypes filetype)
        {
            string[] wareHouseLevelFile = Directory.GetFiles(folder);
            string StartsWith = string.Empty;

            switch (filetype)
            {
                case FileTypes.wareHouse:
                    StartsWith = "warehouse"; break;
                case FileTypes.employee:
                    StartsWith = "employee"; break;
                case FileTypes.inventory:
                    StartsWith = "inventory"; break;
                case FileTypes.customers:
                    StartsWith = "customers"; break;
                case FileTypes.orders:
                    StartsWith = "orders"; break;
                case FileTypes.orderitem:
                    StartsWith = "orderitem"; break;
                case FileTypes.returns:
                    StartsWith = "returns"; break;


            }

            foreach (string file in wareHouseLevelFile)
            {
                if (Path.GetFileNameWithoutExtension(file).ToLower().Trim().StartsWith(StartsWith))
                {
                    return file;
                }
            }
            return null;
        }


        public static void LogEntries(string log)
        {
            string filepath = Path.Combine(RootFolderPath, "EcommerceDataLog.txt");
            File.AppendAllText(filepath, log);
            Console.WriteLine();
        }
    }
}

