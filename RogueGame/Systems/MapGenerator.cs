using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RogueGame.Core;

namespace RogueGame.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _roomMinSize;
        private readonly int _roomMaxSize;

        private readonly DungeonMap _map;

        public MapGenerator(int width, int height, int maxRooms, int roomMinSize, int roomMaxSize)
        {
            _width = width;
            _height = height;
            _map = new DungeonMap();
            _maxRooms = maxRooms;
            _roomMinSize = roomMinSize;
            _roomMaxSize = roomMaxSize;
            _map = new DungeonMap();
        }
        public DungeonMap CreateMap()
        {
            _map.Initialize(_width, _height);
            //foreach (var cell in _map.GetAllCells())
            //{
            //    _map.SetCellProperties(cell.X, cell.Y, true, true);

            //    cell.IsExplored = true;
            //}

            //foreach (var cell in _map.GetCellsInRows(0, _height - 1))
            //{
            //    _map.SetCellProperties(cell.X, cell.Y, false, false);

            //    cell.IsExplored = true;
            //}

            //foreach (var cell in _map.GetCellsInColumns(0, _width - 1))
            //{
            //    _map.SetCellProperties(cell.X, cell.Y, false, false);

            //    cell.IsExplored = true;
            //}

            for (var r = _maxRooms; r > 0; r--)
            {
                var roomWidth = Game.Random.Next(_roomMinSize, _roomMaxSize);
                var roomHeight = Game.Random.Next(_roomMinSize, _roomMaxSize);
                var roomXPosition = Game.Random.Next(0, _width - roomWidth - 1);
                var roomYPosition = Game.Random.Next(0, _height - roomHeight - 1);

                var newRoom = new RogueSharp.Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                var newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }

            foreach ( RogueSharp.Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }

            PlacePlayer();

            for (var r = 1; r < _map.Rooms.Count; r++)
            {
                var previousRoomCenterX = _map.Rooms[r - 1].Center.X;
                var previousRoomCenterY = _map.Rooms[r - 1].Center.Y;
                var currentRoomCenterX = _map.Rooms[r].Center.X;
                var currentRoomCenterY = _map.Rooms[r].Center.Y;

                if (Game.Random.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }

            return _map;
        }

        private void CreateRoom(RogueSharp.Rectangle room)
        {
            for (var x = room.Left; x < room.Right; x++)
            {
                for (var y = room.Top; y < room.Bottom; y++)
                {
                    var cell = _map.GetCell(x, y);
                    _map.SetCellProperties(x, y, true, true);
                    cell.IsExplored = true;
                }
            }
        }

        private void PlacePlayer()
        {
            var player = Game.Player;

            if (player == null)
            {
                player = new Player();
            }

            player.X = _map.Rooms[0].Center.X;
            player.Y = _map.Rooms[0].Center.Y;

            _map.AddPlayer(player);
        }

        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (var x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                _map.SetCellProperties(x, yPosition, true, true);
            }
        }

        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (var y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map.SetCellProperties(xPosition, y, true, true);
            }
        }
    }
}
