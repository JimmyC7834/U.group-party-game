using System.Linq;
using Game.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Interactor : MonoBehaviour
    {
        /// <summary>
        /// return the interactor and interactable when the interactor interacted with something
        /// </summary>
        public event UnityAction<Interactor, Interactable> OnInteract = delegate { };

        public void Interact(Interactable interactable)
        {
            if (interactable == null) return;
            if (!interactable.interactable) return;
            interactable.Interact(this);
            OnInteract.Invoke(this, interactable);
        }

        public Interactable GetInteractable(Vector3 from, Vector3 dir, float dist)
        {
            // raycast and check for interactions
            RaycastHit2D[] hits = Physics2D.RaycastAll(from, dir, dist, LayerMask.GetMask("Default"));

            Debug.DrawRay(from, dir * dist, Color.green, .1f);
            RaycastHit2D hit = hits.FirstOrDefault(x => x.collider != null && !x.collider.isTrigger);

            if (hit.collider == null) return null;

            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactable == null) return null;

            return interactable;
        }
    }
}