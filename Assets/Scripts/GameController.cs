using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;

public class GameController : MonoBehaviour, IMixedRealityInputHandler
{
    private bool isStarted;
    private bool isFinished;
    public GameObject instructions;
    private TextMesh instructionsText;

    public GameObject tourniquet;
    public GameObject gloveBox;
    public GameObject needle;
    public GameObject veinScanner;
    public GameObject gauze;
    public GameObject tube;
    public GameObject veins;

    private List<Goal> goals;
    private int currentGoalIdx;
    private Goal currentGoal;
    private bool isCompleting;

    public void OnInputDown(InputEventData eventData)
    {
        isStarted = true;
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);

        currentGoalIdx = 1;
        currentGoal = goals[currentGoalIdx];
        isCompleting = false;
        currentGoal.Activate();
    }

    public void OnInputUp(InputEventData eventData)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        instructionsText = instructions.GetComponent<TextMesh>();
        instructionsText.text = "Welcome to the VR phlebotomy\nsimulator!\nWhen you are ready, press any\nkey to start training";

        isStarted = false;
        isFinished = false;
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);

        // Workflow:
        // 1. gatherMaterials
        // 2. swabArea
        // 3. putGlovesOn
        // 4. applyTourniquet
        // 5. findVein
        // 6. insertNeedle
        // 7. drawBlood
        // 8. removeNeedle & applyGauze
        // 9. complete
        GatherMaterials gatherMaterials = new GatherMaterials(instructions, tourniquet, veinScanner, gauze, needle, tube);
        PutGlovesOn putGlovesOn = new PutGlovesOn(instructions, gloveBox);
        ApplyTourniquet applyTourniquet = new ApplyTourniquet(instructions, tourniquet);
        FindVein findVein = new FindVein(instructions, veinScanner);
        InsertNeedle insertNeedle = new InsertNeedle(instructions, needle, veins);
        DrawBlood drawBlood = new DrawBlood(instructions, needle, tube);
        ExtractNeedle extractNeedle = new ExtractNeedle(instructions, needle, gauze);
	SwabArea swabArea = new SwabArea(instructions, gauze);

        goals = new List<Goal>();
        goals.Add(gatherMaterials);
	goals.Add(swabArea);
        goals.Add(putGlovesOn);
        goals.Add(applyTourniquet);
        goals.Add(findVein);
        goals.Add(insertNeedle);
        goals.Add(drawBlood);
        goals.Add(extractNeedle);
    }

    IEnumerator CompleteGoal()
    {
        currentGoal.Complete();
        instructionsText.text = "Completed!";
        yield return new WaitForSecondsRealtime(1);
        currentGoalIdx++;
        if (currentGoalIdx + 1 < goals.Count)
        {
            currentGoal = goals[currentGoalIdx];
            isCompleting = false;
            currentGoal.Activate();
        }
        else
        {
            isFinished = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            if (currentGoal.IsAchieved())
            {
                if (!isCompleting)
                {
                    isCompleting = true;
                    StartCoroutine(CompleteGoal());
                }
            }
            else
            {
                currentGoal.Continue();
            }
        }
        if (isFinished)
        {
            instructionsText.text = "Congratulations!\nYou have completed the\ntraining simulation";
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

    }
}

public class PutGlovesOn : Goal
{
    private GameObject instructions;
    private TextMesh instructionsText;

    private GameObject gloveBox;
    private GloveBoxController gloveBoxController;

    public PutGlovesOn(GameObject _instructions, GameObject _glovebox)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

        gloveBox = _glovebox;
        gloveBoxController = gloveBox.transform.GetChild(0).GetComponent<GloveBoxController>();
    }

    public override void Activate()
    {
        instructionsText.text = "Put gloves on by clicking \non the glove box";
    }

    public override void Continue()
    {
        
    }

    public override bool IsAchieved()
    {

        return gloveBoxController.isClicked;
    }

    public override void Complete()
    {
        
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
        instructionsText.text = "Apply the tourniquet to the arm \nby moving it towards \nthe rotating diamond";
        pointOfInterest.SetActive(true);
    }

    public override bool IsAchieved()
    {
        return tourniquetController.isApplied;
    }

    public override void Complete()
    {
        pointOfInterest.SetActive(false);
    }

}

public class FindVein: Goal
{
    private GameObject instructions;
    private TextMesh instructionsText;

    private GameObject veinScanner;
    private VeinScannerController veinScannerController;
    private GameObject pointOfInterest;
    private float collisionTimeLimit = 3f;

    public FindVein(GameObject _instructions, GameObject _veinScanner)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

        veinScanner = _veinScanner;

        veinScannerController = veinScanner.transform.GetChild(0).GetComponent<VeinScannerController>();

