using System;

public class Utilities
{
    public static Random Random;
    public static float Rand { get { return (float)Random.NextDouble(); } }

    static Utilities()
    {
        Random = new Random();
    }
}
