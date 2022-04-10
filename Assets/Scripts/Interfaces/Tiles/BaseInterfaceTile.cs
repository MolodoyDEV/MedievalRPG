using Molodoy.Inspector.Extentions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Molodoy.Interfaces
{
    public class BaseInterfaceTile : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        protected Image background;
        [SerializeField] protected Text tileName;
        [SerializeField] private Image icon;
        [ReadOnlyInspector] [SerializeField] private int entityID;
        [SerializeField]
        protected ColorBlock colors = new ColorBlock()
        {
            normalColor = Color.white,
            highlightedColor = Color.white,
            pressedColor = Color.white,
            selectedColor = Color.white,
            disabledColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.5f)
        };

        public UnityEvent<int> OnLeftClick = new UnityEvent<int>();
        public int EntityID { get => entityID; protected set => entityID = value; }
        public Image Icon { get => icon;}


        protected virtual void Awake()
        {
            background = GetComponent<Image>();
        }

        public void SetValues(string name, Sprite sprite, int id)
        {
            tileName.text = name;
            icon.sprite = sprite;
            EntityID = id;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick?.Invoke(EntityID);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (background.color != colors.selectedColor)
            {
                background.color = colors.normalColor;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (background.color != colors.selectedColor)
            {
                background.color = colors.highlightedColor;
            }
        }
    }
}