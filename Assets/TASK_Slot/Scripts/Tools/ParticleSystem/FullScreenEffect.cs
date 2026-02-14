using UnityEngine;

namespace Rusleo.TestTask.Tools
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(ParticleSystem))]
    public class FullScreenEffect : MonoBehaviour
    {
        [SerializeField] private bool setScalingMode = true;
        [SerializeField] private bool setSimulationSpace = true;

        private RectTransform _rectTransform;
        private ParticleSystem _particleSystem;
        private ParticleSystem.ShapeModule _shape;

        private void Awake()
        {
            Cache();
            ApplyStaticSettings();
            UpdateShapeFromRect();
        }

        private void OnEnable()
        {
            Cache();
            ApplyStaticSettings();
            UpdateShapeFromRect();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateShapeFromRect();
        }

        private void OnValidate()
        {
            Cache();
            ApplyStaticSettings();
            UpdateShapeFromRect();
        }

        private void Cache()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            if (_particleSystem == null)
                _particleSystem = GetComponent<ParticleSystem>();

            _shape = _particleSystem.shape;
        }

        private void ApplyStaticSettings()
        {
            var main = _particleSystem.main;

            if (setSimulationSpace)
                main.simulationSpace = ParticleSystemSimulationSpace.Local;

            if (setScalingMode)
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;

            _shape.enabled = true;
            _shape.shapeType = ParticleSystemShapeType.Rectangle;
        }

        private void UpdateShapeFromRect()
        {
            if (_rectTransform == null || _particleSystem == null)
                return;

            var rect = _rectTransform.rect;
            var size = rect.size;

            _shape.scale = new Vector3(size.x, size.y, 1f);
            var center = rect.center;
            _shape.position = new Vector3(center.x, center.y, 0f);
        }
    }
}