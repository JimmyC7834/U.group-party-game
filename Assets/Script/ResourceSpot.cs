using Game.Resource;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class ResourceSpot : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;
        [SerializeField] private Collider2D _hitTrigger;

        [Header("Spawner Settings")] [SerializeField]
        private ResourceId _resourceType;

        // [SerializeField] private float _spawnVerticalVelocity;
        [SerializeField] private float _noSpawnRadius;
        [SerializeField] private float _spawnRadius;

        [SerializeField] private float _bounceOffStrength;
        [SerializeField] private float _hitHeight;
        [Range(0.1f, 10f)]
        [SerializeField] private float _travelTime;

        private void Awake()
        {
            _hitTrigger = GetComponent<Collider2D>();
        }

        private void RandomlySpawnResource(ResourceId id)
        {
            float a = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(_noSpawnRadius, _spawnRadius);
            SpawnResourceObject(id, new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0) * r);
        }

        private void SpawnResourceObject(ResourceId id, Vector3 dir)
        {
            float initHeight = _hitHeight;
            ResourceObject resource = _gameplayService.resourceManager.SpawnResource(id);
            resource.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            float gravity = 30f;
            float distance = dir.magnitude;
            float xSpeed = distance / _travelTime;
            float yInitSpeed = gravity * _travelTime * 0.5f + initHeight / _travelTime;
            float ySpeed = Mathf.Sqrt(Mathf.Pow(yInitSpeed, 2) - 2 * gravity * initHeight);
            Debug.Log("distance: " + distance);
            Debug.Log("xSpeed: " + xSpeed);
            Debug.Log("ySpeed: " + ySpeed);
            resource.GetComponent<ThrowableObject>().Launch(
                Vector3.Normalize(dir) * xSpeed, ySpeed, initHeight);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger == false) return;
            if (!other.gameObject.CompareTag("ResourceCollector")) return;
            ThrowableObject throwableObject = other.GetComponent<ThrowableObject>();
            if (throwableObject.height < _hitHeight) return;

            // bounce off throwableObject
            throwableObject.SetGroundVelocity(-throwableObject.groundVelocity);
            throwableObject.SetVerticalVelocity(Mathf.Abs(throwableObject.verticalVelocity) * _bounceOffStrength);


                RandomlySpawnResource(_resourceType);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _spawnRadius);
            Gizmos.DrawWireSphere(transform.position, _noSpawnRadius);
            Gizmos.color = Color.white;
        }
    }
}