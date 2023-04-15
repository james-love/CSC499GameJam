using TMPro;
using UnityEngine;

public enum AnimationState
{
    Idle,
    Walk,
    Run,
    Melee,
    Range,
    Death,
    JumpStart,
    JumpRise,
    JumpApex,
    JumpFall,
    JumpLand,
}

public class MovementDebug : MonoBehaviour
{
    [SerializeField] private AnimationState state;
    private TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        switch (state)
        {
            case AnimationState.Idle:
                text.text = "Idle";
                break;
            case AnimationState.Walk:
                text.text = "Walk";
                break;
            case AnimationState.Run:
                text.text = "Run";
                break;
            case AnimationState.Melee:
                text.text = "Melee";
                break;
            case AnimationState.Range:
                text.text = "Range";
                break;
            case AnimationState.Death:
                text.text = "Death";
                break;
            case AnimationState.JumpStart:
                text.text = "JumpStart";
                break;
            case AnimationState.JumpRise:
                text.text = "JumpRise";
                break;
            case AnimationState.JumpApex:
                text.text = "JumpApex";
                break;
            case AnimationState.JumpFall:
                text.text = "JumpFall";
                break;
            case AnimationState.JumpLand:
                text.text = "JumpLand";
                break;
            default:
                break;
        }
    }

}
