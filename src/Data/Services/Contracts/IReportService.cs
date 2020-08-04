using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Services.Contracts
{
    public interface IReportService
    {
        Task<byte[]> GenerateExcelReport();
    }
}
