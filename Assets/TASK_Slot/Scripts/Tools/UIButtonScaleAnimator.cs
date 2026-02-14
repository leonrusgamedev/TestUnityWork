using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rusleo.TestTask.Tools
{
    [RequireComponent(typeof(Button))]
    public class UIButtonScaleAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        [Header("Scale")] [SerializeField] private float hoverScale = 1.05f;
        [SerializeField] private float pressedScale = 0.92f;

        [Header("Timing")] [SerializeField] private float hoverSpeed = 12f;
        [SerializeField] private float pressSpeed = 18f;
        [SerializeField] private float bounceSpeed = 10f;

        [Header("Bounce")] [SerializeField] private float bounceAmount = 0.08f;

        private RectTransform _rectTransform;
        private Button _button;

        private Vector3 _baseScale;
        private Vector3 _targetScale;
        private bool _isPressed;
        private bool _isHover;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _button = GetComponent<Button>();
            _baseScale = _rectTransform.localScale;
            _targetScale = _baseScale;
        }

        private void Update()
        {
            var speed = _isPressed ? pressSpeed : hoverSpeed;

            _rectTransform.localScale = Vector3.Lerp(
                _rectTransform.localScale,
                _targetScale,
                Time.unscaledDeltaTime * speed
            );
            
            if (_button.interactable == false)
                _rectTransform.localScale = _baseScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHover = true;

            if (_isPressed == false)
                _targetScale = _baseScale * hoverScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHover = false;

            if (_isPressed == false)
                _targetScale = _baseScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _targetScale = _baseScale * pressedScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;

            StopAllCoroutines();
            StartCoroutine(BounceRoutine());
        }

        private System.Collections.IEnumerator BounceRoutine()
        {
            var overshoot = _baseScale * (1f + bounceAmount);

            while (Vector3.Distance(_rectTransform.localScale, overshoot) > 0.001f)
            {
                _rectTransform.localScale = Vector3.Lerp(
                    _rectTransform.localScale,
                    overshoot,
                    Time.unscaledDeltaTime * bounceSpeed
                );

                yield return null;
            }

            _targetScale = _isHover
                ? _baseScale * hoverScale
                : _baseScale;
        }
    }
}