using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject tourniquet;
    public GameObject gloveBox;

    private List<Goal> goals = new List<Goal>();
    private int currentGoalIdx = 0;
    private Goal currentGoal;
    private GatherMaterials currentGoal_test;

    void Start()
    {
        //currentGoal = GetComponent<Goal>();
        currentGoal_test = new GatherMaterials(tourniquet);
        PutGlovesOn gloves = new PutGlovesOn(gloveBox);
        Debug.Log(currentGoal_test.val);
        // Debug.Log(typeof(currentGoal_test));
        currentGoal = currentGoal_test;
        Debug.Log(currentGoal);
        // Debug.Log(typeof(currentGoal));
        goals.Add(currentGoal);
        goals.Add(gloves);
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
    private TrolleyCollision tourniquetCollide;
    public int val = 10;

    public GatherMaterials(GameObject _tourniquet)
    {

        tourniquet = _tourniquet;
        tourniquetCollide = tourniquet.transform.GetChild(0).GetComponent<TrolleyCollision>();
    }

    public override bool IsAchieved()
    {
        return (tourniquetCollide.collision_trolley == 1);
    }

    public override void Complete()
    {

    }

    public override void DrawHUD()
    {

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