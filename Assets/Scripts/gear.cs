using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gear : MonoBehaviour
{
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
