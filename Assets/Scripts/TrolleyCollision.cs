using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyCollision : MonoBehaviour
{
    public int trolleyCollision = 0;

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (string.Equals(other.gameObject.tag, "Trolley"))
        {
            trolleyCollision = 1;
            Debug.Log(trolleyCollision);
        }
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log(other);
        if (string.Equals(other.gameObject.tag, "Trolley"))
        {
            trolleyCollision = 0;
            Debug.Log(trolleyCollision);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
