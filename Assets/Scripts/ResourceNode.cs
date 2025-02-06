using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] public int resourceValue;
    [SerializeField] public bool depleted;
    private GolemBase golem;
    private void OnCollisionEnter(Collision collision)
    {
        golem = collision.gameObject.GetComponent<GolemBase>();
        if (golem != null)
        {
            // Safely check if job matches
            if (System.Enum.TryParse("Gather" + gameObject.tag, out Jobs job))
            {
                if (golem.CurrentJob == job)
                {
                    GatherResource();
                }
            }
        }
    }
    public void GatherResource()
    {
        if (golem == null) return;
        golem.Harvest(resourceValue);
        depleted = true;
    }
}
