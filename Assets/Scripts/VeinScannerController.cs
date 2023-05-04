using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class VeinScannerController : MonoBehaviour, IMixedRealityInputHandler
{
    public VeinScannerScreenController screenController;
    public float collisionTime;

    private IMixedRealityController controller;
    public bool isClicked;

    public void OnInputDown(InputEventData eventData)
    {
        if (!isClicked)
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
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject rotationCenter = transform.GetChild(0).gameObject;
        GameObject scanner = rotationCenter.transform.GetChild(0).gameObject;
        GameObject FOV = scanner.transform.GetChild(0).gameObject;

        screenController = FOV.GetComponent<VeinScannerScreenController>();
        collisionTime = 0.0f;
        isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicked)
        {
            transform.position = controller.Visualizer.GameObjectProxy.transform.position;
            transform.eulerAngles = controller.Visualizer.GameObjectProxy.transform.eulerAngles;
        }

        if (screenController.collisionWithVein)
        {
            collisionTime += Time.deltaTime;
        }
        else
        {
            collisionTime = 0.0f;
        }
    }
}