        pointOfInterest = veinScanner.transform.GetChild(1).gameObject;
        pointOfInterest.SetActive(false);
    }

    public override void Activate()
    {
        instructionsText.text = String.Format("Grab the vein scanner and\nfind the correct vein.\nKeep the vein in the screen\nfor at least {0} second\nto complete the task", collisionTimeLimit);
        pointOfInterest.SetActive(true);
    }

    public override void Continue()
    {
        if (veinScannerController.screenController.collisionWithVein)
        {
            instructionsText.text = string.Format("Time remaining: {0:F2}", collisionTimeLimit - veinScannerController.collisionTime);
        }
        else
        {
            instructionsText.text = String.Format("Grab the vein scanner and\nfind the correct vein.\nKeep the vein in the screen\nfor at least {0} second\nto complete the task", collisionTimeLimit);
        }
    }

    public override bool IsAchieved()
    {
        return veinScannerController.collisionTime >= collisionTimeLimit;
    }

    public override void Complete()
    {
        pointOfInterest.SetActive(false);
    }
}


public class SwabArea: Goal
{
    private GameObject instructions;
    private TextMesh instructionsText;

    private GameObject gauze;
    private GameObject pointOfInterest;
    private int numberOfGauzeApplied;

    public SwabArea(GameObject _instructions, GameObject _gauze)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

        gauze = _gauze;
        pointOfInterest = gauze.transform.GetChild(4).gameObject;
        numberOfGauzeApplied = 0;
        pointOfInterest.SetActive(true);
    }

    public override void Activate()
    {
        instructionsText.text = "Extract the needle from the\nvein and apply gauze";
        instructionsText.text = "Swab the insertion area with\nalcohol using gauze";
    }

    public override void Continue()
    {
        int gauze1Alc = gauze.transform.GetChild(0).GetComponent<GauzeController>().alcohol ? 1 : 0;
        int gauze2Alc = gauze.transform.GetChild(1).GetComponent<GauzeController>().alcohol ? 1 : 0;
        int gauze3Alc = gauze.transform.GetChild(2).GetComponent<GauzeController>().alcohol ? 1 : 0;
        int gauze1Swab = gauze.transform.GetChild(0).GetComponent<GauzeController>().isSwabbed ? 1 : 0;
        int gauze2Swab = gauze.transform.GetChild(1).GetComponent<GauzeController>().isSwabbed ? 1 : 0;
        int gauze3Swab = gauze.transform.GetChild(2).GetComponent<GauzeController>().isSwabbed ? 1 : 0;
        if ((gauze1Alc + gauze1Swab) == 2 || (gauze2Alc + gauze2Swab) == 2 || (gauze3Alc + gauze3Swab) == 2 )
	{
	    numberOfGauzeApplied = 1;
	}
 
    }
    public override bool IsAchieved()
    {
        return (numberOfGauzeApplied == 1);
    }

    public override void Complete()
    {
	    pointOfInterest.SetActive(false);
    }
}


public class InsertNeedle: Goal
{
    private GameObject instructions;
    private TextMesh instructionsText;

    private GameObject needle;
    private NeedleController needleController;
    private GameObject targetVein;
    private GameObject targetPath;
    private bool lastNeedleClicked = false;
    private bool isNeedleReleased = false;

    private float tryAgainMessageTime;

    public InsertNeedle(GameObject _instructions, GameObject _needle, GameObject _veins)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

        needle = _needle;
        needleController = needle.transform.GetChild(0).GetComponent<NeedleController>();

