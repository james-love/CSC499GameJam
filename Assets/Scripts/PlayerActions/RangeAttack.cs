using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.InputSystem.InputAction;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private AudioClip shoot;
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
            
            SoundManager.Instance.PlaySound(shoot);
            timeSinceLastAttack = 0f;
            playerAnim.SetTrigger("RangeAttack");
            // TODO Generate projectile using playerMovement.IsFacingRight
            StartCoroutine(GenerateBullet(playerMovement.IsFacingRight ? 1f : -1f));
        }
    }

    private IEnumerator GenerateBullet(float direction)
    {
        AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAssetAsync<GameObject>("Bullet");
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
            Instantiate(opHandle.Result, transform.position, Quaternion.identity).GetComponent<Bullet>().SetDirection(direction);
    }
}
