using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueGame.Interfaces
{
    public interface IActor
    {
        public string Name { get; set; }
        public int Awareness { get; set; }
    }
}
