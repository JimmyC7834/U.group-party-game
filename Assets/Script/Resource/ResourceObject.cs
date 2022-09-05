using Game.Resource;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    [RequireComponent(typeof(ThrowableObject))]
    public class ResourceObject : MonoBehaviour
    {
        public ResourceId id { get; private set; }
        private ObjectPool<ResourceObject> _pool;
        [SerializeField] private SpriteRenderer spriteRenderer = default;

        private void OnEnable() {
            if (gameObject.layer != LayerMask.NameToLayer("ResourceObject"))
                gameObject.layer = LayerMask.NameToLayer("ResourceObject");
        }

        public void Initialize(ResourceSO data)
        {
            id = data.id;
            spriteRenderer.sprite = data.sprite;
        }
        
        public void SetPool(ObjectPool<ResourceObject> pool) => _pool = pool;

        public void ReturnToPool() => _pool.Release(this);
    }
}