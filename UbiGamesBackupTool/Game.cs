using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UbiGamesBackupTool
{
    public class Game
    {
        public string id { get; set; }
        public string name { get; set; }
        public string img { get; set; }
        List<string> backuptime { get; set; }
    }
}
