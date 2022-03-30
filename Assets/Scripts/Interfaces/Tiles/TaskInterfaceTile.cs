using QuantumTek.QuantumQuest;
using UnityEngine;
using UnityEngine.UI;

namespace Molodoy.Interfaces
{
    public class TaskInterfaceTile : MonoBehaviour
    {
        [SerializeField] private Image taskImage;
        [SerializeField] private QQ_Task task = new QQ_Task(-1);
        private Text taskInfo;

        private void Awake()
        {
            taskInfo = GetComponentInChildren<Text>();
        }

        public void SetTask(QQ_Task _task)
        {
            task = _task;
            task.ProgressIncreased.AddListener(UpdateTaskInfo);
            task.TaskCompleted.AddListener(UpdateTaskInfo);
            UpdateTaskInfo();
        }

        private void UpdateTaskInfo()
        {
            if (task.Completed)
            {
                taskImage.color = Color.green;
            }
            else
            {
                taskImage.color = Color.yellow;
            }

            taskInfo.text = $"{task.Progress}/{task.MaxProgress} {task.Name} | Optional: {task.Optional}\n{task.Description}";
        }
    }
}