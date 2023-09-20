using UnityEngine;

namespace Game
{

    /// <summary>
    /// Modifier for throwable. Creates fake height visuals
    /// </summary>
    [RequireComponent(typeof(Throwable))]
    public class FakeHeight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _bodySprite;
        [SerializeField] private float _perspectiveMultiplier = 0.2f;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            Throwable throwable = GetComponent<Throwable>();
            throwable.OnGrounded += BringSpriteBackward;
            throwable.OnLaunched += BringSpriteUpward;
            throwable.OnHeld += (_) => BringSpriteUpward();
        }

        private void Update()
        {
            _bodySprite.transform.localScale =
                    Vector2.one * (1 + _transform.position.z * _perspectiveMultiplier);
        }

        private void BringSpriteUpward()
        {
            _bodySprite.sortingOrder = 1;
        }

        private void BringSpriteBackward()
        {
            _bodySprite.sortingOrder = 0;
        }
    }
}
