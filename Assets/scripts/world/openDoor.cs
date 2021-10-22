using UnityEngine;

public class openDoor : MonoBehaviour
{
    // Setup

    [HideInInspector]
    private bool inRange;
    



    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
        //if (doorAnimator.GetBool("isOpening"))
        //{
        //    doorAnimator.SetBool("isOpening", false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Interact") && inRange)
        {
        //    doorAnimator.SetBool("isOpening", true);
        }
    }
}
