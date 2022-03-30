using System.Collections.Generic;

namespace Molodoy.Extensions
{
    public interface IQueueable
    {
        int Hash { get; }
        public void TurnHasCome(int _hash);
    }

    public class CustomQueue
    {
        public int Count { get => queue.Count; }
        private List<IQueueable> queue = new List<IQueueable>();
        private IQueueable baseElement;

        public void InitializeQueue(IQueueable newBaseElement)
        {
            baseElement = newBaseElement;
            ClearQueue();
        }

        public void AddToQueueAndApply(IQueueable queueable)
        {
            queue.Add(queueable);
            ApplyQueue(queueable.Hash);
        }

        /// <summary>
        /// Removes all instances who match with hashcode and apply it
        /// </summary>
        /// <param name="hashCode"></param>
        public void RemoveStateFromQueueAll(int hashCode)
        {
            IQueueable lastElement = queue.GetLastElementOrDefault();

            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].Hash == hashCode)
                {
                    queue.RemoveAt(i);
                    i--;
                }
            }

            if (lastElement?.GetHashCode() != queue.GetLastElementOrDefault()?.GetHashCode())
            {
                ApplyQueue(hashCode);
            }
        }

        /// <summary>
        /// Removes last instances who match with hashcode and apply it
        /// </summary>
        /// <param name="hashCode"></param>
        public void RemoveLastStateFromQueue(int hashCode)
        {
            IQueueable lastElement = queue.GetLastElementOrDefault();

            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].Hash == hashCode)
                {
                    queue.RemoveAt(i);
                    break;
                }
            }

            if (lastElement?.GetHashCode() != queue.GetLastElementOrDefault()?.GetHashCode())
            {
                ApplyQueue(hashCode);
            }
        }

        private void ApplyQueue(int hashCode)
        {
            if (queue.Count == 0)
            {
                baseElement.TurnHasCome(hashCode);
            }
            else
            {
                queue.GetLastElementOrDefault().TurnHasCome(hashCode);
            }
        }

        public void ClearQueue()
        {
            queue.Clear();
            ApplyQueue(baseElement.Hash);
        }
    }
}