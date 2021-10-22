using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class teleport_up : MonoBehaviour
{
    private bool hasCollided = false;
    

    public Transform tp_point;

    public Transform player;

    public GameObject tpText;
    private CharacterController playerCC;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCC = player.gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasCollided) return;

        if (Input.GetKey(KeyCode.E))
        {
            playerCC.enabled = false;
            player.position = tp_point.position;
            hasCollided = false;
            tpText.SetActive(false);
            playerCC.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tpText.SetActive(true);
            hasCollided = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        tpText.SetActive(false);
        hasCollided = false;
    }
}
