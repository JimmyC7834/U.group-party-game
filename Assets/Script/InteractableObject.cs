using Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableObject : MonoBehaviour
    {
        public bool interactable { get; private set; }
        public event UnityAction<PlayerInteractControl> OnInteracted = delegate { };

        public void Interact(PlayerInteractControl player)
        {
            if (!interactable) return;
            OnInteracted.Invoke(player);
        }

        public void SetInteractable(bool value)
        {
            interactable = value;
        }
    }
}