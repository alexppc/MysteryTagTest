using UniRx;
using UnityEngine;

namespace Bullets
{
    public class Spawner
    {
        private ReactiveCollection<Info> m_Bullets;
        private ReactiveCommand<int> m_Command;
        private SpawnerParameters m_Parameters;
        private ZoneBorders m_Borders;
        private Transform m_SpawnPoint;
        private Pools.IPool<BulletInfoMono> m_Pool;

        public Spawner(ReactiveCollection<Info> bullets)
        {
            m_Bullets = bullets;

            m_Bullets
                .ObserveAdd()
                .Subscribe(AddInCollection);
            m_Bullets
                .ObserveRemove()
                .Subscribe(RemoveAtCollection);
        }

        public Spawner SetParameters(SpawnerParameters param)
        {
            m_Parameters = param;
            return this;
        }

        public Spawner SetBorders(ZoneBorders borders)
        {
            m_Borders = borders;
            return this;
        }

        public Spawner SetCommand(IUserInput input)
        {
            m_Command = input.CharacterShoot;
            m_Command
                .Subscribe(Shoot);
            return this;
        }

        public Spawner SetSpawnPoint(Transform point)
        {
            m_SpawnPoint = point;
            return this;
        }

        public Spawner SetPool(Pools.IPool<BulletInfoMono> pool)
        {
            m_Pool = pool;
            return this;
        }

        private void AddInCollection(CollectionAddEvent<Info> addEvent)
        {
            Pools.IPooled astView = m_Pool.GetObject();
            astView.Bind(addEvent.Value);
            var pos = addEvent.Value.Positioning;
            pos.Position
                 .Where(CheckExitFromZone)
                 .Select(_ => addEvent.Value)
                 .Subscribe(z => m_Bullets.Remove(z));
        }

        private void RemoveAtCollection(CollectionRemoveEvent<Info> removeEvent)
        {
            removeEvent.Value.Dispose();
            m_Pool.ReturnObject(removeEvent.Value.BulletMono);
        }

        private bool CheckExitFromZone(Vector3 pos)
        {
            return pos.x > m_Borders.Up;
        }

        private void Shoot(int id)
        {
            if (id == 0) return;

            Info info = new Info(m_Parameters.Speed);
            info.Positioning.Position.Value = m_SpawnPoint.position;
            m_Bullets.Add(info);
        }
    }

    [System.Serializable]
    public struct SpawnerParameters
    {
        public float Speed;
    }
}