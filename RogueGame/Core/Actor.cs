using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueGame.CustomRogueSharp;
using RogueGame.Interfaces;
using RogueSharp;

namespace RogueGame.Core
{
    public class Actor : IActor, IDrawable
    {
        public string Name { get; set; }
        public int Awareness { get; set; }
        public RLColor Color { get; set; }
        public Char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole console, IMap<MyCell> map)
        {
            var cell = map.GetCell(X, Y);
            if (!cell.IsExplored)
            {
                return;
            }

            if (cell.IsInFov)
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                console.Set(X, Y, Color, Colors.FloorBackground, '.');
            }
        }
    }
}
