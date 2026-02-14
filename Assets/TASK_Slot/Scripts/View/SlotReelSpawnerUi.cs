using System.Collections;
using System.Collections.Generic;
using AxGrid;
using AxGrid.Base;
using Rusleo.TestTask.Core;
using Rusleo.TestTask.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Rusleo.TestTask.View
{
    public class SlotReelSpawnerUi : MonoBehaviourExt
    {
        [Header("Refs")] [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform despawnPoint;
        [SerializeField] private RectTransform centerPoint;
        [SerializeField] private SlotCellView cellPrefab;

        [Header("Motion")] [SerializeField] private float maxSpeed = 900f;
        [SerializeField] private float acceleration = 1800f;
        [SerializeField] private float deceleration = 2200f;

        [Header("Stop")] [SerializeField] private float snapTime = 0.25f;
        [SerializeField] private float snapStartSpeed = 120f;

        [Header("Pool")] [SerializeField] private int poolSize = 7;

        [Header("Offsets")] [SerializeField] private float cellsOffset = 5f;

        public UnityEvent onStart = new();
        public UnityEvent onStopping = new();
        public UnityEvent onStop = new();

        private readonly List<SlotCellView> _cells = new();

        private SlotConfig _config;
        private bool _initialized;


        private float _speed;
        private bool _isStopping;

        private Coroutine _spinCoroutine;
        private Coroutine _brakeCoroutine;

        private SlotItem _targetItem;

        #region Initialization

        [OnStart]
        private void StartThis()
        {
            Model.EventManager.AddAction(SlotEvents.SlotConfigReady, OnConfigReady);
            Model.EventManager.AddAction(SlotEvents.SlotStart, OnSlotStart);
            Model.EventManager.AddAction(SlotEvents.SlotStopping, OnSlotStopRequested);

            var cfg = Model.Get(SlotKeys.SlotConfig) as SlotConfig;
            if (cfg == null) return;

            _config = cfg;
            Initialize();
        }

        private void OnConfigReady()
        {
            var cfg = Model.Get(SlotKeys.SlotConfig) as SlotConfig;
            if (cfg == null) return;

            _config = cfg;
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            if (content == null || despawnPoint == null || centerPoint == null || cellPrefab == null)
            {
                Log.Error("SlotReelSpawnerUi: Missing refs.");
                enabled = false;
                return;
            }

            Prewarm();
        }

        #endregion

        #region Events

        private void OnSlotStart()
        {
            if (_initialized == false) return;
            
            if (_isStopping) return;
            
            StopAllRoutinesHard();

            _targetItem = null;
            _speed = 0f;

            _isStopping = false;
            _spinCoroutine = StartCoroutine(SpinUpAndLoopRoutine());
            onStart.Invoke();
        }

        private void OnSlotStopRequested(params object[] args)
        {
            if (_isStopping) return;

            if (_spinCoroutine == null) return;
            
            StopCoroutine(_spinCoroutine);
            _spinCoroutine = null;
            _isStopping = true;

            var itemObj = args is { Length: > 0 } ? args[0] : null;
            _targetItem = itemObj as SlotItem;

            if (_targetItem == null)
            {
                Log.Warn("SlotReelSpawnerUi: Target item is null.");
            }

            if (_brakeCoroutine == null)
            {
                _brakeCoroutine = StartCoroutine(BrakeRoutine(_targetItem));
                onStopping.Invoke();
            }
        }

        #endregion

        #region Coroutines

        private IEnumerator SpinUpAndLoopRoutine()
        {
            while (_speed < maxSpeed)
            {
                _speed = Mathf.Clamp(_speed + acceleration * Time.deltaTime, 0f, maxSpeed);

                if (_speed > 0f)
                    Scroll(_speed * Time.deltaTime);

                yield return null;
            }

            while (_isStopping == false)
            {
                var dt = Time.deltaTime;

                if (_speed > 0f)
                    Scroll(_speed * dt);

                yield return null;
            }
        }

        private IEnumerator BrakeRoutine(SlotItem resultItem)
        {
            SlotCellView cell = null;
            while (_speed > 0f)
            {
                _speed -= deceleration * Time.deltaTime;
                _speed = Mathf.Clamp(_speed, 0, maxSpeed);
                if (_speed > 0f)
                    Scroll(_speed * Time.deltaTime);

                var remainingDistance = Mathf.Pow(_speed, 2) / (2f * deceleration);
                if (remainingDistance <= _cells[0].RectTransform.anchoredPosition.y)
                {
                    cell = _cells[0];
                    cell.SetItem(resultItem);
                    break;
                }

                yield return null;
            }

            var currentDeceleration = deceleration;
            if (cell)
                currentDeceleration = Mathf.Pow(_speed, 2) / (2f * cell.RectTransform.anchoredPosition.y);

            while (_speed > 0f)
            {
                _speed -= currentDeceleration * Time.deltaTime;
                _speed = Mathf.Clamp(_speed, 0, maxSpeed);
                if (_speed > 0f)
                    Scroll(_speed * Time.deltaTime);
                yield return null;
            }

            if (cell)
                cell.PlayWin();

            _brakeCoroutine = null;

            _isStopping = false;
            Settings.Model.EventManager.Invoke(SlotEvents.SlotResult);
            onStop.Invoke();
        }

        private void StopAllRoutinesHard()
        {
            if (_spinCoroutine != null)
            {
                StopCoroutine(_spinCoroutine);
                _spinCoroutine = null;
            }

            if (_brakeCoroutine != null)
            {
                StopCoroutine(_brakeCoroutine);
                _brakeCoroutine = null;
            }
        }

        #endregion

        #region Slots

        private void Prewarm()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            _cells.Clear();

            poolSize = Mathf.Max(5, poolSize);

            float? prewarmOffset = null;

            for (var i = 0; i < poolSize; i++)
            {
                var cell = Instantiate(cellPrefab, content);
                cell.transform.SetAsFirstSibling();

                if (i == 0)
                {
                    cell.RectTransform.anchoredPosition = despawnPoint.anchoredPosition;
                }
                else
                {
                    cell.RectTransform.anchoredPosition =
                        _cells[0].RectTransform.anchoredPosition + new Vector2(0f, cell.Height + cellsOffset);
                }

                if (cell.RectTransform.anchoredPosition.y > 0f)
                    prewarmOffset ??= cell.RectTransform.anchoredPosition.y;

                cell.SetItem(WeightRandomItemUtil.GetRandomItem(_config.Items));
                cell.gameObject.name += "_" + i;
                _cells.Insert(0, cell);
            }

            Scroll(prewarmOffset ?? 0f);
        }

        private void Scroll(float delta)
        {
            for (var i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];

                cell.RectTransform.anchoredPosition -= new Vector2(0f, delta);

                if (cell.RectTransform.anchoredPosition.y >= despawnPoint.anchoredPosition.y)
                    continue;
                

                var top = _cells[0];
                cell.RectTransform.anchoredPosition =
                    top.RectTransform.anchoredPosition + new Vector2(0f, cell.Height + cellsOffset);

                cell.SetItem(WeightRandomItemUtil.GetRandomItem(_config.Items));

                cell.transform.SetAsFirstSibling();

                _cells.RemoveAt(i);
                _cells.Insert(0, cell);
            }
        }

        #endregion
    }
}