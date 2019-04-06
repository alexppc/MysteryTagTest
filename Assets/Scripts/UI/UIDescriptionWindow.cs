using UnityEngine.UI;
using UnityEngine;
using UniRx;
using System;

namespace UI
{
    public class UIDescriptionWindow : MonoBehaviour
    {
        [SerializeField] private Text m_Description, m_ButtonText;
        [SerializeField] private Button m_PlayButton;

        private Action m_Callback;

        public void SetDescription(string s)
        {
            m_Description.text = s;
        }

        public void SetButtonText(string s)
        {
            m_ButtonText.text = s;
        }

        public void SetCallback(Action act)
        {
            m_Callback = act;
        }

        private void OnClick(Unit data)
        {
            if (m_Callback != null) m_Callback();
        }

        private void Awake()
        {
            m_PlayButton
                .OnClickAsObservable()
                .Subscribe(OnClick);
        }
    }
}