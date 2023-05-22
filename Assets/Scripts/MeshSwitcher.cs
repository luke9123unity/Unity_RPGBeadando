using UnityEngine;

public class MeshSwitcher : MonoBehaviour
{
    public Mesh[] meshes;
    private int currentMeshIndex = 0;

    void Start()
    {
        // Set the initial mesh
        GetComponent<MeshFilter>().mesh = meshes[currentMeshIndex];
    }

    void Update()
    {
        // Check for mouse wheel input
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel > 0f)
        {
            // Scroll up, select next mesh
            currentMeshIndex++;
            if (currentMeshIndex >= meshes.Length)
            {
                currentMeshIndex = 0;
            }
            GetComponent<MeshFilter>().mesh = meshes[currentMeshIndex];
        }
        else if (mouseWheel < 0f)
        {
            // Scroll down, select previous mesh
            currentMeshIndex--;
            if (currentMeshIndex < 0)
            {
                currentMeshIndex = meshes.Length - 1;
            }
            GetComponent<MeshFilter>().mesh = meshes[currentMeshIndex];
        }
    }
}
