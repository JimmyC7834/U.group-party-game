using System.Collections;
using Game.Resource;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Resource
{
    public class ResourceSpawner : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;

        [Header("Spawner Settings")]
        [SerializeField] private float timeInterval = default;
        [SerializeField] private ResourceId resourceType = default;
        [SerializeField] private float spawnVerticalVelocity = default;
        [SerializeField] private float noSpawnRadius = default;
        [SerializeField] private float spawnRadius = default;
        [SerializeField] private float limitCheckRadius = default;
        [SerializeField] private int limitCount = default;
        [SerializeField] private int preWarmNum = default;

        [Space]
        [Header("Debug Values")]
        [SerializeField] private int resourceCountInRange;
        [SerializeField] private bool stopped = false;

        private void Start()
        {
            // pre fill the range with resources
            for (int i = 0; i < preWarmNum; i++)
            {
                RandomlySpawnResource(resourceType);
            }

            StartCoroutine(SpawnResourceAfterSec(timeInterval));
        }

        private void Update()
        {
            // if currently stopped but should spawn resource, continue
            if (stopped && ResourceInRangeCount() < limitCount)
            {
                stopped = false;
                StartCoroutine(SpawnResourceAfterSec(timeInterval));
            }
        }

        private IEnumerator SpawnResourceAfterSec(float sec)
        {
            yield return new WaitForSecondsRealtime(sec);
            RandomlySpawnResource(resourceType);

            // check if limit is reached, +1 because the resource just spawned didn't count
            if (ResourceInRangeCount() + 1 < limitCount)
            {
                StartCoroutine(SpawnResourceAfterSec(timeInterval));
            }
            else
            {
                stopped = true;
            }
        }

        private int ResourceInRangeCount() {
            resourceCountInRange = 0;

            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, limitCheckRadius, LayerMask.GetMask("ResourceObject"));
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<ResourceObject>().id == resourceType)
                    resourceCountInRange++;
            }

            return resourceCountInRange;
        }

        private void RandomlySpawnResource(ResourceId id)
        {
            float a = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(noSpawnRadius, spawnRadius);
            SpawnResourceObject(id, (Vector2) transform.position + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * r);
        }

        private void SpawnResourceObject(ResourceId id, Vector2 position)
        {
            ResourceObject resource = _gameplayService.resourceManager.SpawnResource(id);
            resource.transform.position = position;
            resource.GetComponent<ThrowableObject>().Launch(Vector2.zero, spawnVerticalVelocity, .5f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
            Gizmos.DrawWireSphere(transform.position, noSpawnRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, limitCheckRadius);
            Gizmos.color = Color.white;
        }
    }
}