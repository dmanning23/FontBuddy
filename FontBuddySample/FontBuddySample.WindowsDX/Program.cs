﻿using System;

namespace FontBuddySample.WindowsDX
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
            //using (var game = new RotatedTextGame())
                game.Run();
        }
    }
#endif
}
