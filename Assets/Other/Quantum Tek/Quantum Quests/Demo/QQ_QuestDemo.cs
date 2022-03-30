﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QuantumTek.QuantumQuest.Demo
{
    public class QQ_QuestDemo : MonoBehaviour
    {
        [Header("Object References")]
        public QQ_QuestHandler handler;
        public TextMeshProUGUI questName;
        public TextMeshProUGUI mainTaskName;
        public TextMeshProUGUI smallerTask1Name;
        public TextMeshProUGUI smallerTask2Name;

        private QQ_Quest quest;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            foreach (QQ_QuestSO questSO in handler.questDB.Quests)
            {
                Debug.Log(questSO.Quest.Name);
                quest = handler.AssignQuest(questSO.Quest.Name);
                quest.ActivateQuest();
                questName.text = quest.Name;
                mainTaskName.text = quest.Tasks[0].Name;
                QQ_Task mainTask = handler.GetTask(questSO.Quest.Name, quest.GetTask(quest.FirstTasks[0]).Name);
                int taskID = mainTask.NextTasks[0]; // Get the ID of the next task
                smallerTask1Name.text = quest.GetTask(taskID).Name; // Display the name
                taskID = mainTask.NextTasks[1]; // Get the ID of the next task
                smallerTask2Name.text = handler.GetTask("Bob Wants Tomatoes", taskID).Name; // Display the name
            }
        }

        public void Buy()
        {
            // Exit if the quest was already completed
            if (quest.Completed)
                return;

            // Progress the buy tomatoes task by 3 tomatoes, therefore completing it. Because the choice was to buy or steal, the get tomatoes task is now also complete, and therefore the quest
            handler.ProgressTask("Bob Wants Tomatoes", "Get Tomatoes for Bob", 3);
            handler.ProgressTask("Bob Wants Tomatoes", "Buy Tomatoes", 3);

            // Color the quest text to show completion
            questName.color = mainTaskName.color = smallerTask1Name.color = new Color(55 / 255f, 205 / 255f, 55 / 255f);
        }

        public void Steal()
        {
            // Exit if the quest was already completed
            if (quest.Completed)
                return;

            // Progress the steal tomatoes task by 3 tomatoes, therefore completing it. Because the choice was to buy or steal, the get tomatoes task is now also complete, and therefore the quest
            handler.ProgressTask("Bob Wants Tomatoes", "Get Tomatoes for Bob", 3);
            handler.ProgressTask("Bob Wants Tomatoes", "Steal Tomatoes", 3);

            // Color the quest text to show completion
            questName.color = mainTaskName.color = smallerTask2Name.color = new Color(55 / 255f, 205 / 255f, 55 / 255f);
        }
    }
}