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
        [SerializeField] protected Image icon;
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

        public UnityEvent<string> OnLeftClick = new UnityEvent<string>();

        public int EntityID { get => entityID; protected set => entityID = value; }

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