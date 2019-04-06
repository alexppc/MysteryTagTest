using UniRx;
using UnityEngine;

namespace Bullets
{
    public class MovementController
    {
        private ReactiveCollection<Info> m_Bullets;

        public MovementController(ReactiveCollection<Info> bullets)
        {
            m_Bullets = bullets;
        }

        public void Update(float delta)
        {
            if (m_Bullets.Count == 0) return;

            for (int i = m_Bullets.Count - 1; i >= 0; --i)
            {
                m_Bullets[i].Positioning.Position.Value += Vector3.right * m_Bullets[i].Positioning.Speed * delta;
            }
        }
    }
}