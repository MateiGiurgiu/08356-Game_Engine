using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Objects
{
    public static class CustomRandom
    {
        public static Random random = new Random();

        public static int[] GetDifferentValues(int numberOfValues, int min, int max)
        {
            List<int> values = new List<int>();

            for(int i = 0; i < numberOfValues; i++)
            {
                int randomNumber;
                do
                {
                    randomNumber = random.Next(min, max);
                }
                while (values.Contains(randomNumber));
                values.Add(randomNumber);
            }

            return values.ToArray();
        }

        public static int GetRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
