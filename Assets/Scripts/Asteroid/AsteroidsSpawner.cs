using UnityEngine;
using UniRx;

namespace Asteroids
{
    public class Spawner
    {
        private ReactiveCollection<Info> m_Enemies;
        private ZoneBorders m_Borders;
        private SpawnerParameters m_Params;
        private Pools.IPool<AsteroidInfoMono> m_Pool;
        private float m_LastPos;

        public Spawner(ReactiveCollection<Info> enemies)
        {
            m_Enemies = enemies;
            
            m_Enemies.ObserveAdd().Subscribe(AddInCollection);
            m_Enemies.ObserveRemove().Subscribe(RemoveAtCollection);
        }

        public Spawner SetParameters(SpawnerParameters param)
        {
            m_Params = param;
            return this;
        }

        public Spawner SetBorders(ZoneBorders borders)
        {
            m_Borders = borders;
            return this;
        }

        public Spawner SetPool(Pools.IPool<AsteroidInfoMono> pool)
        {
            m_Pool = pool;
            return this;
        }

        public void Update()
        {
            if (m_Enemies.Count >= m_Params.MaxCount) return;

            bool allFar = true;
            for (int i = m_Enemies.Count - 1; i >= 0; --i)
            {
                allFar &= m_Enemies[i].Positioning.FarFromBorder;
            }
            if (!allFar) return;

            Vector3 pos = new Vector3(m_Borders.Up, 0f, GetRandomPosition());
            Add(pos);
        }

        private float GetRandomPosition()
        {
            float rnd = Utilities.Rand;
            if (rnd != 0f)
            {
                float sing = Mathf.Abs(rnd - 0.5f) / (rnd - 0.5f);
                if (sing == m_LastPos) rnd = 1f - rnd;
                m_LastPos = sing;
            }
            return rnd * (m_Borders.Right - m_Borders.Left) + m_Borders.Left;
        }
        
        private void Add(Vector3 pos)
        {
            float scale = Utilities.Rand;
            float speed = (1f - scale) * (m_Params.MaxSpeed - m_Params.MinSpeed) + m_Params.MinSpeed;
            Info info = new Info(speed);
            info.Positioning.Position.Value = pos;
            info.Positioning.Scale.Value = (scale + 0.5f) * Vector3.one;
            m_Enemies.Add(info);
        }

        private void AddInCollection(CollectionAddEvent<Info> addEvent)
        {
            Pools.IPooled astView = m_Pool.GetObject();
            astView.Bind(addEvent.Value);
            var pos = addEvent.Value.Positioning;
            pos.Position
                 .Where(CheckExitFromZone)
                 .Select(_ => addEvent.Value)
                 .Subscribe(z => m_Enemies.Remove(z));
            pos.Position
                .Where(_ => !pos.FarFromBorder)
                .Select(_ => pos)
                .Subscribe(CheckFarFromUpperBorder);
            pos.Rotation.Value = Quaternion.Euler(GetRandomAngle(), GetRandomAngle(), GetRandomAngle());
        }

        private void RemoveAtCollection(CollectionRemoveEvent<Info> removeEvent)
        {
            removeEvent.Value.Dispose();
            m_Pool.ReturnObject(removeEvent.Value.AsteroidMono);
        }

        private void CheckFarFromUpperBorder(Positioning pos)
        {
            pos.FarFromBorder = m_Borders.Up - pos.Position.Value.x > m_Params.MinDistance;
        }

        private bool CheckExitFromZone(Vector3 v)
        {
            return v.x < m_Borders.Down;
        }

        private float GetRandomAngle()
        {
            return Utilities.Rand * 360f - 180f;
        }
    }

    [System.Serializable]
    public struct SpawnerParameters
    {
        public int MaxCount;
        public float MinDistance, MinSpeed, MaxSpeed;
    }
}