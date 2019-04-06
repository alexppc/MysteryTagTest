using UnityEngine;
using UniRx;

namespace UI
{
    public class ControllerUI : MonoBehaviour
    {
        public readonly ReactiveCommand<bool> ShowWinScreen = new ReactiveCommand<bool>();

        [SerializeField] private ControllerUIGame m_ControllerUIGame;
        [SerializeField] private ControllerUIMap m_ControllerUIMap;
        [SerializeField] private ControllerUIUserInput m_ControllerUIUserInput;
        

        public void SetActiveMapUI()
        {
            m_ControllerUIGame.SetActiveUI(false);
            m_ControllerUIMap.SetActiveUI(true);
        }

        public void SetActiveGameUI()
        {
            m_ControllerUIGame.SetActiveUI(true);
            m_ControllerUIMap.SetActiveUI(false);
        }

        private void Awake()
        {
            m_ControllerUIGame.SetWinScreenObserver(ShowWinScreen);
        }

        private void Start()
        {
            m_ControllerUIUserInput.BindInputController(CommonComponents.UserInput);
            m_ControllerUIMap.SetLevels(CommonComponents.GameController.Levels);
            SetActiveMapUI();
        }
    }
}