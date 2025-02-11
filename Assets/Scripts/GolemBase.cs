using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class GolemBase : NpcBase
{
    private UnityEngine.AI.NavMeshAgent agent; // NavMeshAgent reference

    // Enum for Golem types
    public enum GolemType
    {
        Wood,
        Rock,
        Clay
    }

    // Serializable fields
    [SerializeField] private GolemType type;
    [SerializeField] private int heldResourceMax;
    [SerializeField] public int heldResourceCurrent;
    [SerializeField] private Vector3 currentDestination;



    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // Get NavMeshAgent
        agent.speed = dexterity * 0.3f; // Adjust speed based on dexterity
        agent.stoppingDistance = 0.5f; // Stop before reaching exact position
        DecideGatherDeposit();
    }
    private void Update()
    {
        if (currentDestination != Vector3.zero)
        {
            Move();
        }
    }
    public void DecideGatherDeposit()
    {
        if (heldResourceMax <= heldResourceCurrent)
        {
            SetDestinationDeposit();
        }
        if (heldResourceCurrent < heldResourceMax)
        {
            SetDestinationGather();
        }
    }

    public void SetDestinationDeposit()
    {
        Building[] allObjects = FindObjectsByType<Building>(FindObjectsSortMode.None); // Get all active GameObjects
        switch (currentJob)
        {
            case Jobs.None:
                currentDestination = Vector3.zero;
                break;
            case Jobs.GatherWood:
                foreach (Building obj in allObjects)
                {
                    if (obj.tag == "WoodStorage") 
                    {
                        currentDestination =  obj.transform.position;
                        Debug.Log(obj.transform.position);
                        break;
                    }
                }
                break;
            case Jobs.GatherRock:
                foreach (Building obj in allObjects)
                {
                    if (obj.tag == "RockStorage") 
                    {
                        currentDestination =  obj.transform.position;
                        Debug.Log(obj.transform.position);
                        break;
                    }
                }
                break;
            case Jobs.GatherClay:
                foreach (Building obj in allObjects)
                {
                    if (obj.tag == "ClayStorage") 
                    {
                        currentDestination =  obj.transform.position;
                        Debug.Log(obj.transform.position);
                        break;
                    }
                }
                break;
        }
        

    }
    public void Deposit()
    {
        heldResourceCurrent = 0;
        SetDestinationGather();
    }
    public void SetDestinationGather()
{
    ResourceNode[] allObjects = FindObjectsByType<ResourceNode>(FindObjectsSortMode.None); // Get all active GameObjects
    bool allDepleted = true; // Assume all are depleted, prove otherwise

    switch (currentJob)
    {
        case Jobs.None:
            currentDestination = Vector3.zero;
            break;

        case Jobs.GatherWood:
            foreach (ResourceNode obj in allObjects)
            {
                if (obj.tag == "Wood" && !obj.depleted)
                {
                    currentDestination = obj.transform.position;
                    Debug.Log(obj.transform.position);
                    allDepleted = false; // At least one resource is available
                    break;
                }
            }
            if (allDepleted) StartCoroutine(AvailibleResourceCheck());
            break;

        case Jobs.GatherRock:
            foreach (ResourceNode obj in allObjects)
            {
                if (obj.tag == "Rock" && !obj.depleted)
                {
                    currentDestination = obj.transform.position;
                    Debug.Log(obj.transform.position);
                    allDepleted = false;
                    break;
                }
            }
            if (allDepleted) StartCoroutine(AvailibleResourceCheck());
            break;

        case Jobs.GatherClay:
            foreach (ResourceNode obj in allObjects)
            {
                if (obj.tag == "Clay" && !obj.depleted)
                {
                    currentDestination = obj.transform.position;
                    Debug.Log(obj.transform.position);
                    allDepleted = false;
                    break;
                }
            }
            if (allDepleted) StartCoroutine(AvailibleResourceCheck());
            break;
    }
}

    // Property for Golem type
    public GolemType Type => type;

    // Override methods from NpcBase
    protected override void Move()
    {
        if (currentDestination != Vector3.zero)
        {
            agent.SetDestination(currentDestination); // Move using NavMesh
        }
    }


    protected override void Attack()
    {
        Debug.Log($"{npcName} (Golem) is attacking.");
    }

    protected override void Die()
    {
        Debug.Log($"{npcName} (Golem) is dying.");
    }

    public override void Harvest(int amount)
    {
        heldResourceCurrent += amount;
        DecideGatherDeposit();
    }

    public override void AssignJob(Jobs job)
    {
        base.AssignJob(job); // Call the base method to assign the job
        PerformJob(); // Perform the job immediately after assigning it
    }

    private void PerformJob()
    {
        switch (currentJob)
        {
            case Jobs.GatherWood:
                Debug.Log($"{npcName} (Golem) is gathering wood.");
                SetDestinationGather();
                break;
            case Jobs.GatherRock:
                Debug.Log($"{npcName} (Golem) is gathering rock.");
                SetDestinationGather();
                break;
            case Jobs.GatherClay:
                Debug.Log($"{npcName} (Golem) is gathering clay.");
                SetDestinationGather();
                break;
            default:
                Debug.Log($"{npcName} (Golem) is idle.");
                break;
        }
    }
    private IEnumerator AvailibleResourceCheck()
    {
        yield return new WaitForSeconds(3f);
        DecideGatherDeposit();
    }

}