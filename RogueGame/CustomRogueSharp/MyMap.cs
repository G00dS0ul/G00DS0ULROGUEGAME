using RogueSharp;
using System.Collections.Generic;

namespace RogueGame.CustomRogueSharp
{
    /// <summary>
    /// Extended Map class that uses DungeonCell and manages field of view
    /// </summary>
    public class MyMap : Map<MyCell>
    {
        private FieldOfView<MyCell> _fieldOfView;

        // Constructor
        public MyMap(int width, int height) : base(width, height)
        {
            _fieldOfView = new FieldOfView<MyCell>(this);
        }

        /// <summary>
        /// Update the field of view from a specific position
        /// </summary>
        /// <param name="x">X position of the observer</param>
        /// <param name="y">Y position of the observer</param>
        /// <param name="radius">Radius of vision</param>
        /// <param name="lightWalls">Should walls be lit?</param>
        public void UpdateFieldOfView(int x, int y, int radius, bool lightWalls = true)
        {
            // First, mark all cells as NOT in FOV
            foreach (MyCell cell in GetAllCells())
            {
                cell.IsInFov = false;
            }

            // Compute the new field of view
            var cellsInFov = _fieldOfView.ComputeFov(x, y, radius, lightWalls);

            // Mark cells in FOV and explored
            foreach (MyCell cell in cellsInFov)
            {
                cell.IsInFov = true;
                cell.IsExplored = true; // Once seen, always explored
            }
        }

        /// <summary>
        /// Check if a cell is in the current field of view
        /// </summary>
        public bool IsInFov(int x, int y)
        {
            return this[x, y].IsInFov;
        }

        /// <summary>
        /// Check if a cell has been explored
        /// </summary>
        public bool IsExplored(int x, int y)
        {
            return this[x, y].IsExplored;
        }

        /// <summary>
        /// Get a cell at the specified coordinates
        /// </summary>
        public new MyCell GetCell(int x, int y)
        {
            return this[x, y];
        }

        /// <summary>
        /// Override SetCellProperties to work with DungeonCell
        /// </summary>
        public new void SetCellProperties(int x, int y, bool isTransparent, bool isWalkable)
        {
            this[x, y].IsTransparent = isTransparent;
            this[x, y].IsWalkable = isWalkable;
        }
    }
}