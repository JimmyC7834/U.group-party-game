using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(InventoryFilter))]
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(TagCounter))]
[RequireComponent(typeof(PrefabSpawner))]
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(Recipe))]
public class Factory : MonoBehaviour
{
    [SerializeField] private int _spawnInterval;

    private Inventory _inventory;
    private InventoryFilter _inventoryFilter;
    private Interactable _interactable;
    private TagCounter _tagCounter;
    private PrefabSpawner _prefabSpawner;
    private Timer _timer;
    private Recipe _recipe;

    private void Awake()
    {
        // _inventory = GetComponent<Inventory>();
        _prefabSpawner = GetComponent<PrefabSpawner>();

        _tagCounter = GetComponent<TagCounter>();
        _tagCounter.OnCountChanged += CheckAndSpawn;

        _timer = GetComponent<Timer>();
        _timer.SetTime(_spawnInterval);
        _timer.SetCallBack(() => _prefabSpawner.SpawnPrefab(transform.position));

        _recipe = GetComponent<Recipe>();

        _inventoryFilter = GetComponent<InventoryFilter>();
        _inventoryFilter.AddTag(InventoryItemTag.Material);
        _inventoryFilter.AddTag(InventoryItemTag.Wood);
        _inventoryFilter.AddTag(InventoryItemTag.Metal);
        _inventoryFilter.AddTag(InventoryItemTag.Stone);

        _interactable = GetComponent<Interactable>();
        _interactable.SetInteractable(true);
        _interactable.OnInteracted += HandleInteract;
    }

    private void HandleInteract(Interactor interactor)
    {
        Holder holder = interactor.GetComponent<Holder>();
        if (holder == null) return;

        InventoryItem item = holder.GetHoldingObject().GetComponent<InventoryItem>();
        if (!_inventoryFilter.CanAccept(item)) return;

        holder.Release();
        Destroy(item.gameObject);

        foreach (InventoryItemTag tag in item.GetTags())
            _tagCounter.AddTag(tag);
    }

    private void CheckAndSpawn()
    {
        if (_recipe.CanMake(_tagCounter))
        {
            _recipe.Consume(_tagCounter);
            _timer.Time();
        }
    }
}
