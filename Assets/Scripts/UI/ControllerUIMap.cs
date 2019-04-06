using UnityEngine;
using System.Collections.Generic;

namespace UI
{
    public class ControllerUIMap : MonoBehaviour
    {
        [SerializeField] private UIMapUI m_MapUI;

        private Levels.AllLevels m_Levels;
        private Descriptions m_Descriptions = new Descriptions();

        private string m_ButtonPlay = "Полетели!", m_ButtonReplay = "Ещё! Хочу ещё!";

        public void SetLevels(Levels.AllLevels levels)
        {
            m_Levels = levels;
        }

        public void SetActiveUI(bool state)
        {
            m_MapUI.gameObject.SetActive(state);
            if (state) SetMap();
        }

        private void Awake()
        {
            m_MapUI.Description.SetCallback(OnClickPlay);
            m_MapUI.Map.SetClickAction(OnLevelSelected);
        }

        private void OnClickPlay()
        {
            CommonComponents.GameController.StartLevel();
        }

        private void OnLevelSelected(int id)
        {
            var lvl = m_Levels.GetLevelByID(id);
            if (lvl != null && !lvl.SavedParameters.State.IsClosed)
            {
                CommonComponents.GameController.SetSelectedLevel(id);
                SetDescription(id, lvl.SavedParameters.State.IsCompleted);
                m_MapUI.Map.SetFrame(id);
            }
        }

        private void SetDescription(int id, bool replay)
        {
            m_MapUI.Description.SetButtonText((!replay) ? m_ButtonPlay : m_ButtonReplay);
            m_MapUI.Description.SetDescription(m_Descriptions.GetByLevelID(id));
        }

        private void SetMap()
        {
            Levels.LevelParameters[] levels = m_Levels.Parameters;
            int last = -1;
            for (int i = 0; i < levels.Length; ++i)
            {
                m_MapUI.Map.SetIconState(levels[i].Preset.ID, levels[i].SavedParameters.State);
                if (!levels[i].SavedParameters.State.IsClosed && levels[i].Preset.ID > last) last = levels[i].Preset.ID;
            }
            if (last != -1) OnLevelSelected(last);
        }
    }

    public class Descriptions
    {
        private Dictionary<int, string> m_Descriptions = new Dictionary<int, string>
        {
            {1, "Здравия желаю, капитан! Как же Вас забросило так далеко? Ладно, смысл простой - нужно пролететь это квадрант и постараться не угробить корабль - астероид на астероиде астероидом погоняет"},
            {2, "Хм, какие-то необычные частицы я наблюдаю в этом обычно вакууме. Из-за него мы летим медленнее, а метеоритам - хоть бы хны. И да, что-то не так с оружием - оно перегревается и стреляет реже" },
            {3, "Черт подери! Система охлаждения почти отказала! Оружие еле стреляет, а мы летим всё медленнее. Если мы выберемся из это космической дыры - точно начну заниматься зарядкой по утрам."},
            {4, "КАПИТАН! ОРУЖИЕ ПОЧТИ ОТКАЗАЛО! А МЫ ПЛЕТЁМСЯ МЕДЛЕННЕЕ БЕРЕМЕННОЙ ЧЕРЕПАХИ! Откуда тут столько этой пыли, которая нас тормозит?!. Ладно, пойду хоть поем напоследок..."}
        };

        public string GetByLevelID(int id)
        {
            return m_Descriptions[id];
        }
    }
}