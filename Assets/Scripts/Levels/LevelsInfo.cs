using UnityEngine;
using System;

namespace Levels
{
    public class AllLevels
    {
        public LevelParameters[] Parameters;

        private int m_LastLevelID;

        public AllLevels(LevelParameters[] parameters, int lastLevelID)
        {
            Parameters = parameters;
            m_LastLevelID = lastLevelID;
        }

        public LevelParameters GetLevelByID(int id)
        {
            for (int i = 0; i < Parameters.Length; ++i)
            {
                if (Parameters[i].Preset.ID == id) return Parameters[i];
            }
            return null;
        }

        public LevelParameters GetNextLevelByID(int id)
        {
            if (id == m_LastLevelID) return null;
            int next = m_LastLevelID;
            LevelParameters lvl = null;
            for (int i = 0; i < Parameters.Length; ++i)
            {
                if (Parameters[i].Preset.ID > id && Parameters[i].Preset.ID <= next)
                {
                    lvl = Parameters[i];
                    next = Parameters[i].Preset.ID;
                }
            }
            return lvl;
        }
    }

    [Serializable]
    public class SavingParametersContainer
    {
        public SavingParameters[] SavingParameters;
    }

    public class LevelParameters
    {
        public Preset Preset;
        public SavingParameters SavedParameters;
    }

    [Serializable]
    public struct Preset
    {
        public int ID;
        public float Time, ShootTime;
        public Asteroids.SpawnerParameters AsteroidSpawn;
        public Bullets.SpawnerParameters BulletSpawn;
    }

    [Serializable]
    public struct SavingParameters
    {
        public int ID;
        public State State;
        public int AsteroidType;
    }

    [Serializable]
    public struct State
    {
        public int Value { get { return m_State; } }
        public bool IsClosed { get { return m_State == CLOSED; } }
        public bool IsOpened { get { return m_State == OPENED; } }
        public bool IsCompleted { get { return m_State == COMPLETED; } }

        [SerializeField] private int m_State;
        public const int CLOSED = 1, OPENED = 2, COMPLETED = 3;

        public void SetOpen()
        {
            m_State = OPENED;
        }

        public void SetClose()
        {
            m_State = CLOSED;
        }

        public void SetComplete()
        {
            m_State = COMPLETED;
        }
    }
}