using System.Collections;
using Game;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] private ThrowableObject _throwableObject;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float damage = 5f;

    private void OnEnable() {
        _throwableObject = GetComponent<ThrowableObject>();
        _throwableObject.OnGrounded += Hit;
    }
    public void Hit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyStats enemyStats = collider.GetComponent<EnemyStats>();
                enemyStats?.Hit(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
