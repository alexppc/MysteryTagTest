using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

namespace Asteroids
{
    public interface IAsteroidMovement : IReset
    {
        void Bind(Positioning pos);
    }

    public class AsteroidInfoMono : MonoBehaviour, Pools.IPooled
    {
        public Info Info { get { return m_Info; } }

        private Info m_Info;
        private IAsteroidMovement m_Movement;
        private ObservableTriggerTrigger m_CollisionTrigger;

        private Action<Collider> m_CollisionAction;
        private bool m_Init = false;

        public void Reset()
        {
            m_Movement.Reset();

            m_Info = null;
            m_CollisionAction = null;
        }

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
            info.AsteroidMono = this;
            m_Movement.Bind(info.Positioning);
        }

        public void BindCollision(Action<Collider> act)
        {
            m_CollisionAction = act;
        }

        private void CollisionRedirection(Collider coll)
        {
            if (m_CollisionAction != null) m_CollisionAction(coll);
        }

        private void Awake()
        {
            if (!m_Init) Init();
        }

        private void Init()
        {
            m_CollisionTrigger = GetComponent<ObservableTriggerTrigger>();
            m_Movement = GetComponent<IAsteroidMovement>();

            m_CollisionTrigger
                .OnTriggerEnterAsObservable()
                .Subscribe(CollisionRedirection)
                .AddTo(this);
            m_Init = true;
        }
    }
}