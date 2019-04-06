using UniRx;
using UnityEngine;

namespace Asteroids
{
    public class Info : BaseModel
    {
        public AsteroidInfoMono AsteroidMono;
        public Positioning Positioning;
        public Health Health;

        public Info(float speed, int hp = 1)
        {
            Positioning = new Positioning(speed);
            Health = new Health(hp);
        }

        public void Reset()
        {
            Positioning.Reset();
        }

        public void Dispose()
        {
            Positioning.Dispose();
        }
    }

    public class Positioning
    {
        public readonly Vector3ReactiveProperty Position, Scale;
        public readonly QuaternionReactiveProperty Rotation;
        public readonly float Speed;
        //оптимизация
        public bool FarFromBorder = false;

        public Positioning(float speed)
        {
            Position = new Vector3ReactiveProperty(Vector3.zero);
            Rotation = new QuaternionReactiveProperty(Quaternion.identity);
            Scale = new Vector3ReactiveProperty(Vector3.one);
            Speed = speed;
        }

        public void Reset()
        {
            Position.Value = Vector3.zero;
            Rotation.Value = Quaternion.identity;
            Scale.Value = Vector3.one;
            FarFromBorder = false;
        }

        public void Dispose()
        {
            Position.Dispose();
            Rotation.Dispose();
            Scale.Dispose();
        }
    }

    public class Health
    {
        public readonly IntReactiveProperty Life;

        public Health(int health)
        {
            Life = new IntReactiveProperty(health);
        }
    }
}