using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GolemBase woodGolem;
    // Start is called before the first frame update
    void Start()
    {
        if (woodGolem != null)
        {
            // Assign a job to the Golem
            woodGolem.AssignJob(Jobs.GatherWood);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
