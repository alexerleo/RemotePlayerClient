using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Player.Database;

namespace Player.Models
{
    public class SelectionModel
    {
        public Track track { get; set; }
        public Album album { get; set; }
        public Band band { get; set; }
        public Device device { get; set; }
    }
}
