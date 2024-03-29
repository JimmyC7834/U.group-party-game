using System.Collections.Generic;
using Game.Player;
using Game.Turret;
using Game.Resource;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(InteractableObject))]
    public class PowerStation : ReceivableObject
    {
        [SerializeField] private CircleCollider2D _supplyTrigger;
        // [SerializeField] private List<TurretBase> _turretList;
        // [SerializeField] private GameObject _fuelPrefab;

        [Header("Station Value")]
        [SerializeField]
        private float _activeRange = 5f;

        [SerializeField] private float _minRange = 1f;
        [SerializeField] private float _refuelRate = 1f;
        [SerializeField] private ResourceId _fuelId;

        [SerializeField] private float _decayRate = 0.5f;
        // [SerializeField] private float _energySupplyPerSec = 1f;


        private void Awake()
        {
            base.Awake();
            _supplyTrigger = gameObject.AddComponent<CircleCollider2D>();
            _supplyTrigger.radius = _activeRange;
            _supplyTrigger.isTrigger = true;

            // _fuelId = ResourceId.Wood;
        }

        private void Update()
        {
            _activeRange = Mathf.Max(_minRange, _activeRange - _decayRate * Time.deltaTime);
            _supplyTrigger.radius = _activeRange;
        }

        private void SupplyEnergy(TurretBase turret)
        {
            // turret.EnergySupplied();
        }

        // private void HandleInteract(InteractableObject.InteractInfo info)
        // {
        //     ResourceObject resource;
        //     if (info.pickedObject != null && (resource = info.pickedObject.GetComponent<ResourceObject>()) != null)
        //     {
        //         // get the resource from the player
        //         info.pickedObject.Throw(Vector2.zero, 0, 0);
        //         resource.ReturnToPool();
        //
        //         // todo: add corresponding values of fuels
        //         _activeRange += _refuelRate;
        //     }
        // }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TurretBase turret = other.gameObject.GetComponent<TurretBase>();
            if (turret != null)
            {
                // turret.EnergySupplied();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TurretBase turret = other.gameObject.GetComponent<TurretBase>();
            if (turret != null)
            {
                // turret.StopSupply();
            }
        }

        // private void OnTriggerStay2D(Collider2D other) {
        //     if (other.isTrigger) return;
        //
        //     Vector2 dir = (Vector2)(other.gameObject.transform.position - transform.position) + other.offset;
        //
        //     if (dir.magnitude > _activeRange)
        //         return;
        //
        //     TurretBase turret = other.GetComponent<TurretBase>();
        //     if(turret != null)
        //     {
        //         SupplyEnergy(turret);
        //     }
        // }

        public override bool AcceptObject(ThrowableObject throwableObject)
        {
            ResourceObject resource = throwableObject.GetComponent<ResourceObject>();
            return (resource != null && resource.id == _fuelId) ? true : false;
        }

        protected override void HandleReceive(PlayerInteractControl interactor)
        {
            ResourceObject resourceObject = interactor.pickedObject.GetComponent<ResourceObject>();
            if (resourceObject == null) return;
            interactor.SubmitObject();
            resourceObject.ReturnToPool();
            _activeRange += _refuelRate;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _activeRange);
        }
    }
}