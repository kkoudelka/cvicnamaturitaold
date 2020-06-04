using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Models
{
    public class Note
    {
        public double _time { get; set; }
        public int _lineIndex { get; set; }
        public int _lineLayer { get; set; }
        public int _type { get; set; }
        public int _cutDirection { get; set; }
    }
}
