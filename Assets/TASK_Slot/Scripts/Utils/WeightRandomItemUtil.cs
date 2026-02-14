using System.Collections.Generic;
using System.Linq;
using Rusleo.TestTask.Core;
using UnityEngine;

namespace Rusleo.TestTask.Utils
{
    public static class WeightRandomItemUtil
    {
        public static SlotItem GetRandomItem(IReadOnlyList<SlotItem> slotItems)
        {
            if (slotItems == null || slotItems.Count == 0)
                return null;

            var totalWeight = slotItems.Where(item => item != null).Sum(item => Mathf.Max(0f, item.Chance));

            if (totalWeight <= 0f)
                return null;

            var randomValue = Random.Range(0f, totalWeight);

            foreach (var item in slotItems)
            {
                if (item == null)
                    continue;

                var weight = Mathf.Max(0f, item.Chance);

                if (randomValue < weight)
                    return item;

                randomValue -= weight;
            }

            return slotItems[^1];
        }
    }
}