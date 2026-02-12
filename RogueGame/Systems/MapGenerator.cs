using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueGame.Core;

namespace RogueGame.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;

        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
        }
        public DungeonMap CreateMap()
        {
            _map.Initialize(_width, _height);
            foreach (var cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true);

                cell.IsExplored = true;
            }

            foreach (var cell in _map.GetCellsInRows(0, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false);

                cell.IsExplored = true;
            }

            foreach (var cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false);

                cell.IsExplored = true;
            }

            return _map;
        }
    }
}
