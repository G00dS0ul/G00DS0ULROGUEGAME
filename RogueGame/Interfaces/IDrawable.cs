using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueGame.CustomRogueSharp;
using RogueSharp;

namespace RogueGame.Interfaces
{
    public interface IDrawable
    {
        public RLColor Color { get; set; }
        public Char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        void Draw(RLConsole console, IMap<MyCell> map);
    }
}
