using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedlePointController : MonoBehaviour
{
    public GameObject targetVein;
    public GameObject targetPath;

    public bool isInArm;
    public bool isInVein;

    public float framesInArm;
    public float distanceToVein;
    public float angleToPath;

    // Start is called before the first frame update
    void Start()
    {
        isInArm = false;
        isInVein = false;

        framesInArm = 0;
        distanceToVein = 0f;
        angleToPath = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInArm)
        {
            framesInArm++;
            distanceToVein += Vector3.Distance(targetVein.transform.position, transform.position);
            angleToPath += Vector3.Angle(targetPath.transform.rotation.eulerAngles, transform.rotation.eulerAngles);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "Arm"))
        {
            isInArm = true;
        }

        if (string.Equals(other.gameObject.name, "TargetVein"))
        {
            Debug.Log("in vein");
            isInVein = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (string.Equals(other.gameObject.tag, "Arm"))
        {
            isInArm = false;

            framesInArm = 0;
            distanceToVein = 0f;
            angleToPath = 0f;
        }

        if (string.Equals(other.gameObject.tag, "TargetVein"))
        {
            isInVein = false;
        }
    }
}
