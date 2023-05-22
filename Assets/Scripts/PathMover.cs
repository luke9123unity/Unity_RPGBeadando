using System.Collections.Generic;
using UnityEngine;

public class PathMover : MonoBehaviour
{
    [SerializeField] List<Transform> points;
    [SerializeField] float speed;
    [SerializeField] float maxRotationAngle;

    int currentIndex = 0;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (Transform point in points)
        {
            if (point != null)
            {
                Gizmos.DrawSphere(point.position, 0.1f);
            }
        }
    }

    void Update()
    {
        if (points.Count == 0) return;

        if (currentIndex >= points.Count)
        {
            currentIndex = 0;
            List<Transform> randomList = new List<Transform>();

            while (points.Count > 0)
            {
                int randomIndex = Random.Range(0, points.Count);
                randomList.Add(points[randomIndex]);
                points.RemoveAt(randomIndex);
            }
            points = randomList;
        }

        Transform target = points[currentIndex];

        if (target == null)
        {
            currentIndex++;
            target = points[currentIndex];
            Debug.LogError("Missing Path Point!");
        }
        if (target == null) return;

        Vector3 selfPos = transform.position;
        Vector3 targetPos = target.position;

        Vector3 dir = targetPos - selfPos;


        transform.position = Vector3.MoveTowards(selfPos, targetPos, speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationAngle * Time.deltaTime);
        }

        if (transform.position == targetPos)
        {
            currentIndex++;
        }
    }
}
