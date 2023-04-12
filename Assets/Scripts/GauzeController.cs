using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauzeController : MonoBehaviour
{
    public List<Goal> goals = new List<Goal>();
    public GameObject Tourniquet;

    public int currentGoalIdx;


    void Start()
    {
        GatherMaterials goal1 = new GatherMaterials(Tourniquet);
        goals.Add(goal1);
        currentGoalIdx = 0;

    }

    void OnGUI()
    {
        foreach (var goal in goals)
        {
            goal.DrawHUD();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        Goal currentGoal = goals[currentGoalIdx];
        if (currentGoal.IsAchieved())
        {
            currentGoal.Complete();
            Destroy(currentGoal);
            currentGoalIdx++;
        }

        
        
        /*foreach (var goal in goals)
        {
            if (goal.IsAchieved())
            {
                goal.Complete();
                Destroy(goal);
            }
        }*/
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
    public GameObject Tourniquet;
    private TrolleyCollision tourniquetCollide;
    public GatherMaterials(GameObject T)
    {
        Tourniquet = T;
        tourniquetCollide = Tourniquet.transform.GetChild(0).GetComponent<TrolleyCollision>();
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
