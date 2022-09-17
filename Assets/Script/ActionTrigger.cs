using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class ActionTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Trigger() => _action.Invoke();
    }
}