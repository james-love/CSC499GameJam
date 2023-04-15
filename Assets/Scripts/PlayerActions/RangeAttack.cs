using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1.5f;
    private PlayerMovement playerMovement;
    private float timeSinceLastAttack;
    private Animator playerAnim;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        timeSinceLastAttack = attackCooldown;
        playerAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        timeSinceLastAttack = Mathf.Clamp(timeSinceLastAttack + Time.deltaTime, 0f, attackCooldown);
    }

    public void AttackPressed(CallbackContext context)
    {
        if (context.started && timeSinceLastAttack == attackCooldown)
        {
            // TODO Play Sound here
            timeSinceLastAttack = 0f;
            playerAnim.SetTrigger("RangeAttack");
            // TODO Generate projectile using playerMovement.IsFacingRight
        }
    }
}
