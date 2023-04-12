using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyCollision : MonoBehaviour
{
    public int collision_trolley = 0;

    void OnCollisionEnter(Collision col)
    {
        if (string.Equals(col.gameObject.tag, "Trolley"))
        {
            collision_trolley = 1;
            Debug.Log(collision_trolley);
        }
    }

    void OnCollisionExit(Collision col)
    {
        Debug.Log(col);
        if (string.Equals(col.gameObject.tag, "Trolley"))
        {
            collision_trolley = 0;
            Debug.Log(collision_trolley);
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
