using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour, IGameState, IReset
    {
        public Info Info { get { return m_PlayerInfo; } }
        public PlayerHealthController HealtController { get { return m_HealthController;} }
        
        [SerializeField] private PlayerInfoMono m_PlayerInfoMono = null;
        [SerializeField] private MovementParametersMono m_MovingParameters;

        private Info m_PlayerInfo;
        private PlayerMovementController m_MoveController;
        private PlayerHealthController m_HealthController;
        private bool m_GameState = false;


        public void SetGameState(bool state)
        {
            m_GameState = state;
        }

        public void Reset()
        {
            m_PlayerInfo.Reset();
            m_MoveController.Reset();
        }

        private void Awake()
        {
            m_PlayerInfo = new Info();
            m_PlayerInfoMono.Bind(m_PlayerInfo);
            m_HealthController = new PlayerHealthController();
            m_MoveController = new PlayerMovementController();
        }

        private void Start()
        {

            var borders = CommonComponents.GameController.ZoneBorders;
            m_MoveController
                .SetParameters(new MovementParameters(m_MovingParameters, borders.Left, borders.Right))
                .SetInput(CommonComponents.UserInput)
                .SetPlayer(m_PlayerInfo);
            m_HealthController
                .SetPlayer(m_PlayerInfo);
        }

        private void Update()
        {
            if (!m_GameState) return;
            m_MoveController.Update(Time.deltaTime);
        }
    }
}