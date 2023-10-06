using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

    public class InventoryItem : MonoBehaviour
    {
        private HashSet<InventoryItemTag> _tags = new HashSet<InventoryItemTag>();
        [SerializeField] private InventoryItemTag[] _itemTags;

        private void Awake()
        {
            foreach (InventoryItemTag tag in _itemTags)
                AddTag(tag);
        }

        public bool AddTag(InventoryItemTag tag)
        {
            return _tags.Add(tag);
        }

        public bool RemoveTag(InventoryItemTag tag)
        {
            return _tags.Remove(tag);
        }

        public bool ContainsTag(InventoryItemTag tag)
        {
            return _tags.Contains(tag);
        }

        public bool Any(HashSet<InventoryItemTag> tags)
        {
            return _tags.Intersect(tags).Count() > 0;
        }

        public InventoryItemTag[] GetTags()
        {
            return _tags.ToArray();
        }
    }
}