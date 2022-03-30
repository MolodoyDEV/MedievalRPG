using QuantumTek.QuantumQuest;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Molodoy.Interfaces
{
    public class QuestInterfaceTile : Button
    {
        public UnityEvent<string> OnLeftClick = new UnityEvent<string>();
        private QQ_Quest quest = new QQ_Quest(-1);
        private Text questName;
        private Image background;

        protected override void Awake()
        {
            base.Awake();
            questName = GetComponentInChildren<Text>();
            background = GetComponent<Image>();
        }

        public void SetQuest(QQ_Quest _quest)
        {
            quest = _quest;
            quest.QuestCompleted.AddListener(UpdateQuestInfo);
            quest.QuestFailed.AddListener(UpdateQuestInfo);
            UpdateQuestInfo();
        }

        public void OnLeftClickHandled()
        {
            OnLeftClick?.Invoke(questName.text);
            OnSelected();
        }

        public void OnSelected()
        {
            Select();
        }

        public void OnDeselected()
        {
            background.color = colors.normalColor;
        }

        private void UpdateQuestInfo()
        {
            questName.text = quest.Name;

            //if (quest.Status == QQ_QuestStatus.Failed)
            //{
            //    background.color = Color.red;
            //}
            //else if (quest.Status == QQ_QuestStatus.Completed)
            //{
            //    background.color = Color.green;
            //}
            //else if (quest.Status == QQ_QuestStatus.Active)
            //{
            //    background.color = Color.blue;
            //}
            //else if (quest.Status == QQ_QuestStatus.Inactive)
            //{
            //    background.color = Color.gray;
            //}
        }
    }
}