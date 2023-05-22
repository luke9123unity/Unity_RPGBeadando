using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] Slider healthBar;
    [SerializeField] Canvas healthBarCanvas;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] LootItem[] lootTable;
    [SerializeField] string enemyType="wolf";

    Animator anim;

    void Awake()
    {
        GetComponent<PathMover>().enabled = true;
        GetComponent<Follower>().enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponentInChildren<Renderer>().enabled = true;
        healthBarCanvas.enabled= true;
    }
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void Update()
    {
        healthBarCanvas.transform.position = transform.position + new Vector3(0f, 2f, 0f);

        healthBarCanvas.transform.LookAt(camera.GetComponent<Camera>().transform);

        healthBarCanvas.transform.Rotate(new Vector3(0f, 180f, 0f));

        //for test only
        /*if (Input.GetKeyDown("space"))
        {
            TakeDamage(10);
        }*/
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            GetComponent<PathMover>().enabled = false;
            GetComponent<Follower>().enabled = false;
            anim.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            Invoke("Die",3f);
        }
    }

    void Die()
    {
        /*foreach (LootItem item in lootTable)
        {
            int roll = Random.Range(0, 100); // roll a number between 0 and 99
            if (roll < item.dropChance)
            {
                Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
            }
        }*/
        // Play death animation and remove enemy object
        PlayerAttributes.GainXP(100);
        PlayerAttributes.KillCountRaise();
        PlayerAttributes.LastKilled(enemyType);
        float lateRespawnTime = Random.Range(1f, 60f);
        GetComponentInChildren<Renderer>().enabled = false;
        healthBarCanvas.enabled = false;
        Invoke("LateRespawn", lateRespawnTime);
        //Destroy(gameObject);
    }

    void LateRespawn()
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void DamagePlayer(int damage)
    {
        PlayerAttributes.playerHP -= damage;
        Debug.Log(PlayerAttributes.playerHP);
        //PlayerAttributes.healthBar.value = PlayerAttributes.playerHP;
    }
}