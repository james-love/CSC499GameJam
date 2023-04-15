using UnityEngine;

public class AbilityPickup : Interactable
{
    [SerializeField] private Ability ability;
    public override void Interact()
    {
        Player.Instance.State.AddAbility(ability);
    }
}
