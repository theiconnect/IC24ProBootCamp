﻿using FileModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS_IDAL
{
    public interface IReturnsDAL
    {
        bool PushReturnsDataToDB(List<ReturnsModel> returnsList, string returnFilePath);
    }
}
