using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float rotationSpeed = 5.0f;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        // Get input from keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        
        // Calculate movement direction based on input
        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        if(moveDirection!=Vector3.zero)
        {
            anim.SetBool("moveButtonsPressed",true);
        }
        else
        {
            anim.SetBool("moveButtonsPressed", false);
        }

        // Move the character in the direction of input
        transform.position += moveDirection * speed * Time.deltaTime;

        // Rotate the character to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }
}
