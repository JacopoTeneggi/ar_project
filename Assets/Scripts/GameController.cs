using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject tourniquet;
    public GameObject gloveBox;
    public TextMeshProUGUI instructions;

    private List<Goal> goals;
    private int currentGoalIdx;
    private Goal currentGoal;

    void Start()
    {
        GatherMaterials gatherMaterials = new GatherMaterials(instructions, tourniquet);
        PutGlovesOn putGlovesOn = new PutGlovesOn(instructions, gloveBox);

        goals = new List<Goal>();
        goals.Add(gatherMaterials);
        goals.Add(putGlovesOn);

        currentGoalIdx = 0;
        currentGoal = goals[0];
        currentGoal.Activate();
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

public abstract class Goal : MonoBehaviour
{
    public abstract void Activate();
    public abstract bool IsAchieved();
    public abstract void Complete();
    public abstract void DrawHUD();
}

public class GatherMaterials : Goal
{
    private TextMeshProUGUI instructions;
    private GameObject tourniquet;
    private TrolleyCollision tourniquetCollide;
    public int val = 10;

    public GatherMaterials(TextMeshProUGUI _instructions, GameObject _tourniquet)
    {
        instructions = _instructions;
        tourniquet = _tourniquet;
        tourniquetCollide = tourniquet.transform.GetChild(0).GetComponent<TrolleyCollision>();
    }

    public override void Activate()
    {
        instructions.text = "Next step: Move materials to the trolley";
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

public class PutGlovesOn : Goal
{
    private TextMeshProUGUI instructions;
    private GameObject gloveBox;
    private GloveBoxController gloveBoxController;

    public PutGlovesOn(TextMeshProUGUI _instructions, GameObject _glovebox)
    {
        instructions = _instructions;
        gloveBox = _glovebox;
        gloveBoxController = gloveBox.transform.GetChild(0).GetComponent<GloveBoxController>();
    }

    public override void Activate()
    {
        instructions.text = "Next step: Put gloves on";
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