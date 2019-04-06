using UnityEngine;
using UniRx;

namespace Asteroids
{
    public class AttackController
    {
        private ReactiveCollection<Info> m_Enemies;
        private int m_PlayerLayer, m_AsteroidLayer;
        private Player.PlayerHealthController m_PlayerHealth;
        private HealthController m_AsteroidHC;

        public AttackController(ReactiveCollection<Info> enemies)
        {
            m_Enemies = enemies;
            m_Enemies
                .ObserveAdd()
                .Subscribe(AddToCollection);

            m_PlayerLayer = LayerExtensions.GetLayer(LayerExtensions.Layers.Player);
            m_AsteroidLayer = LayerExtensions.GetLayer(LayerExtensions.Layers.Enemy);
        }

        public AttackController SetPlayerHelathController(Player.PlayerHealthController playerHC)
        {
            m_PlayerHealth = playerHC;
            return this;
        }

        public AttackController SetAsteroidHealthController(HealthController asteroidHC)
        {
            m_AsteroidHC = asteroidHC;
            return this;
        }

        private void OnCollision(Collider coll)
        {
            if (coll.gameObject.layer == m_AsteroidLayer)
            {
                var aim = coll.gameObject.GetComponent<AsteroidInfoMono>();
                if (aim == null) return;
                m_AsteroidHC.Destroy(aim.Info);
            }
            else if (coll.gameObject.layer == m_PlayerLayer)
            {
                m_PlayerHealth.SetDamage();
            }
        }

        private void AddToCollection(CollectionAddEvent<Info> addEvent)
        {
            addEvent.Value.AsteroidMono.BindCollision(OnCollision);
        }
    }
}