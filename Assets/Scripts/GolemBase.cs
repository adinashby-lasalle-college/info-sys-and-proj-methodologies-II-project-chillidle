using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class GolemBase : NpcBase
{
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
        if (heldResourceMax == heldResourceCurrent)
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
        switch (currentJob)
        {
            case Jobs.None:
                currentDestination = Vector3.zero;
                break;
            case Jobs.GatherWood:
                Building[] allObjects = FindObjectsByType<Building>(FindObjectsSortMode.None); // Get all active GameObjects
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
                break;
            case Jobs.GatherClay:
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
        switch (currentJob)
        {
            case Jobs.None:
                currentDestination = Vector3.zero;
                break;
            case Jobs.GatherWood:
                ResourceNode[] allObjects = FindObjectsByType<ResourceNode>(FindObjectsSortMode.None); // Get all active GameObjects
                foreach (ResourceNode obj in allObjects)
                {
                    if (obj.tag == "Wood" && !obj.depleted)
                    {
                        currentDestination = obj.transform.position;
                        Debug.Log(obj.transform.position);
                        break;
                    }
                }
                break;
            case Jobs.GatherRock:
                break;
            case Jobs.GatherClay:
                break;
        }
    }
    // Property for Golem type
    public GolemType Type => type;

    // Override methods from NpcBase
    protected override void Move()
    {
        if (Vector3.Distance(transform.position, currentDestination) < 0.1f)
        {
            currentDestination = Vector3.zero; // Reset destination
            return;
        }

        Vector3 direction = (currentDestination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        transform.position = Vector3.MoveTowards(transform.position, currentDestination, 3*(dexterity * 0.1f) * Time.deltaTime);
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
                break;
            case Jobs.GatherRock:
                Debug.Log($"{npcName} (Golem) is gathering rock.");
                break;
            case Jobs.GatherClay:
                Debug.Log($"{npcName} (Golem) is gathering clay.");
                break;
            default:
                Debug.Log($"{npcName} (Golem) is idle.");
                break;
        }
    }
    

}