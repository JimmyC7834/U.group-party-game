using UnityEngine;

namespace Game
{
    /// <summary>
    /// Modifier for throwables, make it bounce as it lands
    /// </summary>
    [RequireComponent(typeof(Throwable))]
    public class Bounce : MonoBehaviour
    {
        [SerializeField] private Throwable _throwable;
        [SerializeField] private float _bounceSpeedThreshold;
        [SerializeField] private float _bounceSlowMultiplier;
        [SerializeField] private float _initialVerticalVelocity;
        [SerializeField] private Vector2 _initialHorizontalVelocity;

        private void Awake()
        {
            _throwable = GetComponent<Throwable>();
            _throwable.OnGrounded += DoBounce;
            _throwable.OnLaunched += RecordVelocity;
        }

        public void RecordVelocity()
        {
            _initialHorizontalVelocity = _throwable.HorizontalVelocity;
            _initialVerticalVelocity = _throwable.VerticalVelocity;
        }

        public void DoBounce()
        {
            if (_throwable.HorizontalVelocity.magnitude > _bounceSpeedThreshold) return;
            _throwable.Launch(
                _initialHorizontalVelocity * _bounceSlowMultiplier,
                _initialVerticalVelocity * _bounceSlowMultiplier,
                0);
        }
    }
}