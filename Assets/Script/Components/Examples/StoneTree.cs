using Game;
using UnityEngine;

[RequireComponent(typeof(PrefabSpawner))]
[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(Holder))]
[RequireComponent(typeof(Interactable))]
public class StoneTree : MonoBehaviour
{
    [SerializeField] private int _regenInterval;

    private PrefabSpawner _prefabSpawner;
    private Timer _timer;
    private Holder _holder;
    private Interactable _interactable;

    private void Awake()
    {
        _prefabSpawner = GetComponent<PrefabSpawner>();

        _timer = GetComponent<Timer>();
        _timer.SetTime(_regenInterval);
        _timer.SetCallBack(CheckAndSpawn);
        _timer.Start();

        _holder = GetComponent<Holder>();
        _interactable = GetComponent<Interactable>();
        _interactable.SetInteractable(true);
        _interactable.OnInteracted += HandleInteracted;
    }

    private void CheckAndSpawn()
    {
        if (_holder.IsHolding()) return;

        Throwable obj = _prefabSpawner.SpawnPrefab<Throwable>();
        _holder.Hold(obj);
    }

    private void HandleInteracted(Interactor interactor)
    {
        Debug.Log("stoen tree interacted");
        if (!_holder.IsHolding()) return;

        Holder holder = interactor.GetComponent<Holder>();
        if (holder == null) return;
        if (holder.IsHolding()) return;

        holder.Hold(_holder.Release());
        _timer.Start();
    }
}