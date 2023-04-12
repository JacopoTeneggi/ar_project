using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gauze;
    public GameObject tube;
    public GameObject catheter;
    public GameObject veinScanner;
    public GameObject tourniquet;
    public GameObject gloveBox;
    private Goal glovesOn;

    private void Awake()
    {
        PutGLovesOn glovesOn = new PutGLovesOn(gloveBox);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public abstract class Goal : MonoBehaviour
{
    public abstract bool IsAchieved();
}

public class PutGLovesOn : Goal
{
    public GameObject gloveBox;

    public PutGLovesOn(GameObject _gloveBox)
    {
        gloveBox = _gloveBox;
    }

    public override bool IsAchieved()
    {
        return false;
    }
}
