using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motion_detector : MonoBehaviour
{

    private Animator doorAnimator;

    private bool animateDoor;
    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            doorAnimator.SetBool("motion_detected", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            doorAnimator.SetBool("motion_detected", false);
        }
    }
    
}
