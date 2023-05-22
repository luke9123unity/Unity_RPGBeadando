using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDelay = 1f;

    float timeSinceLastAttack;

    void Update()
    {
        if (timeSinceLastAttack >= attackDelay && Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    void Attack()
    {
        animator.SetTrigger("attack");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemyHealth = hitCollider.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }
        }
        GetComponent<CharacterController>().enabled = false;
        timeSinceLastAttack = 0f;
        Invoke("EnableCharacterMove",2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void EnableCharacterMove()
    {
        GetComponent<CharacterController>().enabled = true;
    }
}