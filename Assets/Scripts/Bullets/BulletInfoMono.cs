using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

namespace Bullets
{
    public interface IBulletMovement : IReset
    {
        void Bind(Positioning pos);
    }

    public class BulletInfoMono : MonoBehaviour, Pools.IPooled
    {
        public Info Info { get { return m_Info; } }

        private Info m_Info;
        private ObservableTriggerTrigger m_CollisionTrigger;
        private IBulletMovement m_Movement;

        private Action<Info, Collider> m_CollisionAction;
        private bool m_Init = false;

        public void Bind(BaseModel model)
        {
            if (!m_Init) Init();
            Info info = model as Info;
            if (info == null)
            {
                //error
                return;
            }
            m_Info = info;
            info.BulletMono = this;
            m_Movement.Bind(info.Positioning);
        }

        public void Reset()
        {
            m_Movement.Reset();

            m_Info = null;
            m_CollisionAction = null;
        }
        
        public void BindCollision(Action<Info, Collider> act)
        {
            m_CollisionAction = act;
        }

        private void CollisionRedirection(Collider coll)
        {
            if (m_CollisionAction != null) m_CollisionAction(m_Info, coll);
        }

        private void Awake()
        {
            if (!m_Init) Init();
        }

        private void Init()
        {
            m_CollisionTrigger = GetComponentInChildren<ObservableTriggerTrigger>();
            m_Movement = GetComponent<IBulletMovement>();

            m_CollisionTrigger
                .OnTriggerEnterAsObservable()
                .Subscribe(CollisionRedirection)
                .AddTo(this);
            m_Init = true;
        }
    }
}