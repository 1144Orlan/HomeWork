﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Courses.WinAppsOnCSharp.Lab1Exercise1RectangularForm
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new nFormInheritance());
            Application.Run(new ParentForm());
            Application.Run(new WinContainer());
        }
    }
}
