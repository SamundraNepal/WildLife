using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testboi : MonoBehaviour
{
    Mesh mesh;
   public Vector3[] V;



    private void Start()
    {

        mesh = GetComponent<MeshFilter>().sharedMesh;

        V = mesh.vertices;
    }

    private void Update()
    {
        

       
        


    }

}
