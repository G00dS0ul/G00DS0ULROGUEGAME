using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using RogueGame.Core;
using RogueGame.CustomRogueSharp;

namespace RogueGame.Core
{
    public class DungeonMap : Map<MyCell>
    {
        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();

            foreach (var cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }

        private void SetConsoleSymbolForCell(RLConsole console, MyCell cell)
        {
            if (!cell.IsExplored)
            {
                return;
            }

            if (cell.IsInFov)
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FLoor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FLoor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                }
            }
        }
    }
}
