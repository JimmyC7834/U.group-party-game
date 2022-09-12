using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyRouteNode : MonoBehaviour
    {
        [SerializeField] private EnemyRouteNode[] NextNodes;
        private Queue<EnemyRouteNode> nextNodes;

        private void Awake()
        {
            nextNodes = new Queue<EnemyRouteNode>();
            foreach (EnemyRouteNode node in NextNodes)
            {
                nextNodes.Enqueue(node);
            }
        }

        public EnemyRouteNode GetNextNode()
        {
            if (nextNodes.Count == 0) return null;

            nextNodes.Enqueue(nextNodes.Dequeue());
            return nextNodes.Peek();
        }

#if UNITY_EDITOR
        // update editor value
        private void OnValidate()
        {
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
            Gizmos.DrawSphere(transform.position, .25f);

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