using UnityEngine;

public class Bird : Enemy
{
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    public override void EnemyHit(float damageValue)
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            // TODO Sound and animation
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }
}
