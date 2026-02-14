using System;
using UnityEngine;

namespace Rusleo.TestTask.Core
{
    [CreateAssetMenu(menuName = "SlotMachine/SlotItem", fileName = "SlotItem")]
    public class SlotItem : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField, Min(0f)] private float chance = 1f;

        public Sprite Icon => icon;
        public float Chance => chance;
    }
}