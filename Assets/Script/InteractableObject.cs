using UnityEngine;
using UnityEngine.Events;

using Game.Player;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour
    {
        public struct InteractInfo
        {
            public PlayerInteractControl interactor { get; private set; }
            public ThrowableObject pickedObject { get; private set; }

            public static InteractInfo From(PlayerInteractControl _interactor, ThrowableObject _pickedObject) => new InteractInfo {
                interactor = _interactor,
                pickedObject = _pickedObject,
            };
        }

        public UnityAction<InteractInfo> OnInteracted;

        public void Interact(InteractInfo info) => OnInteracted?.Invoke(info);
    }
}