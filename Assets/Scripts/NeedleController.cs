using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class NeedleController : MonoBehaviour, IMixedRealityInputHandler
{
    private IMixedRealityController controller;
    private bool isClicked;

    public void OnInputDown(InputEventData eventData)
    {
        Debug.Log("down on needle");
        IMixedRealityInputSource inputSource = eventData.InputSource;
        Handedness handedness = eventData.Handedness;

        foreach (IMixedRealityController _controller in CoreServices.InputSystem.DetectedControllers)
        {
            if (_controller.ControllerHandedness.Equals(handedness))
            {
                controller = _controller;
                isClicked = true;
            }
        }
    }

    public void OnInputUp(InputEventData eventData)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
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
