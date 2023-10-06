using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    [SerializeField] private Pair[] _pairs;
    [SerializeField] private (InventoryItemTag, int)[] _recipe;

    private void Awake()
    {
        _recipe = new (InventoryItemTag, int)[_pairs.Length];
        for (int i = 0; i < _pairs.Length; i++)
        {
            _recipe[i] = (_pairs[i].tag, _pairs[i].count);
        }
    }

    public bool CanMake(TagCounter tagCounter)
    {
        foreach ((InventoryItemTag, int) pair in _recipe)
        {
            if (tagCounter.GetCount(pair.Item1) < pair.Item2)
                return false;
        }

        return true;
    }

    public bool Consume(TagCounter tagCounter)
    {
        if (!CanMake(tagCounter)) return false;

        foreach ((InventoryItemTag, int) pair in _recipe)
            tagCounter.RemoveTag(pair.Item1, pair.Item2);

        return true;
    }

    [Serializable]
    private struct Pair
    {
        public InventoryItemTag tag;
        public int count;
    }
}
