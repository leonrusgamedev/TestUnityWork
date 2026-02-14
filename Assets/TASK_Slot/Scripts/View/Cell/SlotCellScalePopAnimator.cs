using System.Collections;
using UnityEngine;

namespace Rusleo.TestTask.View
{
    public class SlotCellScalePopAnimator : MonoBehaviour, ISlotCellAnimator
    {
        [SerializeField] private RectTransform target;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float peakScale = 1.18f;

        private Coroutine _routine;
        private Vector3 _baseScale;

        private void Awake()
        {
            if (target == null)
                target = GetComponent<RectTransform>();

            _baseScale = target.localScale;
        }

        private void OnDisable()
        {
            Stop();
        }

        public void PlayWin()
        {
            if (target == null)
                return;

            if (_routine != null)
                StopCoroutine(_routine);

            _baseScale = target.localScale;
            _routine = StartCoroutine(PlayRoutine());
        }

        public void Stop()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }

            if (target != null)
                target.localScale = _baseScale;
        }

        private IEnumerator PlayRoutine()
        {
            var t = 0f;
            var half = Mathf.Max(0.0001f, duration * 0.5f);

            var from = _baseScale;
            var to = _baseScale * peakScale;

            while (t < half)
            {
                t += Time.unscaledDeltaTime;
                var k = EaseOutCubic(Mathf.Clamp01(t / half));
                target.localScale = Vector3.LerpUnclamped(from, to, k);
                yield return null;
            }

            t = 0f;

            while (t < half)
            {
                t += Time.unscaledDeltaTime;
                var k = EaseInCubic(Mathf.Clamp01(t / half));
                target.localScale = Vector3.LerpUnclamped(to, from, k);
                yield return null;
            }

            target.localScale = from;
            _routine = null;
        }

        private float EaseOutCubic(float x)
        {
            var a = 1f - x;
            return 1f - a * a * a;
        }

        private float EaseInCubic(float x)
        {
            return x * x * x;
        }
    }
}
