using UnityEngine;

public class CommonComponents : MonoBehaviour
{
    public static GameController GameController;
    [SerializeField] private GameController m_GameController = null;

    public static UserInput UserInput;
    [SerializeField] private UserInput m_UserInput = null;

    public static Player.PlayerManager PlayerManager;
    [SerializeField] private Player.PlayerManager m_PlayerManager = null;

    public static Pools.PoolManager PoolManager;
    [SerializeField] private Pools.PoolManager m_PoolManager = null;

    public static Bullets.BulletsManager BulletsManager;
    [SerializeField] private Bullets.BulletsManager m_BulletsManager;

    public static Asteroids.AsteroidsManager AsteroidsManager;
    [SerializeField] private Asteroids.AsteroidsManager m_AsteroidsManager;

    public static UI.ControllerUI ControllerUI;
    [SerializeField] private UI.ControllerUI m_ControllerUI;

    private void Awake()
    {
        GameController = m_GameController;
        UserInput = m_UserInput;
        PlayerManager = m_PlayerManager;
        PoolManager = m_PoolManager;
        BulletsManager = m_BulletsManager;
        AsteroidsManager = m_AsteroidsManager;
        ControllerUI = m_ControllerUI;
    }
}
