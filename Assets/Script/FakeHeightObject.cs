using UnityEngine;
using UnityEngine.Events;

public class FakeHeightObject : MonoBehaviour
{
    [SerializeField] protected Transform bodyTransform = default;
    [SerializeField] protected Transform shadowTransform = default;

    [Header("Physics Values")]
    [SerializeField] protected float gravity = default;
    [SerializeField] protected Vector2 windVelocity = default;

    [Header("Physics Debug Values")]
    [SerializeField] protected Vector2 groundVelocity = default;
    [SerializeField] protected float verticalVelocity = default;
    [SerializeField] protected bool _isGrounded = false;
    public UnityAction OnGrounded;
    public UnityAction OnLaunch;

    public virtual bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }

        set
        {
            _isGrounded = value;
            if (value)
                OnGrounded?.Invoke();
        }
    }


    private void OnEnable()
    {
        if (bodyTransform == null || shadowTransform == null)
        {
            Debug.LogWarning($"missing bodyTransform or shadowTransform, please check your object");
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        UpdatePhysics();
        CheckGroundHit();
    }

    protected virtual void UpdatePhysics()
    {
        if (!IsGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            bodyTransform.position += Vector3.up * verticalVelocity * Time.deltaTime;
            groundVelocity += windVelocity;
            transform.position += (Vector3)(groundVelocity) * Time.deltaTime;
        }
    }

    protected virtual void CheckGroundHit()
    {
        if (bodyTransform.position.y <= transform.position.y && !IsGrounded)
        {
            IsGrounded = true;
            bodyTransform.position = transform.position;
            groundVelocity = Vector2.zero;
        }
    }

    public void Launch(Vector2 _horizontalVelocity, float _verticalVelocity, float _initialHeight)
    {
        groundVelocity = _horizontalVelocity;
        verticalVelocity = _verticalVelocity;
        bodyTransform.position += Vector3.up * _initialHeight;
        _isGrounded = false;

        OnLaunch?.Invoke();
    }
}