        targetVein = _veins.transform.GetChild(0).gameObject;
        targetPath = _veins.transform.GetChild(1).gameObject;
        targetVein.SetActive(false);
        targetPath.SetActive(false);
    }

    public override void Activate()
    {
        instructionsText.text = "Now insert the needle in the vein.\nClick on the needle to grab it and\nfollow the guide to place it\ninto the vein.\nRelease the button when done.";
        targetVein.SetActive(true);
        targetPath.SetActive(true);
        tryAgainMessageTime = 0f;
    }

    public override void Continue()
    {
        if (!isNeedleReleased && !needleController.isClicked && lastNeedleClicked)
        {
            isNeedleReleased = true;
        }
        lastNeedleClicked = needleController.isClicked;

        if (isNeedleReleased)
        {
            if (!needleController.needlePointController.isInVein)
            {
                needle.transform.GetChild(0).transform.localPosition = new Vector3(1364f, 162f, -1468);
                needle.transform.GetChild(0).transform.localEulerAngles = new Vector3(0f, -90f, 0f);

                if (tryAgainMessageTime < 1f)
                {
                    instructionsText.text = "Attempt failed. Try again";
                    tryAgainMessageTime += Time.deltaTime;
                }
                else
                {
                    tryAgainMessageTime = 0f;
                    isNeedleReleased = false;
                }
            }
        }
        else if (needleController.isClicked)
        {
            if (needleController.needlePointController.isInArm)
            {
                float meanDistanceToVein = needleController.needlePointController.distanceToVein / needleController.needlePointController.framesInArm;
                float meanAngleToPath = needleController.needlePointController.angleToPath / needleController.needlePointController.framesInArm;
                instructionsText.text = String.Format("Needle in vein: {0}\nDistance to vein: {1:F4}\nAngle to path: {2:F4}", needleController.needlePointController.isInVein ? "yes" : "no", meanDistanceToVein, meanAngleToPath);
            }
            else
            {
                instructionsText.text = "Now insert the needle in the vein.\nClick on the needle to grab it and\nfollow the guide to place it\ninto the vein.\nRelease the button when done.";
            }
        }
        else
        {
            instructionsText.text = "Now insert the needle in the vein.\nClick on the needle to grab it and\nfollow the guide to place it\ninto the vein.\nRelease the button when done.";
        }
    }

    public override bool IsAchieved()
    {
        return isNeedleReleased && needleController.needlePointController.isInVein;
    }

    public override void Complete()
    {
        targetPath.SetActive(false);
    }
}

public class DrawBlood : Goal
{
    private GameObject instructions;
    private TextMesh instructionsText;

    private GameObject needle;
    private NeedleController needleController;

    private GameObject tube;
    private int numberOfFilledTubes;

    public DrawBlood(GameObject _instructions, GameObject _needle, GameObject _tube)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

        needle = _needle;
        needleController = needle.transform.GetChild(0).GetComponent<NeedleController>();

        tube = _tube;
        numberOfFilledTubes = 0;
    }

    public override void Activate()
    {
        instructionsText.text = String.Format("Fill 3 tubes with blood\nFilled tubes: {0}", numberOfFilledTubes);
        needleController.tubeAttachment.SetActive(true);
    }

    public override void Continue()
    {
        needleController.tubeAttachment.SetActive(true);

        int tube1Filled = tube.transform.GetChild(0).GetComponent<TestTubeController>().isFilled ? 1 : 0;
        int tube2Filled = tube.transform.GetChild(1).GetComponent<TestTubeController>().isFilled ? 1 : 0;
        int tube3Filled = tube.transform.GetChild(2).GetComponent<TestTubeController>().isFilled ? 1 : 0;
        numberOfFilledTubes = tube1Filled + tube2Filled + tube3Filled;
        instructionsText.text = String.Format("Fill 3 tubes with blood\nFilled tubes: {0}", numberOfFilledTubes);
    }

    public override bool IsAchieved()
    {
        return numberOfFilledTubes >= 3;
    }

    public override void Complete()
    {
        needleController.tubeAttachment.SetActive(false);
    }
}

public class ExtractNeedle : Goal
{
    private GameObject instructions;
    private TextMesh instructionsText;

    private GameObject needle;
    private NeedleController needleController;

    private GameObject gauze;
    private GameObject pointOfInterest;
    private int numberOfGauzeApplied;

    public ExtractNeedle(GameObject _instructions, GameObject _needle, GameObject _gauze)
    {
        instructions = _instructions;
        instructionsText = instructions.GetComponent<TextMesh>();

        needle = _needle;
        needleController = needle.transform.GetChild(0).GetComponent<NeedleController>();
        needleController.toExtract = false;

        gauze = _gauze;
        pointOfInterest = gauze.transform.GetChild(3).gameObject;
        numberOfGauzeApplied = 0;
        pointOfInterest.SetActive(false);
    }

    public override void Activate()
    {
        instructionsText.text = "Extract the needle from the\nvein and apply gauze";
        needleController.toExtract = true;
    }

    public override void Continue()
    {
        if (needleController.toExtract && !needleController.needlePointController.isInVein)
        {
            needleController.toExtract = false;
            pointOfInterest.SetActive(true);
        }
        if (!needleController.toExtract)
        {
            int gauze1Applied = gauze.transform.GetChild(0).GetComponent<GauzeController>().isApplied ? 1 : 0;
            int gauze2Applied = gauze.transform.GetChild(1).GetComponent<GauzeController>().isApplied ? 1 : 0;
            int gauze3Applied = gauze.transform.GetChild(2).GetComponent<GauzeController>().isApplied ? 1 : 0;
            numberOfGauzeApplied = gauze1Applied + gauze2Applied + gauze3Applied;
        }
    }

    public override bool IsAchieved()
    {
        return (numberOfGauzeApplied >= 1);
    }

    public override void Complete()
    {
        pointOfInterest.SetActive(false);
    }
}