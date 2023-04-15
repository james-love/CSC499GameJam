using UnityEngine;

public class AbilityPickup : Interactable
{
    [SerializeField] private Ability ability;
    public override void Interact()
    {
        PlayerState.Instance.AddAbility(ability);
    }
}
