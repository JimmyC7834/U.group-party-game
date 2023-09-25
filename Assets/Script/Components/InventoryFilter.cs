using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InventoryFilter : MonoBehaviour
    {
        private HashSet<InventoryItemTag> _tags = new HashSet<InventoryItemTag>();

        public bool AddTag(InventoryItemTag tag)
        {
            return _tags.Add(tag);
        }

        public bool RemoveTag(InventoryItemTag tag)
        {
            return _tags.Remove(tag);
        }

        public bool CanAccept(InventoryItem item)
        {
            if (item == null) return false;
            return item.Any(_tags);
        }
    }
}