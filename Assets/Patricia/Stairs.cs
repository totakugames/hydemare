using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField]
    public Climb objectType;

    [SerializeField]
    public Vector3 stairTargetPosition;    

    [SerializeField]
    public GameObject ladderTargetObject;
}

public enum Climb 
{
    Stairs,
    Ladder
}

