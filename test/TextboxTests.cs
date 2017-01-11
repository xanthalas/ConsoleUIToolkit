using System;
using Xunit;
using xanthalas.ConsoleUIToolkit;

namespace xanthalas.ConsoleUIToolkit.tests
{
    public class TextboxTests
    {
        [Fact]
        public void ConstructorWorks()
        {
            var textbox = new Textbox(10, 15);
            Assert.Equal(10, textbox.Width);
            Assert.Equal(15, textbox.Height);
        }

        
        [Fact]
        public void NeitherWidthNorHeightCanBeZero()
        {
            Assert.Throws<ArgumentException>(() =>  new Textbox(0, 4) );
            Assert.Throws<ArgumentException>(() =>  new Textbox(3, 0) );

            var textBox = new Textbox(10, 10);
            Assert.Throws<ArgumentException>(() =>  textBox.Width = 0 );
            Assert.Throws<ArgumentException>(() =>  textBox.Height = 0 );
        }

        [Fact]
        public void BorderCorrectlyDrawnAroundTextbox()
        {
            var tb = new Textbox(4, 5);
            Assert.Equal(' ', tb._buffer.GetCell(0,0).Character);
            Assert.Equal(' ', tb._buffer.GetCell(3,0).Character);
            Assert.Equal(' ', tb._buffer.GetCell(0,4).Character);
            Assert.Equal(' ', tb._buffer.GetCell(3,4).Character);
            Assert.Equal(' ', tb._buffer.GetCell(0,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(1,0).Character);
            tb.Border = new Border(BorderType.Single);
            Assert.Equal('\u250C', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('\u2510', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('\u2514', tb._buffer.GetCell(0,4).Character);
            Assert.Equal('\u2518', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('\u2502', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('\u2500', tb._buffer.GetCell(1,0).Character);
            tb.Border = new Border(BorderType.Double);
            Assert.Equal('\u2554', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('\u2557', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('\u255A', tb._buffer.GetCell(0,4).Character);
            Assert.Equal('\u255D', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('\u2551', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('\u2550', tb._buffer.GetCell(1,0).Character);
        }

        [Fact]
        public void SetForegroundWorks()
        {
            var tb = new Textbox(4, 5);
            tb.Foreground = ConsoleColor.Green;
            Assert.Equal(ConsoleColor.Green, tb._buffer.GetCell(0,0).Foreground);
            Assert.Equal(ConsoleColor.Green, tb._buffer.GetCell(2,3).Foreground);
            Assert.Equal(ConsoleColor.Green, tb._buffer.GetCell(1,2).Foreground);
        }

        [Fact]
        public void SetBackgroundWorks()
        {
            var tb = new Textbox(8, 10);
            tb.Background = ConsoleColor.Yellow;
            Assert.Equal(ConsoleColor.Yellow, tb._buffer.GetCell(0,0).Background);
            Assert.Equal(ConsoleColor.Yellow, tb._buffer.GetCell(5,8).Background);
            Assert.Equal(ConsoleColor.Yellow, tb._buffer.GetCell(4,4).Background);
        }

        [Fact]
        public void FillWithCharTest()
        {
            var tb = new Textbox(4, 5);
            Assert.Equal(' ', tb._buffer.GetCell(1,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(3,4).Character);
            tb.FillWithChar('*');

            Assert.Equal('*', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('*', tb._buffer.GetCell(3,4).Character);
        }

        [Fact]
        public void FillWithCharDoesNotOverwriteBorders()
        {
            var tb = new Textbox(4, 5);
            tb.Border = new Border(BorderType.Single);
            Assert.Equal('\u250C', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('\u2510', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('\u2514', tb._buffer.GetCell(0,4).Character);
            Assert.Equal('\u2518', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('\u2502', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('\u2500', tb._buffer.GetCell(1,0).Character);

            tb.FillWithChar('*');

            Assert.Equal('\u250C', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('\u2510', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('\u2514', tb._buffer.GetCell(0,4).Character);
            Assert.Equal('\u2518', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('\u2502', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('\u2500', tb._buffer.GetCell(1,0).Character);
        }

        [Fact]
        public void ClearSetsAllCharactersToSpaceNoBorder()
        {
            var tb = new Textbox(4, 5);
            tb.FillWithChar('*');
            Assert.Equal('*', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('*', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('*', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('*', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('*', tb._buffer.GetCell(0,4).Character);
            Assert.Equal('*', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('*', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('*', tb._buffer.GetCell(1,0).Character);

            tb.Clear();

            Assert.Equal(' ', tb._buffer.GetCell(1,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(3,4).Character);
            Assert.Equal(' ', tb._buffer.GetCell(0,0).Character);
            Assert.Equal(' ', tb._buffer.GetCell(3,0).Character);
            Assert.Equal(' ', tb._buffer.GetCell(0,4).Character);
            Assert.Equal(' ', tb._buffer.GetCell(3,4).Character);
            Assert.Equal(' ', tb._buffer.GetCell(0,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(1,0).Character);
            
        }

        [Fact]
        public void ClearSetsAllCharactersToSpaceWithBorder()
        {
            var tb = new Textbox(4, 5);
            tb.Border = new Border(BorderType.Double);
            tb.FillWithChar('*');
            Assert.Equal('*', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('*', tb._buffer.GetCell(2,2).Character);
            Assert.Equal('\u2554', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('\u2557', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('\u255A', tb._buffer.GetCell(0,4).Character);
            Assert.Equal('\u255D', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('\u2551', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('\u2550', tb._buffer.GetCell(1,0).Character);

            tb.Clear();

            Assert.Equal(' ', tb._buffer.GetCell(1,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(2,2).Character);
            Assert.Equal('\u2554', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('\u2557', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('\u255A', tb._buffer.GetCell(0,4).Character);
            Assert.Equal('\u255D', tb._buffer.GetCell(3,4).Character);
            Assert.Equal('\u2551', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('\u2550', tb._buffer.GetCell(1,0).Character);

        }

        [Fact]
        public void CursorLeftTests()
        {
            var tb = new Textbox(5, 5);
            tb.CursorLeft = 2;
            Assert.Equal(2, tb.CursorLeft);
            tb.CursorLeft = 0;
            Assert.Equal(0, tb.CursorLeft);
            tb.CursorLeft = 4;
            Assert.Equal(4, tb.CursorLeft);
            Assert.Throws<ArgumentException>(() =>  tb.CursorLeft = -1 );
            Assert.Throws<ArgumentException>(() =>  tb.CursorLeft = 5 );

            //Now add a border and re-check
            tb.Border = new Border(BorderType.Double);
            tb.CursorLeft = 2;
            Assert.Equal(2, tb.CursorLeft);
            Assert.Throws<ArgumentException>(() =>  tb.CursorLeft = 0 );
            Assert.Throws<ArgumentException>(() =>  tb.CursorLeft = 4 );
        }

        [Fact]
        public void CursorTopTests()
        {
            var tb = new Textbox(5, 5);
            tb.CursorTop = 2;
            Assert.Equal(2, tb.CursorTop);
            tb.CursorTop = 0;
            Assert.Equal(0, tb.CursorTop);
            tb.CursorTop = 4;
            Assert.Equal(4, tb.CursorTop);
            Assert.Throws<ArgumentException>(() =>  tb.CursorTop = -1 );
            Assert.Throws<ArgumentException>(() =>  tb.CursorTop = 5 );

            //Now add a border and re-check
            tb.Border = new Border(BorderType.Double);
            tb.CursorTop = 2;
            Assert.Equal(2, tb.CursorTop);
            Assert.Throws<ArgumentException>(() =>  tb.CursorTop = 0 );
            Assert.Throws<ArgumentException>(() =>  tb.CursorTop = 4 );
        }

        [Fact]
        public void CanWriteSimpleStringNoBorder()
        {
            var tb = new Textbox(10, 10);
            tb.Write("Test 123");

            Assert.Equal('T', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('e', tb._buffer.GetCell(1,0).Character);
            Assert.Equal('s', tb._buffer.GetCell(2,0).Character);
            Assert.Equal('t', tb._buffer.GetCell(3,0).Character);
            Assert.Equal(' ', tb._buffer.GetCell(4,0).Character);
            Assert.Equal('1', tb._buffer.GetCell(5,0).Character);
            Assert.Equal('2', tb._buffer.GetCell(6,0).Character);
            Assert.Equal('3', tb._buffer.GetCell(7,0).Character);
        }

        [Fact]
        public void CanWriteSimpleStringWithBorder()
        {
            var tb = new Textbox(10, 10);
            tb.Border = new Border(BorderType.Single);
            tb.Write("Test 456");

            Assert.Equal('T', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('e', tb._buffer.GetCell(2,1).Character);
            Assert.Equal('s', tb._buffer.GetCell(3,1).Character);
            Assert.Equal('t', tb._buffer.GetCell(4,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(5,1).Character);
            Assert.Equal('4', tb._buffer.GetCell(6,1).Character);
            Assert.Equal('5', tb._buffer.GetCell(7,1).Character);
            Assert.Equal('6', tb._buffer.GetCell(8,1).Character);
        }

        [Fact]
        public void CanWriteStringThatWrapsNoBorder()
        {
            var tb = new Textbox(10, 10);
            tb.Write("This is a long test string");

            Assert.Equal('T', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('h', tb._buffer.GetCell(1,0).Character);
            Assert.Equal('i', tb._buffer.GetCell(2,0).Character);
            Assert.Equal(' ', tb._buffer.GetCell(9,0).Character);
            Assert.Equal('l', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('o', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('n', tb._buffer.GetCell(2,1).Character);
            Assert.Equal('s', tb._buffer.GetCell(0,2).Character);
            Assert.Equal('t', tb._buffer.GetCell(1,2).Character);
        }

        [Fact]
        public void CanWriteStringThatWrapsWithBorder()
        {
            var tb = new Textbox(10, 10);
            tb.Border = new Border(BorderType.Single);
            tb.Write("This is a long test string");

            Assert.Equal('T', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('h', tb._buffer.GetCell(2,1).Character);
            Assert.Equal('i', tb._buffer.GetCell(3,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(8,1).Character);
            Assert.Equal('a', tb._buffer.GetCell(1,2).Character);
            Assert.Equal(' ', tb._buffer.GetCell(2,2).Character);
            Assert.Equal('l', tb._buffer.GetCell(3,2).Character);
            Assert.Equal('e', tb._buffer.GetCell(1,3).Character);
        }

        [Fact]
        public void LongStringWithNoWrapIsTruncatedNoBorder()
        {
            var tb = new Textbox(10, 10);
            tb.Write("This is a long test string");

            Assert.Equal('T', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('i', tb._buffer.GetCell(2,0).Character);
            Assert.Equal('s', tb._buffer.GetCell(3,0).Character);
            Assert.Equal('a', tb._buffer.GetCell(8,0).Character);
            Assert.Equal(' ', tb._buffer.GetCell(9,0).Character);
            Assert.Equal('l', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('o', tb._buffer.GetCell(1,1).Character);
        }


        [Fact]
        public void LongStringWithNoWrapIsTruncatedWithBorder()
        {
            var tb = new Textbox(10, 10);
            tb.Border = new Border(BorderType.Single);
            tb.WordWrap = false;
            tb.Write("This is a long test string");

            Assert.Equal('T', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('i', tb._buffer.GetCell(3,1).Character);
            Assert.Equal('s', tb._buffer.GetCell(4,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(8,1).Character);
            Assert.Equal('\u2502', tb._buffer.GetCell(9,1).Character);
            Assert.Equal(' ', tb._buffer.GetCell(1,2).Character);
            Assert.Equal(' ', tb._buffer.GetCell(2,2).Character);
        }


        [Fact]
        public void LongStringWhichExceedsTextboxSizeIsTruncated()
        {
            var tb = new Textbox(5, 5);
            tb.Write("This string is longer than twenty five characters");
            Assert.Equal('T', tb._buffer.GetCell(0,0).Character);
            Assert.Equal('s', tb._buffer.GetCell(0,1).Character);
            Assert.Equal('a', tb._buffer.GetCell(4,4).Character);
        }

        [Fact]
        public void LongStringWhichExceedsTextboxSizeIsTruncatedWithBorder()
        {
            var tb = new Textbox(5, 5);
            tb.Border = new Border(BorderType.Single);
            tb.Write("This string is longer than twenty five characters");
            Assert.Equal('T', tb._buffer.GetCell(1,1).Character);
            Assert.Equal('s', tb._buffer.GetCell(1,2).Character);
            Assert.Equal('i', tb._buffer.GetCell(3,3).Character);
        }


        [Fact]
        public void LongStringWhichExceedsTextboxSizeThrowsExceptionIfRequired()
        {
            var longString = "This string is longer than twenty five characters";
            var tb = new Textbox(5, 5);
            tb.ThrowExceptionOnOutOfBoundsWrite = true;
            Assert.Throws<IndexOutOfRangeException>(() =>  tb.Write(longString) );
        }

        [Fact]
        public void ClearingContentsResetsCursorToTopLeft()
        {

            var tb = new Textbox(5, 5);
            tb.CursorLeft = 3;
            tb.CursorTop = 2;
            tb.Clear();

            Assert.Equal(0, tb.CursorLeft);
            Assert.Equal(0, tb.CursorTop);

            tb.Border = new Border(BorderType.Simple);
            tb.CursorLeft = 3;
            tb.CursorTop = 2;
            tb.Clear();

            Assert.Equal(1, tb.CursorLeft);
            Assert.Equal(1, tb.CursorTop);
        }
    }
}
