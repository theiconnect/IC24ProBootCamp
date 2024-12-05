using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Interfaces
{
    internal interface IFileProcessor
    {
        string[] ReadDelimetedFileContent(string filePath);
        DataTable ReadExcelOrCSVFileToDatatable(string filePath);
        DataSet ReadExcelOrCSVFileToDataset(string filePath);
        void MoveFile();
    }
}
