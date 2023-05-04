using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class NeedleController : MonoBehaviour, IMixedRealityInputHandler
{
    public GameObject tubeAttachment;
    public NeedlePointController needlePointController;

    private IMixedRealityController controller;
    public bool isClicked;
    public bool toExtract;

    public void OnInputDown(InputEventData eventData)
    {
        if ((!isClicked && !needlePointController.isInVein) || (!isClicked && toExtract))
        {
            IMixedRealityInputSource inputSource = eventData.InputSource;
            Handedness handedness = eventData.Handedness;

            foreach (IMixedRealityController _controller in CoreServices.InputSystem.DetectedControllers)
            {
                if (_controller.ControllerHandedness.Equals(handedness))
                {
                    controller = _controller;
                    isClicked = true;
                    CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
                }
            }
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        isClicked = false;
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);

        if (!needlePointController.isInVein)
        {
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject rotationCenter = transform.GetChild(0).gameObject;
        GameObject needle = rotationCenter.transform.GetChild(0).gameObject;
        GameObject needlePoint = needle.transform.GetChild(0).gameObject;

        tubeAttachment.SetActive(false);

        needlePointController = needlePoint.GetComponent<NeedlePointController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicked)
        {
            transform.position = controller.Visualizer.GameObjectProxy.transform.position;
            transform.eulerAngles = controller.Visualizer.GameObjectProxy.transform.eulerAngles;
        }
    }
}
