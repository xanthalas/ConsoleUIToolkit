using System;
using System.Collections.Generic;

namespace xanthalas.ConsoleUIToolkit
{
    public class Buffer
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        protected Cell[,] _cells;

        #region Constructors
        /// <summary>
        /// Create a new buffer
        /// </summary>
        /// <param name="width">Width of the window</param>
        /// <param name="height">Height of the window</param>
        public Buffer(int width, int height)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentException("Cannot create buffer with a width or height less than 1");
            }

            Width = width;
            Height = height;
            _cells = new Cell[Width, Height];
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    _cells[w, h] = new Cell(' ');
                }
            }
        }

        /// <summary>
        /// Create a new buffer. Private constructor used by the static
        /// </summary>
        /// <param name="width">Width of the window</param>
        /// <param name="height">Height of the window</param>
        /// <param name="noInitialise">Used only to distinguish this constructor</param>
        /// <note>
        /// This constructor is only used in the Clone method to skip the initialisation of cells as
        /// they will be copied from the source buffer so there is no need to initialise them.
        /// </note>
        private Buffer(int width, int height, bool noInitialise)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentException("Cannot create buffer with a width or height less than 1");
            }

            Width = width;
            Height = height;
            _cells = new Cell[Width, Height];
        }

        #endregion

        #region Public methods

        public Cell GetCell(int left, int top)
        {
            if (left < 0 || left >= Width)
            {
                throw new ArgumentException("Parameter 'left' is outside of the range for the buffer");
            }

            if (top < 0 || top >= Height)
            {
                throw new ArgumentException("Parameter 'top' is outside of the range for the buffer");
            }

            return _cells[left, top];
        }

        public void SetCell(int left, int top, Cell cell)
        {
            if (left < 0 || left >= Width)
            {
                throw new ArgumentException("Parameter 'left' is outside of the range for the buffer");
            }

            if (top < 0 || top >= Height)
            {
                throw new ArgumentException("Parameter 'top' is outside of the range for the buffer");
            }

            _cells[left, top] = cell;
        }


        /// <summary>
        /// Indexer to Get/Set a given cell
        /// </summary>
        public Cell this[int left, int top]
        {
            get
            {
                return _cells[left, top];
            }
            set
            {
                _cells[left, top] = value;
            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a copy of the source buffer.
        /// </summary>
        /// <param name="sourceBuffer">The buffer to clone</param>
        public static Buffer Clone(Buffer sourceBuffer)
        {
            var cloneBuffer = new Buffer(sourceBuffer.Width, sourceBuffer.Height, true);

            for (int w = 0; w < sourceBuffer.Width; w++)
            {
                for (int h = 0; h < sourceBuffer.Height; h++)
                {
                    var cellToClone = sourceBuffer.GetCell(w, h);
                    cloneBuffer.SetCell(w, h, new Cell(cellToClone.Character, cellToClone.Background, cellToClone.Foreground));
                }
            }

            return cloneBuffer;
        }
        #endregion

    }
}

