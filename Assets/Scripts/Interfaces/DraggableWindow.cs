using Assets.Scripts.UI;
using Molodoy.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Molodoy.Interfaces
{
    public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [Header("Optional")]
        [SerializeField] private RectTransform draggableRectTransform;
        [Header("Optional")]
        [SerializeField] private Canvas parentCanvas;

        private void Awake()
        {
            if(draggableRectTransform == null)
            {
                draggableRectTransform = GetComponent<RectTransform>();
            }

            if (parentCanvas == null)
            {
                parentCanvas = transform.FindParentWithComponentOrDefault<Canvas>();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            draggableRectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            draggableRectTransform.SetAsLastSibling();
        }
    }
}