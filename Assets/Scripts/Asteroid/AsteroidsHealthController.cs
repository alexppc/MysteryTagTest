using UniRx;
using UnityEngine;

namespace Asteroids
{
    public class HealthController
    {
        private ReactiveCollection<Info> m_Enemies;

        public HealthController(ReactiveCollection<Info> enemies)
        {
            m_Enemies = enemies;
            m_Enemies
                .ObserveAdd()
                .Subscribe(AddInCollection);
        }

        public void SetDamage(Info info, int damage = 1)
        {
            info.Health.Life.Value -= damage;
        }

        public void Destroy(Info info)
        {
            info.Health.Life.Value = 0;
        }

        private void AddInCollection(CollectionAddEvent<Info> addEvent)
        {
            addEvent.Value.Health.Life
                .Where(x => x <= 0)
                .DelayFrame(0, FrameCountType.EndOfFrame)
                .Subscribe(_ => m_Enemies.Remove(addEvent.Value));
        }
    }
}