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
        
    }

    private void FlipApplication()
    {
        isApplied = !isApplied;
    }

    void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "TourniquetPointOfInterest"))
        {
            FlipApplication();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "TourniquetPointOfInterest"))
        {
            FlipApplication();
        }
    }

}
