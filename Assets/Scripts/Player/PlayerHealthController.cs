using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerHealthController
    {
        private Health m_Health;

        public PlayerHealthController SetPlayer(Info info)
        {
            m_Health = info.Health;
            return this;
        }

        public void SetDamage(int damage = 1)
        {
            if (!m_Health.Immortal.Value)
            {
                m_Health.Life.Value -= damage;
                m_Health.Immortal.Value = true;
                Observable.Timer(System.TimeSpan.FromSeconds(1f)).Subscribe(x => m_Health.Immortal.Value = false);
            }
        }
    }
}