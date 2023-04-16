using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float attackRange = 1.25f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int attackValue = 1;
    [SerializeField] private AudioClip attack;
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
            
            SoundManager.Instance.PlaySound(attack);
            timeSinceLastAttack = 0f;
            playerAnim.SetTrigger("MeleeAttack");
            Collider2D hit = Physics2D.OverlapBox(new Vector2(transform.position.x + ((attackRange / 2f) * (playerMovement.IsFacingRight ? 1f : -1f)), transform.position.y), new Vector2(attackRange, 2f), 0, enemyMask);
            if (hit)
            {
                Enemy enemy = hit.gameObject.GetComponent<Enemy>();
                enemy.EnemyHit(attackValue);
            }
        }
    }
}
