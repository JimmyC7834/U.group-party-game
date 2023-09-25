using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Events;

public class TagCounter : MonoBehaviour
{
    private Dictionary<InventoryItemTag, int> _dict = new Dictionary<InventoryItemTag, int>();

    public event UnityAction OnCountChanged = delegate { };

    public void AddTag(InventoryItemTag tag)
    {
        if (!_dict.ContainsKey(tag))
            _dict.Add(tag, 0);

        _dict[tag]++;
        OnCountChanged.Invoke();
    }

    public bool RemoveTag(InventoryItemTag tag, int count)
    {
        if (GetCount(tag) < count) return false;

        _dict[tag] -= count;
        OnCountChanged.Invoke();
        return true;
    }

    public int GetCount(InventoryItemTag tag)
    {
        if (!_dict.ContainsKey(tag)) return 0;
        return _dict[tag];
    }
}
