using UnityEngine;

public class CameraBlender : MonoBehaviour
{
    [SerializeField] Camera sourceCamera;
    [SerializeField] Camera targetCamera;
    [SerializeField] float blendTime = 1f;

    Camera blendCamera;
    float blendTimer = 0f;

    void Awake()
    {
        // Create a new camera to use for blending
        blendCamera = new GameObject("Blend Camera").AddComponent<Camera>();
        blendCamera.enabled = false;
    }

    void LateUpdate()
    {
        if (blendTimer < blendTime)
        {
            // Calculate the blend weight based on the current blend timer
            float blendWeight = blendTimer / blendTime;

            // Set the blendCamera's transform to blend between the sourceCamera and targetCamera's transforms
            blendCamera.transform.position = Vector3.Lerp(sourceCamera.transform.position, targetCamera.transform.position, blendWeight);
            blendCamera.transform.rotation = Quaternion.Slerp(sourceCamera.transform.rotation, targetCamera.transform.rotation, blendWeight);

            // Set the blendCamera's projection matrix to interpolate between the sourceCamera and targetCamera's projection matrices
            Matrix4x4 sourceMatrix = sourceCamera.projectionMatrix;
            Matrix4x4 targetMatrix = targetCamera.projectionMatrix;
            Matrix4x4 blendMatrix = new Matrix4x4();
            for (int i = 0; i < 16; i++)
            {
                blendMatrix[i] = Mathf.Lerp(sourceMatrix[i], targetMatrix[i], blendWeight);
            }
            blendCamera.projectionMatrix = blendMatrix;

            // Set the blendCamera's near and far clip planes to interpolate between the sourceCamera and targetCamera's clip planes
            blendCamera.nearClipPlane = Mathf.Lerp(sourceCamera.nearClipPlane, targetCamera.nearClipPlane, blendWeight);
            blendCamera.farClipPlane = Mathf.Lerp(sourceCamera.farClipPlane, targetCamera.farClipPlane, blendWeight);

            // Set the blendCamera's field of view to interpolate between the sourceCamera and targetCamera's field of view
            blendCamera.fieldOfView = Mathf.Lerp(sourceCamera.fieldOfView, targetCamera.fieldOfView, blendWeight);

            blendTimer += Time.deltaTime;
        }
        else
        {
            // Once the blend is complete, disable the blendCamera and enable the targetCamera
            blendCamera.enabled = false;
            targetCamera.enabled = true;

            // Reset the blend timer
            blendTimer = 0f;
        }
    }

    public void StartBlend()
    {
        // Disable the sourceCamera and enable the blendCamera
        sourceCamera.enabled = false;
        blendCamera.enabled = true;

        // Set the blendCamera's transform and projection matrix to match the sourceCamera
        blendCamera.transform.position = sourceCamera.transform.position;
        blendCamera.transform.rotation = sourceCamera.transform.rotation;
        blendCamera.projectionMatrix = sourceCamera.projectionMatrix;

        // Set the blendCamera's near and far clip planes, and field of view to match the sourceCamera
        blendCamera.nearClipPlane = sourceCamera.nearClipPlane;
        blendCamera.farClipPlane = sourceCamera.farClipPlane;
        blendCamera.fieldOfView = sourceCamera.fieldOfView;

        // Enable the script and start the blend
        enabled = true;
        blendTimer = 0f;
    }
}
