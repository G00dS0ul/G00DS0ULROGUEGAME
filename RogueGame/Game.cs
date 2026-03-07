using RLNET;
using RogueGame.Core;
using RogueGame.Systems;
using RogueSharp.Random;

namespace RogueGame
{
    public class Game
    {
        public static Player? Player { get; set; }
        public static DungeonMap? DungeonMap { get; set; }
        public static MessageLog? MessageLog { get; private set; }
        private static bool _renderRequired = true;
        public static CommandSystem? CommandSystem { get; private set; }
        public static IRandom Random { get; private set; }

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
            var seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            var fontFileName = "terminal8x8.png";

            var consoleTitle = $"G00dS0ulRogueGame - Level 1 - Seed {seed}";

            var mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 7, 13);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();
            CommandSystem = new CommandSystem();

            MessageLog = new MessageLog();
            MessageLog.Add("The Rogue arrives on level 1");
            MessageLog.Add($"Level created with seed '{seed}'");

            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1.5f, consoleTitle);
                _mapConsole = new RLConsole(_mapWidth, _mapHeight);
                _messageConsole = new RLConsole(_messageWidth, _messageHeight);
                _statConsole = new RLConsole(_statWidth, _statHeight);
                _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

                _mapConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Colors.FloorBackground);
                _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

                _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
                _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            _rootConsole.Update += OnRootConsoleUpdate;  

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
                else if (keyPress.Key == RLKey.Left)
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
            MessageLog.Draw(_messageConsole);
            DungeonMap?.Draw(_mapConsole);
            Player?.Draw(_mapConsole, DungeonMap);
            Player.DrawStats(_statConsole);

            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, _inventoryHeight);
            RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

            _rootConsole?.Draw();
        }
    }
}

