using System.Collections.Generic;

public class LayerExtensions
{
    public enum Layers { Player, Enemy, Bullet }
    private static Dictionary<Layers, int> m_Layers;

    static LayerExtensions()
    {
        m_Layers = new Dictionary<Layers, int>()
        {
            { Layers.Player, 8 },
            { Layers.Enemy, 9 },
            { Layers.Bullet, 10 }
        };
    }

    public static int GetLayer(Layers type)
    {
        int i = -1;
        m_Layers.TryGetValue(type, out i);
        return i;
    }
}
