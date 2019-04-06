using UnityEngine;
using UniRx.Triggers;
using UniRx;

namespace Player
{
    public interface IPlayerMovement : IReset
    {
        void Bind(Positioning pos);
    }
    public interface IPlayerColor : IReset
    {
        void Bind(Health health);
    }

    public class PlayerInfoMono : MonoBehaviour
    {
        public Transform BulletsSpawnPoint { get { return m_BulletsSpawnPoint; } }

        [SerializeField] private Transform m_BulletsSpawnPoint;

        public void Bind(Info info)
        {
            info.PlayerInfo = this;
            GetComponent<IPlayerMovement>().Bind(info.Positioning);
            GetComponent<IPlayerColor>().Bind(info.Health);
        }
    }
}