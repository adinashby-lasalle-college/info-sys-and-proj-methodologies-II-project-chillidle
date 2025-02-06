using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcBase : MonoBehaviour
{

    // Serializable fields
    [SerializeField] protected Jobs currentJob = Jobs.None; // Default job
    [SerializeField] protected string npcName;
    [SerializeField] protected int strength;
    [SerializeField] protected int dexterity;
    [SerializeField] protected int intelligence;
    [SerializeField] private Vector3 position;

    // Properties
    public string NpcName => npcName;
    public Jobs CurrentJob => currentJob;

    // Virtual methods to be overridden by derived classes
    protected virtual void Move()
    {
        Debug.Log($"{npcName} is moving.");
    }

    protected virtual void Attack()
    {
        Debug.Log($"{npcName} is attacking.");
    }

    protected virtual void Die()
    {
        Debug.Log($"{npcName} is dying.");
    }

    public virtual void Harvest(int amount)
    {
        Debug.Log($"{npcName} is harvesting.");
    }

    public virtual void AssignJob(Jobs job)
    {
        currentJob = job;
        Debug.Log($"{npcName} has been assigned the job: {job}");
    }
}