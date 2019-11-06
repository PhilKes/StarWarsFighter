using System;

namespace StarWarsFighter
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (StarWarsFighter game = new StarWarsFighter())
            {
                game.Run();
            }
        }
    }
#endif
}

