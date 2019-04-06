using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerMovementMono : MonoBehaviour, IPlayerMovement
    {
        private Transform m_TF;

        private bool m_Init = false;

        public void Bind(Positioning playerPos)
        {
            if (!m_Init) Init();
            playerPos.Position
                .Subscribe((pos) => m_TF.position = pos)
                .AddTo(this);
            playerPos.Rotation
                .Subscribe((rot) => m_TF.rotation = rot)
                .AddTo(this);
        }

        public void Reset()
        {
            m_TF.position = Vector3.zero;
            m_TF.rotation = Quaternion.identity;
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