using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Interfaces
{
    public interface IDataSender
    {
        Task SendAsync(string endpoint, object payload);
    }
}
