using UnityEngine.UI;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIMapIcon : MonoBehaviour
    {
        public int LevelID { get { return m_LevelID; } }
        public Image Icon, Frame;
        public ObservablePointerClickTrigger m_Observable;
        [HideInInspector] public int State;

        [SerializeField] private int m_LevelID;

        private Action<int> m_Callback;

        public void SetClickAction(Action<int> act)
        {
            m_Callback = act;
        }

        private void OnClick(PointerEventData data)
        {
            if (m_Callback != null) m_Callback(m_LevelID);
        }

        private void Awake()
        {
            m_Observable
               .OnPointerClickAsObservable()
               .Subscribe(OnClick);
        }
    }
}