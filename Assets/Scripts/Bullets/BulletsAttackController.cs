using UniRx;
using UnityEngine;

namespace Bullets
{
    public class AttackController
    {
        private ReactiveCollection<Info> m_Bullets;
        private int m_AsteroidLayer;
        private Asteroids.HealthController m_AsteroidHC;

        public AttackController(ReactiveCollection<Info> bullets)
        {
            m_Bullets = bullets;
            m_AsteroidLayer = LayerExtensions.GetLayer(LayerExtensions.Layers.Enemy);

            m_Bullets
                .ObserveAdd()
                .Subscribe(AddToCollection);
        }

        public AttackController SetAsteroidHealthController(Asteroids.HealthController asteroidHC)
        {
            m_AsteroidHC = asteroidHC;
            return this;
        }

        private void AddToCollection(CollectionAddEvent<Info> addEvent)
        {
            addEvent.Value.BulletMono.BindCollision(OnCollision);
        }

        private void OnCollision(Info sender, Collider coll)
        {
            if (coll.gameObject.layer == m_AsteroidLayer)
            {
                var aim = coll.gameObject.GetComponent<Asteroids.AsteroidInfoMono>();
                if (aim == null) return;
                m_AsteroidHC.SetDamage(aim.Info, 1);
                m_Bullets.Remove(sender);
            }
        }
    }
}