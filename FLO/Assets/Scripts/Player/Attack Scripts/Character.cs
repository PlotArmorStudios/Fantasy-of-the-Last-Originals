using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public bool IsJumping;
    public float FallTime { get; set; }
}