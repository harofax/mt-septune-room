using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class respawn : MonoBehaviour
{
    public Transform respawnPoint;
    private CharacterController controller;
    private Vector3 teleport;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        teleport = respawnPoint.position - transform.position;
    }
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {

            controller.enabled = false;
            Debug.Log("about to respawn the player (currently located at " + transform.position + ") at the position " + respawnPoint.position);
            transform.position = respawnPoint.position;
            Debug.Log("player has been respawned at " + transform.position);
            controller.enabled = true;
        }
    }
}
