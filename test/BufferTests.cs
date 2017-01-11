using System;
using Xunit;
using xanthalas.ConsoleUIToolkit;

namespace xanthalas.ConsoleUIToolkit.tests
{
    public class BufferTests
    {
        [Fact]
        public void ConstructorWorks()
        {
            var buffer = new Buffer(10, 20);
            Assert.Equal(' ', buffer.GetCell(0,0).Character);
            Assert.Equal(' ', buffer.GetCell(9,19).Character);
        }

        [Fact]
        public void CannotCreateBufferWithInvalidDimension()
        {
           Buffer buffer;
           Assert.Throws<ArgumentException>(() => buffer = new Buffer(0, 2));
           Assert.Throws<ArgumentException>(() => buffer = new Buffer(-1, 3));
           Assert.Throws<ArgumentException>(() => buffer = new Buffer(4, 0));
           Assert.Throws<ArgumentException>(() => buffer = new Buffer(6, -22));
        }

        [Fact]
        public void GetCellAtInvalidLocation()
        {
           var buffer = new Buffer(5, 8);
           Assert.Throws<ArgumentException>(() => buffer.GetCell(6, 2));
           Assert.Throws<ArgumentException>(() => buffer.GetCell(4, 12));
           //Check for off-by-one errors
           Assert.Throws<ArgumentException>(() => buffer.GetCell(5, 4));
           Assert.Throws<ArgumentException>(() => buffer.GetCell(3, 8));
        }

        [Fact]
        public void IndexerReturnsCorrectCell()
        {
           var buffer = new Buffer(5, 8);
           buffer.SetCell(3, 3, new Cell('S'));
           Assert.Equal('S', buffer[3, 3].Character);
        }

        [Fact]
        public void IndexerSetterWorks()
        {
           var buffer = new Buffer(5, 8);
           buffer[2, 1] = new Cell('M');
           Assert.Equal('M', buffer[2, 1].Character);
        }

        [Fact]
        public void SetCellTest()
        {
           var buffer = new Buffer(5, 8);
           Assert.Equal(' ', buffer.GetCell(2,2).Character);
           buffer.SetCell(2,2, new Cell('*'));
           Assert.Equal('*', buffer.GetCell(2,2).Character);
        }

        [Fact]
        public void BufferShouldNeverContainNullCells()
        {
            var buffer = new Buffer(10, 12);
            for (int w = 0; w < 10; w++)
            {
                for (int h = 0; h < 12; h++)
                {
                    Assert.NotNull(buffer.GetCell(w, h));
                }
            }
        }

        [Fact]
        public void CloneReturnsNewBufferAsCopy()
        {
            const string sourceDataString = "abcdefghijklmnopqrstuvwxyz";
            char[] sourceDataArray = sourceDataString.ToCharArray();

            var sourceBuffer = new Buffer(6, 4);

            int i = 0;
            for (int w = 0; w < 6; w++)
            {
                for (int h = 0; h < 4; h++)
                {
                    sourceBuffer.SetCell(w, h, new Cell(sourceDataArray[i]));
                    i++;
                }
            }

            var copyOfBuffer = Buffer.Clone(sourceBuffer);
            Assert.Equal(sourceBuffer.Width, copyOfBuffer.Width);
            Assert.Equal(sourceBuffer.Height, copyOfBuffer.Height);
            Assert.Equal(sourceBuffer.GetCell(2, 2).Character, copyOfBuffer.GetCell(2, 2).Character);
            Assert.Equal(sourceBuffer.GetCell(5, 3).Character, copyOfBuffer.GetCell(5, 3).Character);

            //Check that clone is a true copy and doesn't just point to the source
            Assert.Equal(sourceBuffer.GetCell(2,2).Character, copyOfBuffer.GetCell(2,2).Character);
            sourceBuffer.SetCell(2, 2, new Cell('T'));
            Assert.NotEqual(sourceBuffer.GetCell(2,2).Character, copyOfBuffer.GetCell(2,2).Character);
        }
    }
}
