using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] public int resourceValue;
    [SerializeField] public bool depleted;
    private void OnCollisionEnter(Collision collision)
    {
        GolemBase golem;
        golem = collision.gameObject.GetComponent<GolemBase>();
        if (golem != null)
        {
            // Safely check if job matches
            if (System.Enum.TryParse("Gather" + gameObject.tag, out Jobs job))
            {
                if (golem.CurrentJob == job && !depleted)
                {
                    GatherResource(golem);
                }
            }
        }
    }
    public void GatherResource(GolemBase golem)
    {
        if (golem == null) return;
        golem.Harvest(resourceValue);

        if (!depleted)
        {
            depleted = true;
            Invoke(nameof(ResetDepleted), 10f);
        }
    }

    private void ResetDepleted()
    {
        depleted = false;
    }

}
