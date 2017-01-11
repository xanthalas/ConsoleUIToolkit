using System;

namespace xanthalas.ConsoleUIToolkit
{
    public enum BorderType { None, Simple, Single, Double };
    public class Border
    {
        public bool InheritColoursFromParent = true;

        public ConsoleColor Foreground { get; set; }
        public ConsoleColor Background { get; set; }

        public BorderType BorderType { get; set; }

        public char BorderTopLeft;
        public char BorderTopRight;
        public char BorderBottomLeft;
        public char BorderBottomRight;
        public char BorderLine;
        public char BorderColumn;

        /// <summary>
        /// Create a new Border object with the default console colours
        /// </summary>
        /// <param name="borderType">The type of border</param>
        public Border(BorderType borderType)
        {
            this.BorderType = borderType;

            setupBorderCharacters();

            Foreground = Console.ForegroundColor;
            Background = Console.BackgroundColor;
            InheritColoursFromParent = true;
        }

        /// <summary>
        /// Create a new Border object with the colours given
        /// </summary>
        /// <param name="borderType">The type of border</param>
        /// <param name="foreground">The foreground colour of the border</param>
        /// <param name="background">The background colour of the border</param>
        public Border(BorderType borderType, ConsoleColor foreground, ConsoleColor background)
            : this(borderType)
        {
            Foreground = foreground;
            Background = background;
            InheritColoursFromParent = false;
        }

        /// <summary>
        /// Define the characters used to render the border
        /// </summary>
        private void setupBorderCharacters()
        {
            switch (this.BorderType)
            {
                case BorderType.Simple:
                    BorderTopLeft = '+';
                    BorderTopRight = '+';
                    BorderBottomLeft = '+';
                    BorderBottomRight = '+';
                    BorderLine = '-';
                    BorderColumn = '|';

                    break;

                case BorderType.Single:
                    BorderTopLeft = '\u250C';
                    BorderTopRight = '\u2510';
                    BorderBottomLeft = '\u2514';
                    BorderBottomRight = '\u2518';
                    BorderLine = '\u2500';
                    BorderColumn = '\u2502';
                    break;

                case BorderType.Double:
                    BorderTopLeft = '\u2554';
                    BorderTopRight = '\u2557';
                    BorderBottomLeft = '\u255A';
                    BorderBottomRight = '\u255D';
                    BorderLine = '\u2550';
                    BorderColumn = '\u2551';
                    break;

                default:
                    BorderTopLeft = '+';
                    BorderTopRight = '+';
                    BorderBottomLeft = '+';
                    BorderBottomRight = '+';
                    BorderLine = '-';
                    BorderColumn = '|';

                    break;
            }
        }
    }
}
