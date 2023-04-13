using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public GameObject tourniquet;
    public GameObject gloveBox;
    public GameObject needle;
    public GameObject scanner;
    public GameObject gauze;
    public GameObject tube;

    private List<Goal> goals = new List<Goal>();
    private int currentGoalIdx = 0;
    private Goal currentGoal;
    private GatherMaterials currentGoal_test;

    void Start()
    {
        //currentGoal = GetComponent<Goal>();
        currentGoal_test = new GatherMaterials(tourniquet, scanner, gauze, needle, tube);
        PutGlovesOn gloves = new PutGlovesOn(gloveBox);
        // Debug.Log(typeof(currentGoal_test));
        currentGoal = currentGoal_test;
        Debug.Log(currentGoal);
        // Debug.Log(typeof(currentGoal));
        goals.Add(currentGoal);
        //goals.Add(gloves);
        currentGoal = goals[0];
        Debug.Log(currentGoal);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (currentGoal.IsAchieved())
        {
            currentGoal.Complete();
            Destroy(currentGoal);
            currentGoalIdx++;
        }
        else
        {
            currentGoal.DrawHUD();
        }
    } 
}

public abstract class  Goal : MonoBehaviour
{
    public abstract bool IsAchieved();
    public abstract void Complete();
    public abstract void DrawHUD();
}

public class GatherMaterials : Goal
{
    private GameObject tourniquet;
    private GameObject scanner;
    private GameObject gauzes;
    private GameObject tubes;
    private GameObject needle;

    private TrolleyCollision tourniquetCollide;
    private TrolleyCollision scannerCollide;
    private TrolleyCollision gauze1Collide;
    private TrolleyCollision gauze2Collide;
    private TrolleyCollision gauze3Collide;
    private TrolleyCollision tube1Collide;
    private TrolleyCollision tube2Collide;
    private TrolleyCollision tube3Collide;
    private TrolleyCollision needleCollide;


    public GatherMaterials(GameObject _tourniquet, GameObject _scanner, GameObject _gauze, GameObject _needle, GameObject _tube)
    {

        tourniquet = _tourniquet;
        scanner = _scanner;
        gauzes = _gauze;
        needle = _needle;
        tubes = _tube;

        tourniquetCollide = tourniquet.transform.GetChild(0).GetComponent<TrolleyCollision>();
        scannerCollide = scanner.transform.GetChild(0).GetComponent<TrolleyCollision>();
        gauze1Collide = gauzes.transform.GetChild(0).GetComponent<TrolleyCollision>();
        gauze2Collide = gauzes.transform.GetChild(1).GetComponent<TrolleyCollision>();
        gauze3Collide = gauzes.transform.GetChild(2).GetComponent<TrolleyCollision>();
        tube1Collide = tubes.transform.GetChild(0).GetComponent<TrolleyCollision>();
        tube2Collide = tubes.transform.GetChild(1).GetComponent<TrolleyCollision>();
        tube3Collide = tubes.transform.GetChild(3).GetComponent<TrolleyCollision>();
        needleCollide = needle.transform.GetChild(0).GetComponent<TrolleyCollision>();
    }

    public override bool IsAchieved()
    {
        return (tourniquetCollide.collision_trolley == 1 && scannerCollide.collision_trolley == 1 && (gauze1Collide.collision_trolley == 1 || gauze2Collide.collision_trolley == 1 || gauze3Collide.collision_trolley == 1) && tube1Collide.collision_trolley == 1 && tube2Collide.collision_trolley == 1 && tube3Collide.collision_trolley == 1 && needleCollide.collision_trolley == 1);
    }

    public override void Complete()
    {

    }

    public override void DrawHUD()
    {
        int obj = tourniquetCollide.collision_trolley + scannerCollide.collision_trolley + needleCollide.collision_trolley + tube1Collide.collision_trolley + tube2Collide.collision_trolley + tube3Collide.collision_trolley + Math.Max(Math.Max(gauze1Collide.collision_trolley, gauze2Collide.collision_trolley), gauze3Collide.collision_trolley);
        int required_obj = 7;
        GUILayout.Label(string.Format("Gathered {0}/{1} materials", obj, required_obj));
    }

}

public class PutGlovesOn: Goal
{
    private GameObject gloveBox;
    private GloveBoxController gloveBoxController;

    public PutGlovesOn(GameObject _glovebox)
    {
        gloveBox = _glovebox;
        gloveBoxController = gloveBox.transform.GetChild(0).GetComponent<GloveBoxController>();
    }

    public override bool IsAchieved()
    {

        return gloveBoxController.isClicked;
    }

    public override void Complete()
    {
        Destroy(gloveBox);
    }

    public override void DrawHUD()
    {

    }
}