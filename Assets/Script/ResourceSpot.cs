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

        [SerializeField] private float _spawnVerticalVelocity;
        [SerializeField] private float _noSpawnRadius;
        [SerializeField] private float _spawnRadius;

        [SerializeField] private float _bounceOffStrength;
        [SerializeField] private float _hitHeight;
        [SerializeField] private float _travelTime;

        private void Awake()
        {
            _hitTrigger = GetComponent<Collider2D>();
        }

        private void RandomlySpawnResource(ResourceId id)
        {
            float a = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(_noSpawnRadius, _spawnRadius);
            SpawnResourceObject(id, new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * r);
        }

        private void SpawnResourceObject(ResourceId id, Vector3 dir)
        {
            ResourceObject resource = _gameplayService.resourceManager.SpawnResource(id);
            resource.transform.position = transform.position;
            float gravity = -30f;
            float distance = dir.magnitude;
            float xSpeed = distance / _travelTime;
            float ySpeed = gravity * _travelTime * 0.5f;
            resource.GetComponent<ThrowableObject>().Launch(
                Vector3.Normalize(dir) * xSpeed, ySpeed, .5f);
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