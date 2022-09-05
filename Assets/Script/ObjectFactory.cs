using Game.Data;
using Game.Resource;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    [RequireComponent(typeof(InteractableObject))]
    public class ObjectFactory : MonoBehaviour
    {
        [SerializeField] private TimerTrigger spawnTimer;
        [SerializeField] private InteractableObject interactable;

        [Header("Spawn Values")]
        [SerializeField] private RecipeSO recipe;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float spawnTime;
        [SerializeField] private float spawnHeight;
        [SerializeField] private float spawnVerticalVelocity;
        
        private ObjectPool<ThrowableObject> _pool;
        [SerializeField] private ThrowableObject _prefab;

        private ThrowableObject CreateThrowableObject() => Instantiate(_prefab).GetComponent<ThrowableObject>();
        private void PoolThrowableObject(ThrowableObject enemyController) => enemyController.gameObject.SetActive(true);
        private void ReturnThrowableObject(ThrowableObject enemyController) => enemyController.gameObject.SetActive(false);

        
        [Header("Debug Values")]
        [SerializeField] private int[] resourceCount;

        private void OnEnable()
        {
            interactable = GetComponent<InteractableObject>();
        }

        private void Awake()
        {
            _pool = new ObjectPool<ThrowableObject>(
                CreateThrowableObject,
                PoolThrowableObject,
                ReturnThrowableObject
            );
            
            resourceCount = new int[(int) ResourceId.Count];

            spawnTimer.enabled = false;
            spawnTimer.SetTimeInterval(spawnTime);
            
            interactable = GetComponent<InteractableObject>();
            interactable.OnInteracted += HandleInteract;
        }

        public void SpawnObject()
        {
            recipe.ConsumeIngredients(resourceCount);
            ThrowableObject throwable = _pool.Get();
            throwable.transform.position = spawnPoint.position;
            throwable.Throw(Vector2.zero, spawnVerticalVelocity, spawnHeight);

            if (!recipe.Craftable(resourceCount))
            {
                Debug.Log($"{name}: craftable: {recipe.Craftable(resourceCount)}");
                spawnTimer.enabled = false;
            }

        }

        private void HandleInteract(InteractableObject.InteractInfo info)
        {
            ResourceObject resource;
            if (info.pickedObject != null && (resource = info.pickedObject.GetComponent<ResourceObject>()) != null)
            {
                // get the resource from the player
                info.pickedObject.Throw(Vector2.zero, 0, 0);
                resource.ReturnToPool();

                // check if resources is enough for a spawn
                resourceCount[(int) resource.id]++;
                if (recipe.Craftable(resourceCount))
                {
                    spawnTimer.enabled = true;
                }
            }
        }
    }
}