using UnityEngine;

[DisallowMultipleComponent]
public class EntityBehaviour : MonoBehaviour
{
    public int entityAttackMonoId;
    public int entityBeAttackMonoId = -1;
    public float animationSpeed = 1;
    public AnimationCurve animationCurve;
}
