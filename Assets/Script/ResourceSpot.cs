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

        private void Awake()
        {
            _hitTrigger = GetComponent<Collider2D>();
        }

        private void RandomlySpawnResource(ResourceId id)
        {
            float a = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(_noSpawnRadius, _spawnRadius);
            SpawnResourceObject(id, (Vector2) transform.position + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * r);
        }

        private void SpawnResourceObject(ResourceId id, Vector2 position)
        {
            ResourceObject resource = _gameplayService.resourceManager.SpawnResource(id);
            resource.transform.position = position;
            resource.GetComponent<ThrowableObject>().Launch(Vector2.zero, _spawnVerticalVelocity, .5f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger == false) return;
            if (!other.gameObject.CompareTag("ResourceCollector")) return;
            ThrowableObject throwableObject = other.GetComponent<ThrowableObject>();
            if (throwableObject.height < _hitHeight) return;

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