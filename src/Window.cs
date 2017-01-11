using System;
using System.Collections.Generic;
using System.Linq;

namespace xanthalas.ConsoleUIToolkit
{
    public class Window : IDisposable
    {
        #region public members
        /// <summary>
        /// The background color of the console when first started
        /// </summary>
        public static ConsoleColor DefaultBackground;

        ///
        /// <summary>
        /// The foreground color of the console when first started
        /// </summary>
        public static ConsoleColor DefaultForeground;

        /// <summary>
        /// Gets the width of the console window
        /// </summary>
        public int ConsoleWidth
        {
            get
            {
                return Console.WindowWidth;
            }
        }

        /// <summary>
        /// Gets the height of the console window
        /// </summary>
        public int ConsoleHeight
        {
            get
            {
                return Console.WindowHeight;
            }
        }

        /// <summary>
        /// The current cursor position (left)
        /// </summary>
        public int CursorPositionLeft { get; set; }

        /// <summary>
        /// The current cursor position (top)
        /// </summary>
        public int CursorPositionTop { get; set; }

        #endregion

        #region Private members
        private List<Control> children;

        /// <summary>
        /// The width of the window. This can be less than the console width
        /// </summary>
        private int _width = 0;
        /// <summary>
        /// The height of the window. This can be less than the console height
        /// </summary>
        private int _height = 0;

        /// <summary>
        /// The buffer which was last rendered to the screen
        /// </summary>
        private Buffer currentScreenBuffer;

        /// <summary>
        /// The buffer which is currently being modified
        /// </summary>
        private Buffer workingBuffer;

        private ConsoleColor _backgroundOnStartup = Console.BackgroundColor;
        private ConsoleColor _foregroundOnStartup = Console.ForegroundColor;

        private ConsoleColor _background;
        private ConsoleColor _foreground;

        private bool disposed = false;

        #endregion

        #region Properties

        public int Width
        {
            get
            {
                return _width;
            }
        }
        
        public int Height
        {
            get
            {
                return _height;
            }
        }

        /// <summary>
        /// Gets/Sets the background colour of all cells on the console
        /// </summary>
        public ConsoleColor Background
        {
            get
            {
                return _background;
            }
            set
            {
                var currentBackground = _background;
                _background = value;

                //Might have to look at asking all child controls to honour this colour change
                //processAllCells((x) => { x.Background = Background; });
            }
        }

        /// <summary>
        /// Gets/Sets the foreground colour of all cells on the console
        /// </summary>
        public ConsoleColor Foreground
        {
            get
            {
                return _foreground;
            }
            set
            {
                var currentForeground = _foreground;
                _foreground = value;

                //Might have to look at asking all child controls to honour this colour change
                //processAllCells((x) => { x.Foreground = Foreground; });
            }
        }

        /// <summary>
        /// The type of border to use on this window
        /// </summary>
        public Border ShowBorder { get; set; }

        public int Zorder 
        { 
            get 
            {
                return 0;           //This is the top level so the Zorder is always zero
            } 
        }

        #endregion

        #region Constructors and Dispose
        /// <summary>
        /// Store the base console colours before any windows are created
        /// </summary>
        public Window()
        {
            DefaultBackground = Console.BackgroundColor;
            DefaultForeground = Console.ForegroundColor;
            _backgroundOnStartup = Console.BackgroundColor;
            _foregroundOnStartup = Console.ForegroundColor;

            children = new List<Control>();

            Console.CursorVisible = false;
            _width = Console.WindowWidth;
            _height = Console.WindowHeight - 1;

            currentScreenBuffer = new Buffer(_width, _height);
            workingBuffer = Buffer.Clone(currentScreenBuffer);
            Console.Clear();
        }


        /// <summary>
        /// Create a new window at the position and with the size given. Window will have the border given.
        /// </summary>
        /// <param name="showBorder">The border to use for this window</param>
        public Window(Border showBorder)
            : this()
        {
            ShowBorder = showBorder;

            if (showBorder.BorderType == BorderType.None)
            {
                CursorPositionLeft = 0;
                CursorPositionTop = 0;
            }
            else
            {
                CursorPositionLeft = 1;
                CursorPositionTop = 1;
                addBorder();
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                //Put the console colours back to what they were before we started
                Console.BackgroundColor = _backgroundOnStartup;
                Console.ForegroundColor = _foregroundOnStartup;
            }
        }
        #endregion

        #region Public methods
        public void AddControl(Control control)
        {
            children.Add(control);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Adds a border to the window
        /// </summary>
        private void addBorder()
        {
            //Top
            workingBuffer.SetCell(0, 0, new Cell(ShowBorder.BorderTopLeft, ShowBorder.Background, ShowBorder.Foreground));

            for (int i = 1; i < _width - 1; i++)
            {
                workingBuffer.SetCell(i, 0, new Cell(ShowBorder.BorderLine, ShowBorder.Background, ShowBorder.Foreground));
            }
            workingBuffer.SetCell(_width - 1, 0, new Cell(ShowBorder.BorderTopRight, ShowBorder.Background, ShowBorder.Foreground));

            //Bottom
            workingBuffer.SetCell(0, _height - 1, new Cell(ShowBorder.BorderBottomLeft, ShowBorder.Background, ShowBorder.Foreground));
            for (int i = 1; i < _width - 1; i++)
            {
                workingBuffer.SetCell(i, _height - 1, new Cell(ShowBorder.BorderLine, ShowBorder.Background, ShowBorder.Foreground));
            }
            workingBuffer.SetCell(_width - 1, _height - 1, new Cell(ShowBorder.BorderBottomRight, ShowBorder.Background, ShowBorder.Foreground));

            //Sides
            for (int j = 1; j < _height - 1; j++)
            {
                workingBuffer.SetCell(0, j, new Cell(ShowBorder.BorderColumn, ShowBorder.Background, ShowBorder.Foreground));
                workingBuffer.SetCell(_width - 1, j, new Cell(ShowBorder.BorderColumn, ShowBorder.Background, ShowBorder.Foreground));
            }

            Console.ForegroundColor = DefaultForeground;
            Console.BackgroundColor = DefaultBackground;
        }

        #endregion

        #region Class methods
        /// <summary>
        /// Redraw all the windows
        /// </summary>
        public void UpdateScreen()
        {
            //First, get all the windows to update the working buffer with their latest state
            var childControls = from w in children orderby w.Zorder select w;

            foreach(var child in childControls)
            {
                if (child.ControlVisibility == Visibility.Visible)
                {
                    child.Draw(workingBuffer);
                }
            }

            //Determine all the cells in the working buffer which no longer match the current
            //screen buffer. Any which have changed are written to the screen

            for(int w = 0; w < currentScreenBuffer.Width; w++)
            {
                for(int h = 0; h < currentScreenBuffer.Height; h++)
                {
                    var cell = workingBuffer.GetCell(w, h);

                    if (!currentScreenBuffer.GetCell(w, h).IsSameAsThisCell(cell))
                    {
                        Console.SetCursorPosition(w, h);
                        Console.ForegroundColor = cell.Foreground;
                        Console.BackgroundColor = cell.Background;
                        Console.Write(cell.Character);
                    }
                }
            }
            
            //Update the current screen buffer to be the same as the working buffer as the two are now in sync
            currentScreenBuffer = Buffer.Clone(workingBuffer);
        }
        #endregion
    }
}
