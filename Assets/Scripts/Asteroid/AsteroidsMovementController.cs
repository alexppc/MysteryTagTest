using UnityEngine;
using UniRx;

namespace Asteroids
{
    public class MovementController
    {
        private ReactiveCollection<Info> m_Enemies;
        
        public MovementController(ReactiveCollection<Info> enemies)
        {
            m_Enemies = enemies;
        }

        public void Update(float delta)
        {
            Vector3 rot;
            float speed;
            for (int i = m_Enemies.Count - 1; i >= 0; --i)
            {
                speed = m_Enemies[i].Positioning.Speed;
                rot = m_Enemies[i].Positioning.Rotation.Value.eulerAngles;
                rot.x += delta * speed;
                m_Enemies[i].Positioning.Rotation.Value = Quaternion.Euler(rot);
                m_Enemies[i].Positioning.Position.Value += Vector3.left * delta * speed;
            }
        }
    }
}