using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCDetect : MonoBehaviour
{
    [SerializeField] float detectionRadius = 5f;
    [SerializeField] Color detectionColor = Color.green;
    [SerializeField] GameObject targetObject;
    [SerializeField] GameObject dialogBox;
    [SerializeField] public Light light;
    [SerializeField] public string enemyType = "wolf";
    [SerializeField] public int howMany = 5;
    [SerializeField] public string actionType = "kill";
    [SerializeField] public int loopCount = 0;

    SphereCollider sphereCollider;
    public bool heroReturn = false;
    public bool isColliding = false;

    void Awake()
    {
        light.enabled = true;
        light.color = Color.yellow;
        light.intensity = 2;
        isColliding = false;
        heroReturn = false;
        sphereCollider = GetComponent<SphereCollider>();

        if (sphereCollider == null)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
        }

        sphereCollider.radius = detectionRadius;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = detectionColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == targetObject)
        {
            Debug.Log("Target object entered detection radius!");
            other.gameObject.GetComponentInChildren<Animator>().enabled = false;
            other.gameObject.GetComponentInChildren<Camera>().enabled = true;
            other.gameObject.GetComponentInChildren<CharacterController>().enabled = false;
            //other.gameObject.GetComponentInChildren<PlayerAttack>().enabled = false;
            //dialogBox.GetComponentInChildren<TextDisplay>().enabled = true;
            transform.LookAt(targetObject.transform);
            isColliding = true;
            //gameObject.GetComponent<CameraBlender>().enabled = true;
            light.enabled= false;
        }

    }

    public void ActivateBeacon()
    {
        light.enabled = true;
        if (heroReturn == true)
        {
            light.color = Color.green;
        }
        if (heroReturn == false)
        {
            light.color = Color.yellow;
        }
    }

}