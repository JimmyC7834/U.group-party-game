using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetLoader : MonoBehaviour
{
    [SerializeField] protected CircleCollider2D _shootingTrigger;
    [SerializeField] private List<Collider2D> _targetQueue;
    private float _range;
    private string _targetLayer = "Enemy";

    private void Awake()
    {
        // addcompo
        _shootingTrigger = gameObject.AddComponent<CircleCollider2D>();
        _shootingTrigger.radius = _range;
        _shootingTrigger.isTrigger = true;
        _targetQueue = new List<Collider2D>();
    }
    public void SetRange(float range)
    {
        _range = range;
        _shootingTrigger.radius = _range;
    }

    public void SetLayer(string layer)
    {
        _targetLayer = layer;
    }


    public Transform GetTarget()
    {
        return _targetQueue.FirstOrDefault()?.transform;
    }

    public List<Transform> GetTargets()
    {
        return _targetQueue.Select(target => target.transform).ToList();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
        {
            _targetQueue.Add(other);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _targetQueue.Remove(other);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, _range);
    // }
}
