using SampleApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.BusinessAccess
{
    internal class StockProcessor : FileProcessor
    {
        public StockProcessor(string filePath) : base(filePath)
        {

        }

        protected override void ProcessFileContentToDB()
        {
            throw new NotImplementedException();
        }

        protected override void ReadFileContent()
        {
            throw new NotImplementedException();
        }

        protected override bool ValidateFileContent()
        {
            throw new NotImplementedException();
        }
    }
}
