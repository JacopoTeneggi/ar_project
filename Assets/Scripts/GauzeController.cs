using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauzeController : MonoBehaviour
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
            transform.localPosition = new Vector3(0.6468f, 0.1301f, -1.0013f);
            transform.localEulerAngles = new Vector3(24.793f, 0f, 0f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "GauzePointOfInterest"))
        {
            isApplied = true;
        }
    }
}
