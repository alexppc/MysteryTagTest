using System;
using UnityEngine;
using System.Collections.Generic;

namespace UI
{
    public class UIMap : MonoBehaviour
    {
        [SerializeField] private Color m_CloseColor, m_OpenColor, m_CompleteColor, m_FrameActive, m_FrameUnactive;
        [SerializeField] private UIMapIcon[] m_Icons;

        private Dictionary<int, UIMapIcon> m_IconParameters = new Dictionary<int, UIMapIcon>(4);

        public void SetIconState(int levelID, Levels.State state)
        {
            UIMapIcon icon = GetIconByID(levelID);
            if (icon != null)
            {
                if (state.Value != icon.State)
                {
                    icon.State = state.Value;
                    if (state.IsClosed) icon.Icon.color = m_CloseColor;
                    else if (state.IsOpened) icon.Icon.color = m_OpenColor;
                    else if (state.IsCompleted) icon.Icon.color = m_CompleteColor;
                }
            }
        }

        public void SetFrame(int levelID)
        {
            for (int i = 0; i < m_Icons.Length; ++i)
            {
                m_Icons[i].Frame.color = (m_Icons[i].LevelID == levelID) ? m_FrameActive : m_FrameUnactive;
            }
        }

        public void SetClickAction(Action<int> act)
        {
            for (int i = 0; i < m_Icons.Length; ++i)
            {
                m_Icons[i].SetClickAction(act);
            }
        }

        private UIMapIcon GetIconByID(int id)
        {
            UIMapIcon icon = null;
            m_IconParameters.TryGetValue(id, out icon);
            return icon;
        }

        private void Awake()
        {
            for (int i = 0; i < m_Icons.Length; ++i)
            {
                m_IconParameters.Add(m_Icons[i].LevelID, m_Icons[i]);
            }
        }
    }
}