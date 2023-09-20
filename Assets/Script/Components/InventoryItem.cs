using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum InventoryItemTag
    {
        Material,
        Wood,
        Stone,
        Metal,
        Crystal,
    }

    public abstract class InventoryItem : MonoBehaviour
    {
        [SerializeField] private HashSet<InventoryItemTag> _tags;

        private bool AddTag(InventoryItemTag tag)
        {
            return _tags.Add(tag);
        }

        private bool RemoveTag(InventoryItemTag tag)
        {
            return _tags.Remove(tag);
        }

        private bool ContainsTag(InventoryItemTag tag)
        {
            return _tags.Contains(tag);
        }
    }
}