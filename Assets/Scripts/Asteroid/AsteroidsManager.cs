using UnityEngine;
using UniRx;

namespace Asteroids
{
    public class AsteroidsManager : MonoBehaviour, IGameState, IReset
    {
        public HealthController HealthManager { get { return m_Health; } }

        //не нашел как выставить изначальную размерность
        private ReactiveCollection<Info> m_Enemies;
        private MovementController m_Movement;
        private Spawner m_Spawner;
        private AttackController m_Attack;
        private HealthController m_Health;
        private bool m_Active = false;
        private Pools.IPool<AsteroidInfoMono> m_Pool;


        public void SetSpawnerParameters(SpawnerParameters param)
        {
            m_Spawner.SetParameters(param);
        }
        
        public void SetGameState(bool state)
        {
            m_Active = state;
            if (!state) AllReturnInPool();
        }
        
        public void Reset()
        {
            AllReturnInPool();
            m_Pool.Clear();
        }

        private void AllReturnInPool()
        {
            for (int i = m_Enemies.Count - 1; i >= 0; --i)
            {
                m_Enemies.RemoveAt(i);
            }
        }

        private void Awake()
        {
            m_Enemies = new ReactiveCollection<Info>();
            m_Movement = new MovementController(m_Enemies);
            m_Spawner = new Spawner(m_Enemies);
            m_Health = new HealthController(m_Enemies);
            m_Attack = new AttackController(m_Enemies);
        }

        private void Start()
        {
            m_Pool = CommonComponents.PoolManager.GetPool<AsteroidInfoMono>();

            m_Spawner
                .SetBorders(CommonComponents.GameController.ZoneBorders)
                .SetPool(m_Pool);
            m_Attack
                .SetPlayerHelathController(CommonComponents.PlayerManager.HealtController)
                .SetAsteroidHealthController(m_Health);
        }

        private void Update()
        {
            if (!m_Active) return;
            m_Movement.Update(Time.deltaTime);
            m_Spawner.Update();
        }
    }
}