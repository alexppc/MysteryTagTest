using UnityEngine;
using UniRx;

namespace UI
{
    public class ControllerUIGame : MonoBehaviour
    {
        [SerializeField] private UIGameUI m_GameUI;



        public void SetActiveUI(bool state)
        {
            m_GameUI.gameObject.SetActive(state);
            if (state) m_GameUI.WinLoseScreen.Hide();
        }

        public void SetWinScreenObserver(ReactiveCommand<bool> comm)
        {
            comm.Subscribe(m_GameUI.WinLoseScreen.Show);
        }

        private void Awake()
        {
            m_GameUI.WinLoseScreen.SetCallback(OnWinScreenClose);
        }

        private void Start()
        {
            CommonComponents.PlayerManager.Info.Health.Life.Subscribe(m_GameUI.GameScreen.SetHP);
            CommonComponents.GameController.NormalizedTimer.Subscribe(m_GameUI.GameScreen.SetProgress);
        }

        private void OnWinScreenClose()
        {
            CommonComponents.ControllerUI.SetActiveMapUI();
        }
    }
}