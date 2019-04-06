using UnityEngine;
using UniRx;

namespace Bullets
{
    public class BulletsManager : MonoBehaviour, IGameState
    {
        private ReactiveCollection<Info> m_Bullets;
        private Spawner m_Spawner;
        private MovementController m_MovementController;
        private AttackController m_AttackController;
        private bool m_Active = false;
        Pools.IPool<BulletInfoMono> m_Pool;

        
        public void SetGameState(bool state)
        {
            m_Active = state;
            if (!state) AllReturnInPool();
        }

        public void SetSpawnParameters(SpawnerParameters param)
        {
            m_Spawner.SetParameters(param);
        }

        private void AllReturnInPool()
        {
            for (int i = m_Bullets.Count - 1; i >= 0; --i)
            {
                m_Bullets.RemoveAt(i);
            }
        }

        private void Awake()
        {
            m_Bullets = new ReactiveCollection<Info>();
            m_Spawner = new Spawner(m_Bullets);
            m_MovementController = new MovementController(m_Bullets);
            m_AttackController = new AttackController(m_Bullets);
        }

        private void Start()
        {
            m_Pool = CommonComponents.PoolManager.GetPool<BulletInfoMono>();

            m_Spawner
                .SetBorders(CommonComponents.GameController.ZoneBorders)
                .SetSpawnPoint(CommonComponents.PlayerManager.Info.PlayerInfo.BulletsSpawnPoint)
                .SetCommand(CommonComponents.UserInput)
                .SetPool(m_Pool);
            m_AttackController
                .SetAsteroidHealthController(CommonComponents.AsteroidsManager.HealthManager);
        }

        private void Update()
        {
            if (!m_Active) return;
            m_MovementController.Update(Time.deltaTime);
        }
    }
}