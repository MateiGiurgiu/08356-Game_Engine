using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    public static class Time
    {
        public static float deltaTime;
        public static float timeSinceGameStarted
        {
            get
            {
                DateTime now = DateTime.Now;
                TimeSpan elapsed = now - startTime;
                return (float)elapsed.TotalSeconds;
            }
        }

        private static DateTime startTime;

        public static void Start()
        {
            startTime = DateTime.Now;
        }
    }
}
