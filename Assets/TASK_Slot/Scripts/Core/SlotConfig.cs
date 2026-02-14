using System.Collections.Generic;
using UnityEngine;

namespace Rusleo.TestTask.Core
{
    [CreateAssetMenu(menuName = "SlotMachine/SlotConfig", fileName = "SlotConfig")]
    public class SlotConfig : ScriptableObject
    {
        [Header("Items")]
        [SerializeField] private SlotItem[] items;

        public IReadOnlyList<SlotItem> Items => items;
    }
}