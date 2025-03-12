using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace BusinessAccessLayer
{
    public class FileHelper
    {
        public static string GetFileNameByFileType(string storeDirectoryPath, FileTypes fileType)
        {
            //get all files from store directory
            string[] storeLevelFiles = Directory.GetFiles(storeDirectoryPath);
            string startWithValue = string.Empty;
            switch (fileType)
            {
                case FileTypes.Stores:
                    startWithValue = "stores_"; break;

                case FileTypes.Stock:
                    startWithValue = "stock_"; break;

                case FileTypes.Employee:
                    startWithValue = "employee_"; break;

                case FileTypes.Customer:
                    startWithValue = "customer_"; break;
                default:
                    startWithValue=string.Empty; break;



            }
            foreach (string file in storeLevelFiles)
            {
                if (Path.GetFileNameWithoutExtension(file).Trim().ToLower().StartsWith(startWithValue))
                {
                    return file;
                }
            }
            return null;
        }

        public static void Move(string storeDirectoryPath, FileStatus status)
        {
            string file = string.Empty;
            string destPath = string.Empty;
            switch (status)
            {
                case FileStatus.Sucess:
                    file = "Processed";break;
                case FileStatus.Failure:
                    file = "Archieve";break;
                default:
                    file=string.Empty;
                    break;

                  

            }

            string sourcePath = storeDirectoryPath;
            destPath = Path.Combine(Path.GetDirectoryName(storeDirectoryPath), "Processed", Path.GetFileName(storeDirectoryPath));
            File.Move(sourcePath, destPath);

        }
    }
}
