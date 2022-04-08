using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionCheck : MonoBehaviour
{



    public Transform RootMotion;

    // Update is called once per frame
    void Update()
    {
        RootMotion.transform.position = transform.position;
        RootMotion.transform.rotation = transform.rotation;



    }
}
