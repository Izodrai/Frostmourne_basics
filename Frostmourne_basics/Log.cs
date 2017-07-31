using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frostmourne_basics
{
    public class Log
    {
        static public void Info(string text_to_print)
        {
            Console.WriteLine("INFO     {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
        }
        static public void WhiteInfo(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("INFO     {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }
        static public void BlueInfo(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("INFO     {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }
        static public void CyanInfo(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("INFO     {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }
        static public void MagentaInfo(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("INFO     {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }
        static public void GreenInfo(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("INFO     {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }
        static public void YellowInfo(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("INFO     {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }

        static public void Warning(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING  {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }
        static public void Error(string text_to_print)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR    {0:yyyy/MM/dd HH:mm:ss} {1}", DateTime.Now, text_to_print);
            Console.ResetColor();
        }
        static public void JumpLine()
        {
            Console.WriteLine();
        }
    }
}
