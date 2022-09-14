using UnityEngine;
using UnityEngine.Events;

public class FakeHeightObject : MonoBehaviour
{
    [SerializeField] protected Transform bodyTransform;
    [SerializeField] protected Transform shadowTransform;

    [Header("Physics Values")] [SerializeField]
    protected float gravity;

    [SerializeField] protected Vector2 windVelocity = default;
    [SerializeField] protected Vector2 _groundVelocity;
    [SerializeField] protected float _verticalVelocity;
    [SerializeField] protected bool _isGrounded;

    public float height => bodyTransform.localPosition.y;
    public Vector2 groundVelocity => _groundVelocity;
    public float verticalVelocity => _verticalVelocity;

    public event UnityAction OnGrounded;
    public event UnityAction OnLaunch;

    public virtual bool IsGrounded
    {
        get { return _isGrounded; }

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

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    private void Update()
    {
        CheckGroundHit();
    }

    protected virtual void UpdatePhysics()
    {
        if (IsGrounded) return;
        _verticalVelocity += gravity * Time.deltaTime;
        bodyTransform.position += _verticalVelocity * Time.deltaTime * Vector3.up;
        _groundVelocity += windVelocity;
        transform.position += (Vector3) (groundVelocity) * Time.deltaTime;
    }

    protected virtual void CheckGroundHit()
    {
        if (bodyTransform.position.y <= transform.position.y && !IsGrounded)
        {
            IsGrounded = true;
            bodyTransform.position = transform.position;
            _groundVelocity = Vector2.zero;
        }
    }

    public void Launch(Vector2 _horizontalVelocity, float _verticalVelocity, float _initialHeight)
    {
        _groundVelocity = _horizontalVelocity;
        this._verticalVelocity = _verticalVelocity;
        bodyTransform.position += Vector3.up * _initialHeight;
        _isGrounded = false;

        OnLaunch?.Invoke();
    }

    public float GetGravity()
    {
        return gravity;
    }

    public void SetGroundVelocity(Vector2 groundVelocity)
    {
        _groundVelocity = groundVelocity;
    }

    public void SetVerticalVelocity(float value)
    {
        _verticalVelocity = value;
    }
}