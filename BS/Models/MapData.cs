using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Models
{
    public class MapData
    {

        public Version _version { get; set; }
        public List<Event> _events { get; set; }
        public List<Note> _notes { get; set; }
        public List<Obstacle> _obstacles { get; set; }
    }
}

