using System.Collections;
using UnityEngine;

class SpawnEffectOnClick : MonoBehaviour
{
    [SerializeField] GameObject effectPrefab;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float effectScale = 1f;
    [SerializeField] float effectDuration = 2f;
    [SerializeField] float radius = 3f;
    bool effectIsOn = false;
    float damageInterval = 1f;
    float damageTimer = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<Animator>().SetTrigger("magic");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                GameObject newEffect = Instantiate(effectPrefab, hit.point, Quaternion.identity);
                newEffect.transform.localScale = Vector3.one * effectScale;
                SphereCollider collider = newEffect.AddComponent<SphereCollider>();
                collider.radius = radius;
                effectIsOn = true;
                transform.LookAt(newEffect.transform.position);
                Invoke("ResetMagicSpawn", effectDuration);
                Destroy(newEffect, effectDuration);
                
            }
        }

        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }

        if (effectIsOn && damageTimer <= 0f)
        {
            DamageEnemy();
            damageTimer = damageInterval;
        }
    }

    private void ResetMagicSpawn()
    {
        effectIsOn = false;
        GetComponentInChildren<SpawnEffectOnClick>().enabled = false;
        GetComponentInChildren<PlayerAttributes>().KillCountNull();
    }

    void DamageEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemyHealth = hitCollider.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10);
            }
        }
    }
}