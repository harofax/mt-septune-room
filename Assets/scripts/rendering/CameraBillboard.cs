using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }
}
