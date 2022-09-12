using System.Collections.Generic;
using UnityEngine;

using Game.Resource;
using Game.Turret;
using Game;

namespace Game.Core
{
    [RequireComponent(typeof(InteractableObject))]
    public class PowerStation : MonoBehaviour
    {
        [SerializeField] private InteractableObject _interactable;
        [SerializeField] private CircleCollider2D _supplyTrigger;
        [SerializeField] private List<TurretBase> _turretList;

        [Header("Station Value")]
        [SerializeField] private float _activeRange = 5f;
        [SerializeField] private float _minRange = 1f;
        [SerializeField] private float _supportPerFuel = 1f;
        [SerializeField] private float _decayRate = 0.5f;
        [SerializeField] private float _energySupplyPerSec = 1f;


        private void Awake()
        {
            _supplyTrigger = gameObject.AddComponent<CircleCollider2D>();
            _supplyTrigger.radius = _activeRange;
            _supplyTrigger.isTrigger = true;

            _interactable = GetComponent<InteractableObject>();
            _interactable.OnInteracted += HandleInteract;
        }

        private void Update()
        {
            _activeRange -= _decayRate * Time.deltaTime;
            if (_activeRange < _minRange)
            {
                _activeRange = _minRange;
            }
            _supplyTrigger.radius = _activeRange;

        }

        private void SupplyEnergy(TurretBase turret)
        {
            turret.EnergySupplied();
        }

        private void HandleInteract(InteractableObject.InteractInfo info)
        {
            ResourceObject resource;
            if (info.pickedObject != null && (resource = info.pickedObject.GetComponent<ResourceObject>()) != null)
            {
                // get the resource from the player
                info.pickedObject.Throw(Vector2.zero, 0, 0);
                resource.ReturnToPool();

                // todo: add corresponding values of fuels
                _activeRange += _supportPerFuel;
            }
        }

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     TurretBase turret = other.gameObject.GetComponent<TurretBase>();
        //     if (turret != null)
        //     {
        //         turret.Supplied();
        //         _turretList.Add(turret);
        //     }
        // }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.isTrigger) return;

            Vector2 dir = (Vector2)(other.gameObject.transform.position - transform.position) + other.offset;

            if (dir.magnitude > _activeRange)
                return;

            TurretBase turret = other.GetComponent<TurretBase>();
            if(turret != null)
            {
                SupplyEnergy(turret);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _activeRange);
        }
    }
}

