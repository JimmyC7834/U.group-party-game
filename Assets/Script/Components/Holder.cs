using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    /// <summary>
    /// Holder for throwables
    /// </summary>
    public class Holder : MonoBehaviour
    {
        [SerializeField] private Throwable _throwable;
        [SerializeField] private Transform _holderTrans;

        public UnityAction<Throwable> OnHold = delegate { };
        public UnityAction<Throwable> OnRelease = delegate { };

        public bool IsHolding() => _throwable != null;
        public Transform HolderTrans => _holderTrans;

        public Throwable GetHoldingObject() => _throwable;

        public bool Hold(Throwable throwable)
        {
            if (_throwable != null) return false;
            _throwable = throwable;
            _throwable.transform.SetParent(_holderTrans, true);
            _throwable.HoldBy(this);

            OnHold.Invoke(_throwable);
            return true;
        }

        public void Throw(Vector2 dir, float magnitude, float initialHeight)
        {
            if (_throwable == null) return;
            _throwable.Launch(dir * magnitude, magnitude, initialHeight);
            _throwable.ThrowBy(this);
            Release();
        }

        public Throwable Release()
        {
            if (_throwable == null) return null;
            _throwable.ReleaseBy(this);
            _throwable.transform.SetParent(null);

            Throwable drop = _throwable;
            _throwable = null;
            OnRelease.Invoke(drop);
            return drop;
        }
    }
}