using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private Transform visualTrans;

        private void OnEnable()
        {
            GetComponent<PlayerControl>().OnMove += RotateVisuals;
        }

        private void RotateVisuals(Vector2 moveDir)
        {
            visualTrans.rotation = Quaternion.LookRotation(Vector3.back, moveDir);
        }
    }
}