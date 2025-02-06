using UnityEngine;

public enum ResourceType
{
    Wood,
    Clay,
    Stone
}

public class Player : MonoBehaviour
{
    public int Wood { get; private set; }
    public int Clay { get; private set; }
    public int Stone { get; private set; }

    
}
