using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourniquetController : MonoBehaviour
{
    public bool isApplied;
    
    // Start is called before the first frame update
    void Start()
    {
        isApplied = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isApplied)
        {
            transform.localPosition = new Vector3(0.4362f, 0.1115f, -1.0087f);
            transform.rotation = Quaternion.Euler(0.0f, -90.0f, 90.0f);
        }    
    }
 
    void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "TourniquetPointOfInterest"))
        {
            isApplied = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "TourniquetPointOfInterest"))
        {
            isApplied = false;
        }
    }

}
