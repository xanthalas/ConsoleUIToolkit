using System;
using xanthalas.ConsoleUIToolkit;

namespace xanthalas.ConsoleUIToolkit.examples
{
    public class Program
    {
        private static Window window;
        public static void Main(string[] args)
        {
           window = new Window(new Border(BorderType.Single)); 

           var backgroundTextBox = new Textbox(window.Width - 2, window.Height - 4);
           backgroundTextBox.Background = ConsoleColor.Gray;
           window.AddControl(backgroundTextBox);

           var tb = new Textbox(5, 5);
           tb.Zorder = 2;
           tb.Left = 1;
           tb.Top = 1;
           tb.Background = ConsoleColor.Blue;
           tb.Border = new Border(BorderType.Single);
           window.AddControl(tb);
           window.UpdateScreen();
           //Console.ReadKey(true);

           var tb2 = new Textbox(12, 12);
           tb.Zorder = 3;
           tb2.Left = 25;
           tb2.Top = 2;
           tb2.Background = ConsoleColor.Red;
           window.AddControl(tb2);
           window.UpdateScreen();
           //Console.ReadKey(true);

           tb.Border = new Border(BorderType.Double);
           window.UpdateScreen();
           Console.ReadKey(true);
           tb.Border = new Border(BorderType.Double, ConsoleColor.Green, ConsoleColor.Black);
           window.UpdateScreen();
           //Console.ReadKey(true);

           tb2.FillWithChar('*');
           window.UpdateScreen();
           //Console.ReadKey(true);

           tb2.Border = new Border(BorderType.Single);
           tb2.FillWithChar('"');
           tb2.Write("Waste a moment");
           tb2.Write("Now write another longer one");
           window.UpdateScreen();
           //Console.ReadKey(true);

           tb2.Border = new Border(BorderType.None);
           tb2.FillWithChar('~');
           tb2.CursorLeft = 0;
           tb2.CursorTop = 0;
           tb2.Write("Waste a moment");
           tb2.Write("Now write another longer one");
           window.UpdateScreen();
           //Console.ReadKey(true);

           tb.Clear();
           tb.WordWrap = false;
           tb.Write("This long line should be truncated");
           window.UpdateScreen();
           Console.ReadKey(true);

           backgroundTextBox.Zorder = 20;
           window.UpdateScreen();
           Console.ReadKey(true);

           backgroundTextBox.Zorder = 0;
           window.UpdateScreen();
           Console.ReadKey(true);

        }
    }
}
