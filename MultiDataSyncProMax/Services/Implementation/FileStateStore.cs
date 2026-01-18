using MultiDataSyncProMax.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiDataSyncProMax.Services.Implementation
{
    public class FileStateStore : IStateStore
    {
        private HashSet<string> _ids = new();
        private const string FileName = "state.txt";

        public FileStateStore()
        {
            if (File.Exists(FileName))
                _ids = File.ReadAllLines(FileName).ToHashSet();
        }

        public bool IsProcessed(string id) => _ids.Contains(id);

        public void MarkProcessed(string id)
        {
            if (_ids.Add(id))
                File.AppendAllLines(FileName, new[] { id });
        }
    }
}
