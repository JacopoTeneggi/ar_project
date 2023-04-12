using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class GloveBoxController : MonoBehaviour, IMixedRealityInputHandler
{
    public bool isClicked;

    public void OnInputDown(InputEventData eventData)
    {
        isClicked = true;
    }

    public void OnInputUp(InputEventData eventData)
    {
        isClicked = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
