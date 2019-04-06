using System;
using UnityEngine;

namespace Levels
{
    public class LevelGenerator
    {
        private int m_LastAsteroid = -1, m_AsteroidsCount = 1;
        private int[] m_ForAsteroidsRandom;

        public LevelGenerator(int asteroidsCount)
        {
            m_AsteroidsCount = asteroidsCount;
            m_ForAsteroidsRandom = new int[asteroidsCount - 1];
        }

        public AllLevels GetLevels(ISaver saver, Preset[] levelPresets)
        {
            int lastID = -1;
            LevelParameters[] param = new LevelParameters[levelPresets.Length];
            SavingParametersContainer spc;
            bool hasData = saver != null && saver.HasData();
            if (hasData)
            {
                spc = saver.Load();
            }
            else
            {
                spc = new SavingParametersContainer();
                spc.SavingParameters = new SavingParameters[0];
            }

            bool find = false;
            for (int i = 0; i < param.Length; ++i)
            {
                param[i] = new LevelParameters();
                param[i].Preset = levelPresets[i];
                find = false;
                for (int j = 0; j < spc.SavingParameters.Length; ++j)
                {
                    if (spc.SavingParameters[j].ID == param[i].Preset.ID)
                    {
                        param[i].SavedParameters = spc.SavingParameters[j];
                        find = true;
                        break;
                    }
                }
                if (!find) param[i].SavedParameters = GenerateLevel(param[i].Preset.ID);
                if (lastID < param[i].Preset.ID) lastID = param[i].Preset.ID;
            }
            if (!hasData) SetFirstOpen(param);
            return new AllLevels(param, lastID);
        }

        private void SetFirstOpen(LevelParameters[] param)
        {
            if (param.Length == 0) return;
            int first = param[0].Preset.ID, index = 0;
            for (int i = 0; i < param.Length; ++i)
            {
                if (param[i].Preset.ID < first)
                {
                    first = param[i].Preset.ID;
                    index = i;
                }
            }
            param[index].SavedParameters.State.SetOpen();
        }

        private SavingParameters GenerateLevel(int id)
        {
            SavingParameters sp = new SavingParameters
            {
                ID = id,
                AsteroidType = GetRandomNotRepeatAsteroid()
            };
            sp.State.SetClose();
            return sp;
        }

        private int GetRandomNotRepeatAsteroid()
        {
            int r;
            if (m_LastAsteroid == -1)
            {
                r = Utilities.Random.Next(m_AsteroidsCount);
            }
            else
            {
                for (int i = 0; i < m_ForAsteroidsRandom.Length; ++i)
                {
                    m_ForAsteroidsRandom[i] = (i < m_LastAsteroid) ? i : i + 1;
                }
                r = m_ForAsteroidsRandom[Utilities.Random.Next(m_ForAsteroidsRandom.Length)];
            }
            m_LastAsteroid = r;
            return r;
        }
    }
}