using Rusleo.TestTask.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Rusleo.TestTask.View
{
    public class SlotCellView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image icon;

        public RectTransform RectTransform =>
            rectTransform != null ? rectTransform : (rectTransform = GetComponent<RectTransform>());

        public Image Icon => icon;

        public float Height => RectTransform.rect.height;

        public void SetItem(SlotItem item)
        {
            if (icon == null) return;

            icon.sprite = item == null ? null : item.Icon;
        }
    }
}