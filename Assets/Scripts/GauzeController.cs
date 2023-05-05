using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauzeController : MonoBehaviour
{
    public bool isApplied;
    public bool alcohol;
    public bool isSwabbed;
    // Start is called before the first frame update
    void Start()
    {
        isApplied = false;
        alcohol = false;
        isSwabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isApplied)
        {
            transform.localPosition = new Vector3(0.6468f, 0.1301f, -1.0013f);
            transform.localEulerAngles = new Vector3(24.793f, 0f, 0f);
        }

        if (isSwabbed)
        {
            transform.localPosition = new Vector3(0.6468f, 0.1301f, -1.0013f);
            transform.localEulerAngles = new Vector3(24.793f, 0f, 0f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "GauzePointOfInterest"))
        {
            isApplied = true;
        }

        if (string.Equals(other.gameObject.tag, "Alcohol"))
	    {
	        alcohol = true;
	    }

	    if (string.Equals(other.gameObject.tag, "SwabPointOfInterest"))
	    {
	        isSwabbed = true;
	    } 
    }

    void OnCollisionExit(Collision other)
    {
	    if (string.Equals(other.gameObject.tag, "SwabPointOfInterest"))
	    {
	        isSwabbed = false;
	    }

    }
}
