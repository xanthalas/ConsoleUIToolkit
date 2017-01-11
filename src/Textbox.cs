using System;

namespace xanthalas.ConsoleUIToolkit
{
    public class Textbox : Control
    {
        private int _cursorLeft = 0;
        private int _cursorTop = 0;

        /// <summary>
        /// Indicates whether to wrap around when writing strings to the textbox
        /// </summary>
        public bool WordWrap = true;

        /// <summary>
        /// Indicates whether an exception should be thrown if an attempt is made to write outside the textbox area
        /// </summary>
        public bool ThrowExceptionOnOutOfBoundsWrite = false;

        public Textbox(int width, int height)
        {
            if (width < 1 || height < 1)
            {
                throw new ArgumentException("Control width/height must be greater than zero");
            }
            Width = width;
            Height = height;
            _buffer = new Buffer(width, height);
            processAllCells((x) => { x = new Cell('X', Window.DefaultForeground, Window.DefaultForeground); } );
        }


        #region public properties {{{
        public int CursorLeft
        {
            get { return _cursorLeft; }
            set 
            {
                if (value < 0 || 
                    value >= Width ||
                    (Border.BorderType != BorderType.None && value == 0) ||
                    (Border.BorderType != BorderType.None && value >= Width -1))
                {
                    throw new ArgumentException("Cursor left position cannot be outside the control area or on a border");
                }

                _cursorLeft = value;
            }
        }

        public int CursorTop
        {
            get { return _cursorTop; }
            set 
            {
                if (value < 0 || 
                    value >= Width ||
                    (Border.BorderType != BorderType.None && value == 0) ||
                    (Border.BorderType != BorderType.None && value >= Width -1))
                {
                    throw new ArgumentException("Cursor top position cannot be outside the control area or on a border");
                }

                _cursorTop = value;
            }
        }

        public override Border Border
        {
            get
            {
                return _border;
            }
            set
            {
                base.Border = value;
                if (_border.BorderType != BorderType.None)
                {
                    //If the position we are next due to write is now a border then move the cursor
                    //to the start of the next row.
                    if (_cursorLeft == 0)
                    {
                        _cursorLeft = 1;
                    }
                    else if (_cursorLeft == Width - 1)
                    {
                        _cursorLeft = 1;
                        _cursorTop += 1;
                    }

                    //If the cursor is on the top border then move it to line 1
                    //If it is on the bottom border then leave it where it is - nothing more can be written here
                    if (_cursorTop == 0)
                    {
                        _cursorTop = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the contents of the Textbox and resets the cursor to the top left
        /// </summary>
        public override void Clear()
        {
            base.Clear();

            if (Border.BorderType == BorderType.None)
            {
                CursorLeft = 0;
                CursorTop = 0;
            }
            else
            {
                CursorLeft = 1;
                CursorTop = 1;
            }
        }
        #endregion }}}

        #region public methods {{{
        /// <summary>
        /// General purpose object. Can be used to store anything you like against this control
        /// </summary>
        public override void Draw(Buffer buffer)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    buffer.SetCell(Left + w, Top + h, _buffer.GetCell(w, h));
                }
            }
        }

        /// <summary>
        /// Write the string to the Textbox at the current cursor position
        /// </summary>
        /// <param name="stringToWrite">String to write to the window</param>
        public void Write(string stringToWrite)
        {
            int left = _cursorLeft;
            int top = _cursorTop;
            int maxWidth = (Border.BorderType != BorderType.None) ? Width - 2 : Width - 1;
            int maxHeight = (Border.BorderType != BorderType.None) ? Height - 2 : Height - 1;

            var characters = stringToWrite.ToCharArray();
            foreach (var character in characters)
            {
                //Check if write is out of bounds
                if (left > maxWidth || top > maxHeight)
                {
                    if (ThrowExceptionOnOutOfBoundsWrite)
                    {
                        throw new IndexOutOfRangeException($"Writing string '{stringToWrite}' at position {left},{top}");
                    }
                    else
                    {
                        return;
                    }
                }

                _buffer[left, top].Character = character;
                left++;
                _cursorLeft++;
                if (left > maxWidth)
                {
                    top++;
                    _cursorTop++;
                    left = (Border.BorderType != BorderType.None) ? 1 : 0;
                    _cursorLeft = left;

                    if (!WordWrap)
                    {
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
