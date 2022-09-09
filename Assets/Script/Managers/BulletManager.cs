using UnityEngine;
using UnityEngine.Pool;

namespace Game.Turret
{
    public class BulletManager : MonoBehaviour
    {
        // bullet pool
        private ObjectPool<TurretBulletBase> _pool;
        [SerializeField] public GameObject _prefab;

        private void Awake()
        {
            _pool = new ObjectPool<TurretBulletBase>(
                CreateBullet,
                PoolBullet,
                ReturnBullet
            );
        }

        private TurretBulletBase CreateBullet()
        {
            TurretBulletBase newObject = Instantiate(_prefab).GetComponent<TurretBulletBase>();
            newObject.SetPool(_pool);
            return newObject;
        }

        private void PoolBullet(TurretBulletBase bullet) => bullet.gameObject.SetActive(true);
        private void ReturnBullet(TurretBulletBase bullet) => bullet.gameObject.SetActive(false);

        public TurretBulletBase SpawnBullet()
        {
            TurretBulletBase bullet = _pool.Get();
            return bullet;
        }
    }
}