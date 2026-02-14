using Rusleo.TestTask.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Rusleo.TestTask.View
{
    public class SlotCellView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image icon;
        [SerializeField] private MonoBehaviour animatorBehaviour;

        private ISlotCellAnimator _animator;

        public RectTransform RectTransform =>
            rectTransform != null ? rectTransform : (rectTransform = GetComponent<RectTransform>());

        public Image Icon => icon;

        public float Height => RectTransform.rect.height;

        private void Awake()
        {
            _animator = animatorBehaviour as ISlotCellAnimator;
        }

        public void SetItem(SlotItem item)
        {
            if (icon == null) return;

            icon.sprite = item == null ? null : item.Icon;
        }

        public void PlayWin()
        {
            _animator?.PlayWin();
        }

        public void StopAnimation()
        {
            _animator?.Stop();
        }
    }
}