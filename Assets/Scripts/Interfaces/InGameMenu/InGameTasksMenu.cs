using Molodoy.CoreComponents;
using QuantumTek.QuantumQuest;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Molodoy.Interfaces
{
    [DisallowMultipleComponent]
    public class InGameTasksMenu : MonoBehaviour
    {
        [SerializeField] private GameObject challengeEndElements;
        [SerializeField] private GameObject defaultElements;
        [SerializeField] private RectTransform questTilesParent;
        [SerializeField] private RectTransform tasksTilesParent;
        [SerializeField] private QQ_QuestHandler questHandler;
        [SerializeField] private GameObject questTilePrefab;
        [SerializeField] private GameObject taskTilePrefab;
        [SerializeField] private Text questDescription;
        private string selectedQuestName;

        private void Awake()
        {
            challengeEndElements.SetActive(false);
            defaultElements.SetActive(true);

            if (questHandler == null || questHandler.questDB == null)
            {
                Debug.LogWarning("You try to open quests menu, but quest handler or DB not found!");
                CloseWindow();
                return;
            }
            else
            {
                foreach (QQ_QuestSO questSO in questHandler.questDB.Quests)
                {
                    questHandler.AssignAndActivateQuest(questSO.Quest.Name);
                }
            }
        }

        private void OnEnable()
        {
            if (questHandler == null || questHandler.questDB == null)
            {
                CloseWindow();
                return;
            }

            CursorManager.SetCursorState(GetHashCode(), true, CursorLockMode.Confined);
            GameProcess.FreezeGame(GetHashCode());
            DrawAllQuests();
        }

        private void OnDisable()
        {
            CursorManager.ForgetCursorState(GetHashCode());
            GameProcess.UnFreezeGame(GetHashCode());
            ClearQuestsPreview();
            ClearTasksPreview();

        }

        public void DrawAllQuests()
        {
            if (questHandler.questDB.Quests.Count != 0)
            {
                List<QQ_QuestSO> questsList = questHandler.questDB.Quests;

                for (int i = 0; i < questsList.Count; i++)
                {
                    QuestInterfaceTile questTile = Instantiate(questTilePrefab, questTilesParent, false).GetComponent<QuestInterfaceTile>();
                    questTile.SetQuest(questsList[i].Quest);
                    questTile.OnLeftClick.AddListener(OnQuestSelected);

                    if (i == 0)
                    {
                        questTile.OnSelected();
                        OnQuestSelected(questsList[i].Quest.Name);
                    }
                }
            }
        }

        public void OnQuestSelected(string questName)
        {
            if (selectedQuestName == questName) { return; }
            selectedQuestName = questName;
            ClearTasksPreview();

            QQ_Quest selectedQuest = questHandler.GetQuest(questName);
            questDescription.text = selectedQuest.Description;

            foreach (QQ_Task task in selectedQuest.Tasks)
            {
                TaskInterfaceTile taskTile = Instantiate(taskTilePrefab, tasksTilesParent, false).GetComponent<TaskInterfaceTile>();
                taskTile.SetTask(task);
            }
        }

        public void ClearTasksPreview()
        {
            for (int i = 0; i < tasksTilesParent.childCount; i++)
            {
                Destroy(tasksTilesParent.GetChild(i).gameObject);
            }
        }

        public void ClearQuestsPreview()
        {
            for (int i = 0; i < questTilesParent.childCount; i++)
            {
                Destroy(questTilesParent.GetChild(i).gameObject);
            }

            selectedQuestName = "";
        }

        public void ChallengeEndMode()
        {
            challengeEndElements.SetActive(true);
            defaultElements.SetActive(false);
        }

        public void ExitToMainMenu()
        {
            //CursorManager.ForgetCursorState(GetHashCode());
            //GameProcess.UnFreezeGame(GetHashCode());
            SceneTransition.SwitchToScene(GameConstants.Scene_MainMenuName);
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}