using Staffinfo.Divers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinfo.Divers.Data.Services.Contracts
{
    public interface IReportDataService
    {
        Task<List<DiversReportData>> GetReportDatas();
    }
}
