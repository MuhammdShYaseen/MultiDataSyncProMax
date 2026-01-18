using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Configuration.ProfilesModels
{
    public class DestinationConfig
    {
        public string Endpoint { get; set; } = string.Empty;
        public string PayloadTemplate { get; set; } = string.Empty;

        //public int PageSize { get; set; }
    }
}
