using UnityEngine.UI;
using UnityEngine;
using System;
using UniRx;

namespace UI
{
    public class UIWinLoseScreen : MonoBehaviour
    {
        [SerializeField] private GameObject m_Screen;
        [SerializeField] private Text m_Message = null, m_ButtonText = null;
        [SerializeField] private Button m_Button = null;

        private Action m_Callback;

        private string m_ButtonWinText = "Ничего сложного", m_ButtonLoseText = "Поплакать",
            m_WinText = "Мой капитан, это было ШЕДЕВРАЛЬНО! ВЕЛИКОЛЕПНО! БОЖЕСТВЕННО!", m_LoseText = "Это провал. И кстати, капитан, я увольняюсь";

        public void Show(bool win)
        {
            m_ButtonText.text = (win) ? m_ButtonWinText : m_ButtonLoseText;
            m_Message.text = (win) ? m_WinText : m_LoseText;
            m_Screen.SetActive(true);
        }

        public void Hide()
        {
            m_Screen.SetActive(false);
        }

        public void SetMessage(string s)
        {
            m_Message.text = s;
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
            m_Button
                .OnClickAsObservable()
                .Subscribe(OnClick);
        }
    }
}