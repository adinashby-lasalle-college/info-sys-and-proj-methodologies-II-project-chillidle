using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] public string buildingName;
    [SerializeField] public int resourceQuantity;
    [SerializeField] private int maxCapacity;
    [SerializeField] private int level;
    [SerializeField] private Vector3 position;
    private GolemBase golem;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");

        GolemBase golem = collision.gameObject.GetComponent<GolemBase>();
        if (golem != null)
        {
            Debug.Log($"Golem {golem.NpcName} detected with job: {golem.CurrentJob}");

            Jobs expectedJob;

            // Map correct job based on the object's tag
            if (gameObject.tag == "WoodStorage")
            {
                expectedJob = Jobs.GatherWood; // Map to correct job
            }
            else if (gameObject.tag == "RockStorage")
            {
                expectedJob = Jobs.GatherRock;
            }
            else if (gameObject.tag == "ClayStorage")
            {
                expectedJob = Jobs.GatherClay;
            }
            else
            {
                Debug.LogWarning($"Unknown storage tag: {gameObject.tag}");
                return; // Exit if it's an unrecognized tag
            }

            // Now compare the mapped job correctly
            if (golem.CurrentJob == expectedJob)
            {
                Debug.Log("Job matched! Adding resources.");
                AddResource(golem.heldResourceCurrent);
                golem.Deposit();
            }
            else
            {
                Debug.LogWarning($"Job mismatch: Expected {expectedJob}, but Golem is {golem.CurrentJob}");
            }
        }
    }


    public void AddResource(int amount)
    {
        resourceQuantity += amount;
        Debug.Log($"{gameObject.tag} increased by {amount}. Current: {resourceQuantity}");
    }

    public int GetResourceAmount(ResourceType type)
    {
        return resourceQuantity;
    }
}
