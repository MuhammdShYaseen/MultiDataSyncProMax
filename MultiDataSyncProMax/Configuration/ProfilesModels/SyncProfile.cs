using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Configuration.ProfilesModels
{
    public class SyncProfile
    {
        public SourceConfig Source { get; set; } = new();
        public DestinationConfig Destination { get; set; } = new();


    }
}
