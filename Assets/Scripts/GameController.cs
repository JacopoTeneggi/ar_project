using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public GameObject instructions;

    public GameObject tourniquet;
    public GameObject gloveBox;
    public GameObject needle;
    public GameObject scanner;
    public GameObject gauze;
    public GameObject tube;

    private List<Goal> goals;
    private int currentGoalIdx;
    private Goal currentGoal;

    void Start()
    {
        // Workflow:
        // 1. gatherMaterials
        // 2. swabArea
        // 3. putGlovesOn
        // 4. applyTourniquet
        // 5. findVein
        // 6. insertNeedle
        // 7. removeNeedle & applyGauze
        GatherMaterials gatherMaterials = new GatherMaterials(instructions, tourniquet, scanner, gauze, needle, tube);
        PutGlovesOn putGlovesOn = new PutGlovesOn(gloveBox);
        ApplyTourniquet applyTourniquet = new ApplyTourniquet(instructions, tourniquet);

        goals = new List<Goal>();
        goals.Add(gatherMaterials);
        goals.Add(putGlovesOn);
        goals.Add(applyTourniquet);

        currentGoalIdx = 2;
        currentGoal = goals[currentGoalIdx];
        currentGoal.Activate();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (currentGoal.IsAchieved())
        {
            currentGoal.Complete();
            //Destroy(currentGoal);
            currentGoalIdx++;
        }
        else
        {
            currentGoal.Continue();
        }
    } 
}

public abstract class  Goal : MonoBehaviour
{
    public abstract void Activate();
    public abstract void Continue();
    public abstract bool IsAchieved();
    public abstract void Complete();
}

public class GatherMaterials : Goal
{
    private int requiredObjects = 7;

    private GameObject instructions;
    private TextMesh instructionsText;

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


    public GatherMaterials(GameObject _instructions, GameObject _tourniquet, GameObject _scanner, GameObject _gauze, GameObject _needle, GameObject _tube)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

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
        tube3Collide = tubes.transform.GetChild(2).GetComponent<TrolleyCollision>();
        needleCollide = needle.transform.GetChild(0).GetComponent<TrolleyCollision>();
    }

    public override void Activate()
    {
        instructionsText.text = "Move materials onto the table";
    }

    public override void Continue()
    {
        int gatheredObjects = tourniquetCollide.trolleyCollision + scannerCollide.trolleyCollision + needleCollide.trolleyCollision + tube1Collide.trolleyCollision + tube2Collide.trolleyCollision + tube3Collide.trolleyCollision + Math.Max(Math.Max(gauze1Collide.trolleyCollision, gauze2Collide.trolleyCollision), gauze3Collide.trolleyCollision);
        instructionsText.text = string.Format("Gathered {0}/{1} objects", gatheredObjects, requiredObjects);
    }

    public override bool IsAchieved()
    {
        return (tourniquetCollide.trolleyCollision == 1 && scannerCollide.trolleyCollision == 1 && (gauze1Collide.trolleyCollision == 1 || gauze2Collide.trolleyCollision == 1 || gauze3Collide.trolleyCollision == 1) && tube1Collide.trolleyCollision == 1 && tube2Collide.trolleyCollision == 1 && tube3Collide.trolleyCollision == 1 && needleCollide.trolleyCollision == 1);
    }

    public override void Complete()
    {
        instructionsText.text = "Completed!";
    }
}

public class ApplyTourniquet: Goal
{
    private GameObject instructions;
    private TextMesh instructionsText;

    private GameObject tourniquet;
    private TourniquetController tourniquetController;
    private GameObject pointOfInterest;

    public ApplyTourniquet(GameObject _instructions, GameObject _tourniquet)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

        tourniquet = _tourniquet;

        tourniquetController = tourniquet.transform.GetChild(0).GetComponent<TourniquetController>();

        pointOfInterest = tourniquet.transform.GetChild(1).gameObject;
        pointOfInterest.SetActive(false);
    }

    public override void Activate()
    {
        instructionsText.text = "Apply the tourniquet to the arm \nby moving it towards \nthe rotating diamond";
        pointOfInterest.SetActive(true);
    }

    public override void Continue()
    {
    }

    public override bool IsAchieved()
    {
        return tourniquetController.isApplied;
    }

    public override void Complete()
    {
        instructionsText.text = "Completed!";
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

    public override void Activate()
    {
        throw new NotImplementedException();
    }

    public override void Continue()
    {
        throw new NotImplementedException();
    }

    public override bool IsAchieved()
    {

        return gloveBoxController.isClicked;
    }

    public override void Complete()
    {
        throw new NotImplementedException();
    }
}