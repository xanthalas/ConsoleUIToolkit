using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("test")]

namespace xanthalas.ConsoleUIToolkit
{
    public enum Visibility { Visible, Hidden}

    public abstract class Control
    {        
        #region private and protected members {{{
        private int _width = 0;
        private int _height = 0;
        private ConsoleColor _background = ConsoleColor.Black;
        private ConsoleColor _foreground = ConsoleColor.White;
        
        internal protected Buffer _buffer;
        protected Border _border = new Border(BorderType.None);
        #endregion

        #region public members {{{


        /// <summary>
        /// The parent control to which this control belongs. Controls without a parent will not be rendered.
        /// </summary>
        public Control Parent;

        /// <summary>
        /// The left position of this control relative to it's parent
        /// </summary>
        public int Left = 0;
        ///
        /// <summary>
        /// The top position of this control relative to it's parent
        /// </summary>
        public int Top = 0;

        /// <summary>
        /// The Z order of this control within it's parent
        /// </summary>
        public int Zorder = 1;

        /// <summary>
        /// The visibility of this control
        /// </summary>
        public Visibility ControlVisibility = Visibility.Visible;

        /// <summary>
        /// Gets/Sets the width of the control
        /// </summary>
        public int Width 
        {
            get { return _width; }
            set 
            {
                if (value < 1)
                {
                    throw new ArgumentException("Control width must be greater than zero");
                }
                else
                {
                    _width = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the Height of the control
        /// </summary>
        public int Height 
        {
            get { return _height; }
            set 
            {
                if (value < 1)
                {
                    throw new ArgumentException("Control height must be greater than zero");
                }
                else
                {
                    _height = value;
                }
            }
        }
        
        /// <summary>
        /// Gets/Sets the border used on the control
        /// </summary>
        public virtual Border Border 
        {
            get { return _border; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentException("Error. Cannot set the border to null");
                }

                _border = value;
                addBorder();
            }
        }

        /// <summary>
        /// General purpose object. Can be used to store anything you like against this control
        /// </summary>
        public object Tag;
        #endregion

        #region public methods {{{
        /// <summary>
        /// General purpose object. Can be used to store anything you like against this control
        /// </summary>
        public abstract void Draw(Buffer buffer);

        /// <summary>
        /// Gets/Sets the background colour of this control. The set will affect every cell in the control
        /// </summary>
        public ConsoleColor Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                processAllCells((x) => { x.Background = value; });
            }
        }

        /// <summary>
        /// Gets/Sets the foreground colour of this control. The set will affect every cell in the control
        /// </summary>
        public ConsoleColor Foreground
        {
            get
            {
                return _foreground;
            }
            set
            {
                _foreground = value;
                processAllCells((x) => { x.Foreground = value; });
            }
        }


        /// <summary>
        /// Fill the window with the given character with the colours given.
        /// </summary>
        /// <param name="character">Character to fill with</param>
        public void FillWithChar(char character)
        {
            processAllCells((x) => { x.Character = character; });

            if (Border.BorderType != BorderType.None)
            {
                addBorder();
            }
        }

        /// <summary>
        /// Clears the contents of the control
        /// </summary>
        public virtual void Clear()
        {
            processAllCells((x) => { x.Character = ' '; });

            if (Border.BorderType != BorderType.None)
            {
                addBorder();
            }
        }
        #endregion


        #region private and protected methods
        /// <summary>
        /// Iterate over all the cells in the control, calling the processor passed in on each one
        /// </summary>
        /// <param name="processor">Method to call for each cell</param>
        protected void processAllCells(Action<Cell> processor)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    processor(_buffer[w, h]);
                }
            }
        }

        /// <summary>
        /// Adds a border to the control
        /// </summary>
        protected void addBorder()
        {
            //Top
            _buffer[0, 0].Character = Border.BorderTopLeft;
            _buffer[0, 0].Foreground = getForeground();
            _buffer[0, 0].Background = getBackground();

            for (int i = 1; i < Width - 1; i++)
            {
                _buffer[i, 0].Character = Border.BorderLine;
                _buffer[i, 0].Foreground = getForeground();
                _buffer[i, 0].Background = getBackground();
            }
            _buffer[Width - 1, 0].Character = Border.BorderTopRight;
            _buffer[Width - 1, 0].Foreground = getForeground();
            _buffer[Width - 1, 0].Background = getBackground();

            //Bottom
            _buffer[0, Height - 1].Character = Border.BorderBottomLeft;
            _buffer[0, Height - 1].Foreground = getForeground();
            _buffer[0, Height - 1].Background = getBackground();
            for (int i = 1; i < Width - 1; i++)
            {
                _buffer[i, Height - 1].Character = Border.BorderLine;
                _buffer[i, Height - 1].Foreground = getForeground();
                _buffer[i, Height - 1].Background = getBackground();
            }
            _buffer[Width - 1, Height - 1].Character = Border.BorderBottomRight;
            _buffer[Width - 1, Height - 1].Foreground = getForeground();
            _buffer[Width - 1, Height - 1].Background = getBackground();

            //Sides
            for (int j = 1; j < Height - 1; j++)
            {
                _buffer[0, j].Character = Border.BorderColumn;
                _buffer[0, j].Foreground = getForeground();
                _buffer[0, j].Background = getBackground();
                _buffer[Width - 1, j].Character = Border.BorderColumn;
                _buffer[Width - 1, j].Foreground = getForeground();
                _buffer[Width - 1, j].Background = getBackground();
            }
        }

        protected ConsoleColor getForeground()
        {
            return (Border.InheritColoursFromParent ? _foreground : Border.Foreground);

        }
        protected ConsoleColor getBackground()
        {
            return (Border.InheritColoursFromParent ? _background : Border.Background);

        }
        #endregion
    }
}
