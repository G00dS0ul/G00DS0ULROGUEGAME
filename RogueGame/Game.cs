using RLNET;
using RogueGame.Core;
using RogueGame.Systems;

namespace RogueGame
{
    public class Game
    {
        public static Player? Player { get; set; }
        public static DungeonMap? DungeonMap { get; set; }
        private static bool _renderRequired = true;
        public static CommandSystem? CommandSystem { get; private set; }

        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole? _rootConsole;

        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        private static readonly int _messageWidth = 100;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 48;
        private static RLConsole _statConsole;

        private static readonly int _inventoryWidth = 100;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        public static void Main()
        {
            var fontFileName = "terminal8x8.png";

            var consoleTitle = "G00DS0ULRogueGame";

            Player = new Player();
            var mapGenerator = new MapGenerator(_mapWidth, _mapHeight);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();
            CommandSystem = new CommandSystem();

            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1.5f, consoleTitle);
                _mapConsole = new RLConsole(_mapWidth, _mapHeight);
                _messageConsole = new RLConsole(_messageWidth, _messageHeight);
                _statConsole = new RLConsole(_statWidth, _statHeight);
                _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

                _mapConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Colors.FloorBackground);
                _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

                _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater);
                _messageConsole.Print(1, 1, "Message", Colors.TextHeading);

                _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbOldStone);
                _statConsole.Print(1, 1, "Stats", Colors.TextHeading);

                _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
                _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            _rootConsole.Render += OnRootConsoleRender;

            _rootConsole.Run();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            var didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Up)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                }
                else if (keyPress.Key == RLKey.Down)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                }
                else if (keyPress.Key == RLKey.AltLeft)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                }
                else if (keyPress.Key == RLKey.Right)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                }
                else if (keyPress.Key == RLKey.Escape)
                {
                    _rootConsole.Close();
                }
            }

            if (didPlayerAct)
            {
                _renderRequired = true;
            }
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            _mapConsole.Clear();
            DungeonMap?.Draw(_mapConsole);
            Player?.Draw(_mapConsole, DungeonMap);

            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, _inventoryHeight);
            RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

            _rootConsole?.Draw();
        }
    }
}

