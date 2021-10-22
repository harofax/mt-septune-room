using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class hoverfollow : MonoBehaviour
{

    public GameObject target;
    public float speed = 1f;
    float height;
    // Start is called before the first frame update
    void Start()
    {
        height = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lerppos = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        lerppos.y = height;
        transform.position = lerppos;
    }
}
