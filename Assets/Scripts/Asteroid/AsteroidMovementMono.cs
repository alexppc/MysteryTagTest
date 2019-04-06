using UnityEngine;
using UniRx;

namespace Asteroids
{
    public class AsteroidMovementMono : MonoBehaviour, IAsteroidMovement
    {
        private Transform m_TF;

        private bool m_Init = false;

        public void Bind(Positioning pos)
        {
            if (!m_Init) Init();
            pos.Position
                .Subscribe((x) => m_TF.position = x)
                .AddTo(this);
            pos.Rotation
                .Subscribe((x) => m_TF.rotation = x)
                .AddTo(this);
            pos.Scale
                .Subscribe((x) => m_TF.localScale = x)
                .AddTo(this);
        }

        public void Reset()
        {
            m_TF.position = Vector3.zero;
            m_TF.rotation = Quaternion.identity;
            m_TF.localScale = Vector3.one;
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