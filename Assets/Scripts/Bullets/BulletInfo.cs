using UniRx;

namespace Bullets
{
    public class Info : BaseModel
    {
        public BulletInfoMono BulletMono;
        public Positioning Positioning;

        public Info(float speed)
        {
            Positioning = new Positioning(speed);
        }

        public void Dispose()
        {
            Positioning.Dispose();
        }
    }

    public class Positioning
    {
        public readonly Vector3ReactiveProperty Position;
        public readonly float Speed;

        public Positioning(float speed)
        {
            Position = new Vector3ReactiveProperty();
            Speed = speed;
        }

        public void Dispose()
        {
            Position.Dispose();
        }
    }
}