using UnityEngine;
using UniRx;

namespace Player
{
    public class Info
    {
        public PlayerInfoMono PlayerInfo;
        public readonly Positioning Positioning = new Positioning();
        public readonly Health Health = new Health();

        public void Reset()
        {
            Positioning.Reset();
            Health.Reset();
        }
    }

    public class Positioning
    {
        public readonly Vector3ReactiveProperty Position = new Vector3ReactiveProperty(Vector3.zero);
        public readonly QuaternionReactiveProperty Rotation = new QuaternionReactiveProperty(Quaternion.identity);


        public void Reset()
        {
            Position.Value = Vector3.zero;
            Rotation.Value = Quaternion.identity;
        }
    }

    public class Health
    {
        public readonly IntReactiveProperty Life;
        public readonly BoolReactiveProperty Immortal;
        public bool IsAlive { get { return Life.Value > 0; } }


        private int m_DefHealth;

        public Health(int defaultHealth = 3)
        {
            m_DefHealth = defaultHealth;
            Life = new IntReactiveProperty(defaultHealth);
            Immortal = new BoolReactiveProperty(false);
        }

        public void Reset()
        {
            Life.Value = m_DefHealth;
            Immortal.Value = false;
        }
    }
}