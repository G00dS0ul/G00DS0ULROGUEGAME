using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using RogueGame.Core;
using RogueGame.CustomRogueSharp;
using Rectangle = RogueSharp.Rectangle;

namespace RogueGame.Core
{
    public class DungeonMap : Map<MyCell>
    {
        private FieldOfView<MyCell> _fieldOfView;

        public List<Rectangle> Rooms;

        public DungeonMap()
        {
            Rooms = [];
        }
        public override void Initialize(int width, int height)
        {
            base.Initialize(width, height);
            _fieldOfView = new FieldOfView<MyCell>(this);
        }

        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();

            foreach (var cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
        }

        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
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
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
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

        public void UpdatePlayerFieldOfView()
        {
            var player = Game.Player;

            foreach (var cell in GetAllCells())
            {
                cell.IsInFov = false;
            }

            var cellsInFov = _fieldOfView.ComputeFov(player.X, player.Y, player.Awareness, true);

            foreach (var cell in cellsInFov)
            {
                cell.IsInFov = true;
                cell.IsExplored = true;
            }
        }

        public bool SetActorPosition(Actor actor, int x, int y)
        {
            if (GetCell(x, y).IsWalkable)
            {
                SetIsWalkable(actor.X, actor.Y, true);
                actor.X = x;
                actor.Y = y;

                SetIsWalkable(actor.X, actor.Y, false);
                if (actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }

                return true;
            }

            return false;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            var cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable);
            cell.IsExplored = true;
        }
    }
}
