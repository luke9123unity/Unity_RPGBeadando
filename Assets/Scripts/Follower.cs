using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] float maxRotationAngle;

    [SerializeField] float bigRange = 15;
    [SerializeField] float smallRange = 10;
    Animator anim;

    float damageTimer = 0f;
    float damageInterval = 3f;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, bigRange);
        Gizmos.DrawWireSphere(transform.position, smallRange);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position, 0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 selfPosition = transform.position;
        Vector3 targetPosition = target.position;
        float distance = Vector3.Distance(selfPosition, targetPosition);
        Vector3 dir = targetPosition - selfPosition;
        if (distance <= bigRange && distance>=smallRange)
        {
            anim.SetBool("attack", false);
            GetComponent<PathMover>().enabled = false;
            float t = Mathf.InverseLerp(bigRange, smallRange, distance);
            float actualSpeed = Mathf.Lerp(0, speed, t);

            if (dir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationAngle * Time.deltaTime);
            }
            transform.position = Vector3.MoveTowards(selfPosition, targetPosition, actualSpeed * Time.deltaTime);
        }
        else if(distance <= smallRange)
        {
            if (damageTimer <= 0f)
            {
                GetComponent<PathMover>().enabled = false;
                anim.SetBool("attack", true);
                GetComponent<Enemy>().DamagePlayer(5);
                damageTimer = damageInterval;
            }
        }
        else
        {
            anim.SetBool("attack", false);
            GetComponent<PathMover>().enabled = true;
        }

        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }

    }
}
