using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_Configurations
{
    public enum FileStatus
    {
        Failure,
        Success
    }
    public class FileHelper:AppConfiguration
    {
        public static void MoveFile(string storeFilePath, FileStatus Success)
        {
            string combinedName = string.Empty;
            string desinationPath = string.Empty;
            switch (Success)
            {
                case FileStatus.Success:
                    combinedName = "Processed";
                    break;
                case FileStatus.Failure:
                    combinedName = "Archive";
                    break;
                    default:
                    combinedName = string.Empty;
                    break;
                    
            }
            desinationPath = Path.Combine(Path.GetDirectoryName(storeFilePath), combinedName, Path.GetFileName(storeFilePath));
            File.Move(storeFilePath, desinationPath);
        }
    }
}
