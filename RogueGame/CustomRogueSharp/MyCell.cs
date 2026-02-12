using RogueSharp;

namespace RogueGame.CustomRogueSharp
{
    /// <summary>
    /// Extended Cell class with IsExplored and IsInFov properties
    /// </summary>
    public class MyCell : Cell
    {
        /// <summary>
        /// Has this cell ever been explored (seen) by the player?
        /// </summary>
        public bool IsExplored { get; set; }

        /// <summary>
        /// Is this cell currently in the field of view?
        /// This will be updated each time FOV is calculated
        /// </summary>
        public bool IsInFov { get; set; }

        // Constructor
        public MyCell()
        {
            IsExplored = false;
            IsInFov = false;
        }

        // Constructor with parameters
        public MyCell(int x, int y, bool isTransparent, bool isWalkable)
            : base(x, y, isTransparent, isWalkable)
        {
            IsExplored = false;
            IsInFov = false;
        }
    }
}