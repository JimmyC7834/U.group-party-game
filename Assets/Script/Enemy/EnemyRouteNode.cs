using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyRouteNode : MonoBehaviour
    {
        [SerializeField] private EnemyRouteNode[] NextNodes;
        private Queue<EnemyRouteNode> nextNodes;

        private void OnTriggerStay2D(Collider2D other)
        {
            // skip if not enemy or if enemy is too far
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy == null || Vector2.Distance(transform.position, enemy.transform.position) > .01f)
                return;

            // destory and return enemy to pool if this is the last node
            // if (nextNodes.Count == 0)
            // {
            //     enemy.ReturnToPool();
            //     return;
            // }

            Debug.Log($"redirected {enemy.name} to {nextNodes.Peek()} : {nextNodes.Peek().transform.position}");
            enemy.transform.position = transform.position;
            enemy.RedirectTo(nextNodes.Peek().transform);
            nextNodes.Enqueue(nextNodes.Dequeue());
        }

#if UNITY_EDITOR
        // update editor value
        private void OnValidate() {
            if (NextNodes == null)
                return;

            nextNodes = new Queue<EnemyRouteNode>();
            foreach (EnemyRouteNode node in NextNodes)
            {
                nextNodes.Enqueue(node);
            }
        }

        // draw debug line
        private void OnDrawGizmosSelected()
        {
            if (nextNodes == null)
                return;

            for (int i = 0; i < nextNodes.Count; i++)
            {
                if (nextNodes.Peek() == null)
                {
                    Debug.LogWarning($"null node found in enemy route!");
                    return;
                }

                Gizmos.DrawLine(transform.position, nextNodes.Peek().transform.position);
                nextNodes.Enqueue(nextNodes.Dequeue());
            }
        }
#endif
    }
}