using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataCollectMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
       {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmLogin());
            frmLogin f2 = new frmLogin();
            f2.ShowDialog();
            if (f2.DialogResult == DialogResult.OK)
                Application.Run(new Main());
            else return; 
        }
    }
}