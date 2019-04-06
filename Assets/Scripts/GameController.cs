using UnityEngine;
using Levels;
using UniRx;

public interface IGameState
{
    void SetGameState(bool state);
}

[System.Serializable]
public struct ZoneBorders
{
    public float Left, Right, Up, Down;
}

public class GameController : MonoBehaviour
{
    public readonly FloatReactiveProperty NormalizedTimer = new FloatReactiveProperty();
    public ZoneBorders ZoneBorders { get { return m_Borders; } }
    public AllLevels Levels { get { return m_AllLevels; } }

    [SerializeField] private ZoneBorders m_Borders;
    [SerializeField] private GameObject[] m_AsteroidPrefabs;
    [SerializeField] private Preset[] m_LevelPresets;

    private int m_SelectedLevel;
    private LevelParameters m_CurrentLevel = null;
    private ISaver m_Saver = new PlayerPrefsSaver();
    private AllLevels m_AllLevels;
    private Pools.IPool<Asteroids.AsteroidInfoMono> m_AsteroidsPool;
    private IGameState[] m_StateComponents;
    private IReset[] m_ResetComponents;
    private float m_Timer = 0f;
    private bool m_GameState = false;


    public void SetSelectedLevel(int level)
    {
        m_SelectedLevel = level;
    }

    public void StartLevel()
    {
        CommonComponents.PlayerManager.Reset();
        m_CurrentLevel = m_AllLevels.GetLevelByID(m_SelectedLevel);
        if (m_CurrentLevel == null)
        {
            //error
            return;
        }
        CommonComponents.AsteroidsManager.SetSpawnerParameters(m_CurrentLevel.Preset.AsteroidSpawn);
        CommonComponents.BulletsManager.SetSpawnParameters(m_CurrentLevel.Preset.BulletSpawn);
        CommonComponents.UserInput.SetNewShootTime(m_CurrentLevel.Preset.ShootTime);
        m_AsteroidsPool.ChangePrefab(m_AsteroidPrefabs[m_CurrentLevel.SavedParameters.AsteroidType]);
        NormalizedTimer.Value = 0f;
        m_Timer = 0f;

        CommonComponents.ControllerUI.SetActiveGameUI();
        SetGameState(true);
    }

    public void SetGameState(bool state)
    {
        m_GameState = state;
        for (int i = 0; i < m_StateComponents.Length; ++i)
        {
            m_StateComponents[i].SetGameState(state);
        }
        if (!state) Reset();
    }

    private void Reset()
    {
        for (int i = 0; i < m_ResetComponents.Length; ++i)
        {
            m_ResetComponents[i].Reset();
        }
    }

    private void Awake()
    {
        var levGen = new LevelGenerator(m_AsteroidPrefabs.Length);
        m_AllLevels = levGen.GetLevels(m_Saver, m_LevelPresets);

        NormalizedTimer
            .Where(x => x >= 1f)
            .Subscribe(LevelComplete);
    }

    private void Start()
    {
        m_AsteroidsPool = CommonComponents.PoolManager.GetPool<Asteroids.AsteroidInfoMono>();

        m_StateComponents = new IGameState[]
        {
            CommonComponents.AsteroidsManager,
            CommonComponents.BulletsManager,
            CommonComponents.PlayerManager,
            CommonComponents.UserInput
        };
        m_ResetComponents = new IReset[]
        {
            CommonComponents.AsteroidsManager
        };

        CommonComponents.PlayerManager.Info.Health.Life
            .Where(x => x <= 0)
            .Subscribe(LevelLose);
    }

    private void Update()
    {
        if (!m_GameState) return;

        LevelTimer();
    }

    private void LevelTimer()
    {
        m_Timer += Time.deltaTime;
        if (m_CurrentLevel != null) NormalizedTimer.Value = Mathf.Clamp01(m_Timer / m_CurrentLevel.Preset.Time);
    }

    private bool TimerCheck(float time)
    {
        return (m_CurrentLevel != null) ? time >= m_CurrentLevel.Preset.Time : false;
    }

    private void LevelComplete(float x)
    {
        SetGameState(false);
        m_CurrentLevel.SavedParameters.State.SetComplete();
        LevelParameters next = m_AllLevels.GetNextLevelByID(m_CurrentLevel.Preset.ID);
        if (next != null)
        {
            next.SavedParameters.State.SetOpen();
        }
        SaveLevels();
        CommonComponents.ControllerUI.ShowWinScreen.Execute(true);
    }

    private void LevelLose(int x)
    {
        SetGameState(false);
        CommonComponents.ControllerUI.ShowWinScreen.Execute(false);
    }

    private void SaveLevels()
    {
        SavingParametersContainer spc = new SavingParametersContainer();
        spc.SavingParameters = new SavingParameters[m_AllLevels.Parameters.Length];
        for (int i = 0; i < spc.SavingParameters.Length; ++i)
        {
            spc.SavingParameters[i] = m_AllLevels.Parameters[i].SavedParameters;
        }
        m_Saver.Save(spc);
    }
}

