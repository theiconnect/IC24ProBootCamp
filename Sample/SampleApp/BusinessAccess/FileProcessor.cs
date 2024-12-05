using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Interfaces
{
    
    internal abstract class FileProcessor : IFileProcessor
    {
        protected FileProcessor(string filePath)
        {
            this.filePath = filePath;

            ProcessTheFile();
        }

        protected string filePath { get; set; }
        protected FileStatuses FileStatus { get; set; }
        protected string DirectoryPath { get { return Path.GetDirectoryName(filePath); } }
        protected string fileName { get { return Path.GetFileName(filePath); } }

        #region ReadFiles
        public virtual string[] ReadDelimetedFileContent(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
        public virtual DataTable ReadExcelOrCSVFileToDatatable(string filePath)
        {
            DataSet ds = ReadExcelOrCSVFileToDataset(filePath);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        private string GetOLEDBConnectionString()
        {
            string con = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='text;HDR=Yes;FMT=Delimited(,)'", this.DirectoryPath);
            return con;
        }

        public virtual DataSet ReadExcelOrCSVFileToDataset(string filePath)
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(GetOLEDBConnectionString()))
                {
                    using (OleDbDataAdapter da = new OleDbDataAdapter($"select * from [{fileName}]", con))
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            da.Fill(dataSet);
                            return dataSet;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"ReadExcelOrCSVFileToDataset-Unable to read file;fileName:{fileName}");
                throw;
            }
        }
        #endregion 

        protected virtual void ProcessTheFile()
        {
            ReadFileContent();
            ValidateFileContent();
            ProcessFileContentToDB();
            MoveFile();
        }
        protected abstract void ReadFileContent();
        protected abstract bool ValidateFileContent();
        protected abstract void ProcessFileContentToDB();
        protected void MoveFile()
        {
            string destFolder = string.Empty;
            switch (FileStatus)
            {
                case FileStatuses.Success:destFolder = "Processed"; break;
                case FileStatuses.Failed: destFolder = "Error"; break;
                case FileStatuses.Archive: destFolder = "Archive"; break;
            }
        }

        void IFileProcessor.MoveFile()
        {
            throw new NotImplementedException();
        }
    }
}
