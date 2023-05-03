using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinScannerScreenController : MonoBehaviour
{
    public GameObject veinScannerScreen;
    public bool collisionWithVein;

    // Start is called before the first frame update
    void Start()
    {
        veinScannerScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "Arm"))
        {
            veinScannerScreen.SetActive(true);
        }
        if (string.Equals(other.gameObject.tag, "VeinScannerPointOfInterest"))
        {
            collisionWithVein = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("collision exit");
        if (string.Equals(other.gameObject.tag, "Arm"))
        {
            veinScannerScreen.SetActive(false);
        }
        if (string.Equals(other.gameObject.tag, "VeinScannerPointOfInterest"))
        {
            collisionWithVein = false;
        }
    }
}
