using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_character : MonoBehaviour
{
    public float gravity = 20f;
    Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private float vSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        vSpeed -= gravity * Time.deltaTime;
        moveDirection.y = vSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
        if (characterController.isGrounded)
        {
            vSpeed = -1;
        }
    }

    public void Jump(float jump_power)
    {
        vSpeed = jump_power;
    }
}
