using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class TestTubeController : MonoBehaviour, IMixedRealityInputHandler
{
    public GameObject bloodContainer;
    private Vector3 initScale = new Vector3(1f, 0f, 1f);
    private Vector3 endScale = new Vector3(1f, 4.75f, 1f);

    public int interpolationFrameCount = 200;
    private int elapsedFrame = 0;

    public GameObject needlePoint;
    public GameObject tubeAttachment;

    private NeedlePointController needlePointController;
    public bool isAttached;
    public bool isFilled;

    // Start is called before the first frame update
    void Start()
    {
        needlePointController = needlePoint.GetComponent<NeedlePointController>();
        isAttached = false;
        isFilled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttached)
        {
            transform.position = tubeAttachment.transform.position;
            transform.eulerAngles = tubeAttachment.transform.eulerAngles;

            if (needlePointController.isInVein)
            {
                if (elapsedFrame < interpolationFrameCount)
                {
                    float interpRatio = (float)elapsedFrame / interpolationFrameCount;
                    Vector3 interpScale = Vector3.Lerp(initScale, endScale, interpRatio);
                    bloodContainer.transform.localScale = interpScale;

                    elapsedFrame++;
                }
                else
                {
                    isFilled = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (string.Equals(other.gameObject.name, "TubeAttachment"))
        {
            isAttached = true;
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (isAttached)
        {
            isAttached = false;
        }
    }

    public void OnInputUp(InputEventData eventData)
    {

    }
}
