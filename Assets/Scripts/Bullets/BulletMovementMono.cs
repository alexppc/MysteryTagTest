using UniRx;
using UnityEngine;

namespace Bullets
{
    public class BulletMovementMono : MonoBehaviour, IBulletMovement
    {
        private Transform m_TF;

        private bool m_Init = false;

        public void Bind(Positioning pos)
        {
            if (!m_Init) Init();
            pos.Position
                .Subscribe((x) => m_TF.position = x)
                .AddTo(this);
        }

        public void Reset()
        {
            m_TF.position = Vector3.zero;
        }

        private void Awake()
        {
            if (!m_Init) Init();
        }

        private void Init()
        {
            m_TF = transform;
            m_Init = true;
        }
    }
}