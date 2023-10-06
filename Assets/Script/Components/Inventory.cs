using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int _size;
        private Dictionary<InventoryItem, int> _dict;

        public event UnityAction<InventoryItem> OnItemAdded = delegate { };

        public bool AddItem(InventoryItem item)
        {
            InventoryFilter filter = GetComponent<InventoryFilter>();
            if (filter != null && !filter.CanAccept(item)) return false;

            if (!_dict.ContainsKey(item))
                _dict.Add(item, 0);

            _dict[item]++;
            OnItemAdded.Invoke(item);
            return true;
        }

        public void Clear()
        {
            _dict.Clear();
        }
    }
}