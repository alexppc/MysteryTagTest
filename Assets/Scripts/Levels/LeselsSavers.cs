using System;
using UnityEngine;

namespace Levels
{
    public interface ISaver
    {
        bool HasData();
        SavingParametersContainer Load();
        void Save(SavingParametersContainer save);
    }

    public class PlayerPrefsSaver : ISaver
    {
        private const string ID = "SAVEDATA";

        public bool HasData()
        {
            return PlayerPrefs.HasKey(ID);
        }

        public SavingParametersContainer Load()
        {
            if (!HasData()) return null;
            var s = PlayerPrefs.GetString(ID);
            return JsonUtility.FromJson<SavingParametersContainer>(s);
        }

        public void Save(SavingParametersContainer save)
        {
            var s = JsonUtility.ToJson(save);
            PlayerPrefs.SetString(ID, s);
        }
    }
}