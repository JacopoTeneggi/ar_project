using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyCollision : MonoBehaviour
{
    public int trolleyCollision;

    void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "Trolley"))
        {
            trolleyCollision = 1;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "Trolley"))
        {
            trolleyCollision = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        trolleyCollision = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
