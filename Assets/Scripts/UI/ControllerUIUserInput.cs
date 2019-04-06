using UniRx;
using UnityEngine;
using UniRx.Triggers;

namespace UI
{
    public class ControllerUIUserInput : MonoBehaviour
    {
        [SerializeField] private UIUserInput m_UserInput;

        private BoolReactiveProperty m_Shoot = new BoolReactiveProperty(false);
        private FloatReactiveProperty m_Move = new FloatReactiveProperty(0f);

        public void BindInputController(IUIInput controller)
        {
            m_UserInput.Shoot.DownTrigger
                .OnPointerDownAsObservable()
                .Subscribe(_ => m_Shoot.Value = true);
            m_UserInput.Shoot.UpTrigger
                .OnPointerUpAsObservable()
                .Subscribe(_ => m_Shoot.Value = false);
            m_UserInput.Left.DownTrigger
                .OnPointerDownAsObservable()
                .Subscribe(_ => m_Move.Value = -1f);
            m_UserInput.Left.UpTrigger
                .OnPointerUpAsObservable()
                .Where(_ => m_Move.Value == -1f)
                .Subscribe(_ => m_Move.Value = 0f);
            m_UserInput.Right.DownTrigger
                .OnPointerDownAsObservable()
                .Subscribe(_ => m_Move.Value = 1f);
            m_UserInput.Right.UpTrigger
                .OnPointerUpAsObservable()
                .Where(_ => m_Move.Value == 1f)
                .Subscribe(_ => m_Move.Value = 0f);

            controller.SetUIMovementProperty(m_Move);
            controller.SetUIShootProperty(m_Shoot);
        }
    }
}