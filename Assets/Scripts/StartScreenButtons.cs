using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenButtons : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] GameObject[] buttonsToHide;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float rotationSpeed = 90f;
    [SerializeField] float rotationDuration = 2f;
    [SerializeField] TextMeshProUGUI textToDisplay;
    [SerializeField] GameObject backButton;

    private bool isCameraRotating = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float rotationTimer = 0f;

    private void Start()
    {
        backButton.SetActive(false);
        textToDisplay.enabled = false;
        initialRotation = cameraTransform.rotation;
        targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x + 90f, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
    }

    private void Update()
    {
        if (isCameraRotating)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);

            cameraTransform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            if (t >= 1f)
            {
                isCameraRotating = false;
                rotationTimer = 0f;
                
            }
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ToggleButtons()
    {
        SwapButtons();
        RotateCamera(true);
        Invoke("InfoDisplay", rotationDuration);
    }

    public void RotateCamera(bool isDown)
    {
        if (!isCameraRotating)
        {
            isCameraRotating = true;
            initialRotation = cameraTransform.rotation;
            if(isDown)
                targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x + 90f, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
            else
                targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x - 90f, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
        }
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    public void InfoDisplay()
    {
        textToDisplay.enabled = true;
        backButton.SetActive(true);
    }

    public void RotateBack()
    {
        textToDisplay.enabled = false;
        backButton.SetActive(false);
        RotateCamera(false);
        Invoke("SwapButtons", rotationDuration);
    }

    public void SwapButtons()
    {
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(!button.activeSelf);
        }
    }
}
