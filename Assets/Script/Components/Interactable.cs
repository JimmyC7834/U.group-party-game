using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Interactable : MonoBehaviour
    {
        public bool interactable { get; private set; }
        public event UnityAction<Interactor> OnInteracted = delegate { };

        public void Interact(Interactor player)
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