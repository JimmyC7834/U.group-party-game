using Game.Data;
using Game.Player;
using Game.Resource;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    [RequireComponent(typeof(InteractableObject))]
    public class ObjectFactory : MonoBehaviour
    {
        [SerializeField] private TimerTrigger _spawnTimer;
        [SerializeField] private InteractableObject _interactableObject;

        [Header("Spawn Values")] [SerializeField]
        private RecipeSO _recipe;

        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnTime;
        [SerializeField] private float _spawnHeight;
        [SerializeField] private float _spawnVerticalVelocity;

        private ObjectPool<ThrowableObject> _pool;
        [SerializeField] private ThrowableObject _prefab;

        private ThrowableObject CreateThrowableObject() => Instantiate(_prefab).GetComponent<ThrowableObject>();
        private void PoolThrowableObject(ThrowableObject enemyController) => enemyController.gameObject.SetActive(true);

        private void ReturnThrowableObject(ThrowableObject enemyController) =>
            enemyController.gameObject.SetActive(false);


        [Header("Debug Values")] [SerializeField]
        private int[] resourceCount;

        private void Awake()
        {
            _pool = new ObjectPool<ThrowableObject>(
                CreateThrowableObject,
                PoolThrowableObject,
                ReturnThrowableObject
            );

            resourceCount = new int[(int) ResourceId.Count];

            _spawnTimer.enabled = false;
            _spawnTimer.SetTimeInterval(_spawnTime);

            _interactableObject = GetComponent<InteractableObject>();
            _interactableObject.SetInteractable(true);
            _interactableObject.OnInteracted += HandleInteract;
        }

        public void SpawnObject()
        {
            _recipe.ConsumeIngredients(resourceCount);
            ThrowableObject throwable = _pool.Get();
            throwable.transform.position = _spawnPoint.position;
            throwable.Throw(Vector2.zero, _spawnVerticalVelocity, _spawnHeight);

            if (!_recipe.Craftable(resourceCount))
            {
                Debug.Log($"{name}: craftable: {_recipe.Craftable(resourceCount)}");
                _spawnTimer.enabled = false;
            }
        }

        private void HandleInteract(PlayerInteractControl interactor)
        {
            if (!interactor.pickingObject) return;
            ResourceObject resourceObject = interactor.pickedObject.GetComponent<ResourceObject>();
            if (resourceObject == null) return;

            // get the resource from the player
            interactor.SubmitObject();
            resourceObject.ReturnToPool();

            // check if resources is enough for a spawn
            resourceCount[(int) resourceObject.id]++;
            _spawnTimer.enabled = _recipe.Craftable(resourceCount);
        }
    }
}