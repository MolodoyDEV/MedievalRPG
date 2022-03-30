using Molodoy.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Molodoy.Interfaces
{
    public class ResizeableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform scaleableRectTransform;
        [Header("Optional")]
        [SerializeField] private Canvas parentCanvas;
        [SerializeField] private Vector2 minimumScale = new Vector2();
        private RectTransform parentCanvasTransform;

        private void Awake()
        {
            if (parentCanvas == null)
            {
                parentCanvas = transform.FindParentWithComponentOrDefault<Canvas>();
            }

            parentCanvasTransform = parentCanvas.GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            scaleableRectTransform.localScale += new Vector3(eventData.delta.x, -eventData.delta.y) /
                (parentCanvasTransform.rect.width - parentCanvasTransform.rect.height);

            if (scaleableRectTransform.localScale.x < minimumScale.x)
            {
                scaleableRectTransform.localScale = new Vector3(minimumScale.x, scaleableRectTransform.localScale.y);
            }
            if (scaleableRectTransform.localScale.y < minimumScale.y)
            {
                scaleableRectTransform.localScale = new Vector3(scaleableRectTransform.localScale.x, minimumScale.y);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            scaleableRectTransform.SetAsLastSibling();
        }
    }
}