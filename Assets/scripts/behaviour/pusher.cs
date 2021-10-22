using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pusher : MonoBehaviour
{
    public float force;
    public float sightDistance;
    private CharacterController target;
    private Vector3 movementDirection = Vector3.zero;
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            RaycastHit hit;
            Ray pushRay = new Ray(this.transform.position, transform.forward);

            if (Physics.Raycast (pushRay, out hit, sightDistance) && hit.collider.gameObject.tag == "Pushable")
            {
                target = hit.collider.gameObject.GetComponent<CharacterController>();
                movementDirection = pushRay.direction * force;
                hit.collider.gameObject.GetComponent<NPC_character>().Jump(3f);
            }
            
        }

        if (target != null)
        {
            //movementDirection = Vector3.MoveTowards(movementDirection, Vector3.zero, speed * Time.deltaTime);
            target.Move(movementDirection * Time.deltaTime);
        }
        
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            movementDirection = Vector3.MoveTowards(movementDirection, Vector3.zero, speed * Time.fixedDeltaTime);
        }
    }


}
