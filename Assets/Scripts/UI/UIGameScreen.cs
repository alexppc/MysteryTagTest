using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class UIGameScreen : MonoBehaviour
    {
        [SerializeField] private Text m_HP;
        [SerializeField] private Slider m_Progress;

        public void SetHP(int value)
        {
            m_HP.text = string.Concat("HP: ", value.ToString());
        }

        public void SetProgress(float value)
        {
            m_Progress.value = value;
        }
    }
}