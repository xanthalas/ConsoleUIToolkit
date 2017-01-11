using System;

namespace xanthalas.ConsoleUIToolkit
{
    public class Cell
    {
        public char Character { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        /// <summary>
        /// Create a new Cell with the given colours
        /// </summary>
        /// <param name="character">The character this cell will contain</param>
        /// <param name="foreground">The foreground colour of the cell</param>
        /// <param name="background">The background colour of the cell</param>
        public Cell(char character, ConsoleColor background, ConsoleColor foreground)
        {
            Character= character;
            Background = background;
            Foreground = foreground;
        }

        /// <summary>
        /// Create a new Cell with default foreground and background colours
        /// </summary>
        /// <param name="character">The character this cell will contain</param>
        public Cell(char character)
        {
            Character = character;
            Foreground = Console.ForegroundColor;
            Background = Console.BackgroundColor;
        }

        /// <summary>
        /// Compare the cell passed in to see if it is the same as this cell.
        /// </summary>
        /// <param name="cell">The cell to compare to this cell</param>
        public bool IsSameAsThisCell(Cell cell)
        {
            return (cell.Character == this.Character && cell.Background == this.Background && cell.Foreground == this.Foreground);
        }
    }
}
