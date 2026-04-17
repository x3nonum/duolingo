using System;
using System.Windows.Forms;

namespace Duolingo
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Запускаем главную форму
            Application.Run(new Form1());
        }
    }
}