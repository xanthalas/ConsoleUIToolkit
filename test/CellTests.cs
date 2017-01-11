using System;
using Xunit;
using xanthalas.ConsoleUIToolkit;

namespace xanthalas.ConsoleUIToolkit.tests
{
    public class CellTests
    {
        [Fact]
        public void ConstructorWorks()
        {
            var cell = new Cell('Z');
            Assert.Equal('Z', cell.Character);
        }

        [Fact]
        public void TwoEqualCellsCompareAsEqual()
        {
            var cell1 = new Cell('Z');
            cell1.Background = ConsoleColor.Blue;
            cell1.Foreground = ConsoleColor.Red;
            var cell2 = new Cell('Z');
            cell2.Background = ConsoleColor.Blue;
            cell2.Foreground = ConsoleColor.Red;
            Assert.True(cell1.IsSameAsThisCell(cell2));
        }

        [Fact]
        public void TwoUnequalCellsCompareAsDifferent()
        {
            var cell1 = new Cell('Z');
            cell1.Background = ConsoleColor.Blue;
            cell1.Foreground = ConsoleColor.Red;
            var cell2 = new Cell('A');
            cell2.Background = ConsoleColor.Blue;
            cell2.Foreground = ConsoleColor.Red;
            Assert.False(cell1.IsSameAsThisCell(cell2));

            cell2.Character = 'A';
            cell2.Background = ConsoleColor.Green;
            Assert.False(cell1.IsSameAsThisCell(cell2));

            cell2.Background = ConsoleColor.Blue;
            cell2.Foreground = ConsoleColor.Yellow;
            Assert.False(cell1.IsSameAsThisCell(cell2));
        }

        [Fact]
        public void SetCellTest()
        {
           var cell = new Cell('M', ConsoleColor.Blue, ConsoleColor.Yellow);
           Assert.Equal('M', cell.Character);
           Assert.Equal(ConsoleColor.Blue, cell.Background);
           Assert.Equal(ConsoleColor.Yellow, cell.Foreground);
           cell.Character = 'Q';
           Assert.Equal('Q', cell.Character);
           cell.Background = ConsoleColor.Green;
           cell.Foreground = ConsoleColor.Black;
           Assert.Equal(ConsoleColor.Green, cell.Background);
           Assert.Equal(ConsoleColor.Black, cell.Foreground);
        }
    }
}
