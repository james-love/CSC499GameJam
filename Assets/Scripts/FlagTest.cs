using UnityEngine;

public class FlagTest : Interactable
{
    [SerializeField] private Flag flag;
    private SpriteRenderer sprite;
    public override void Interact()
    {
        Player.Instance.State.AddFlag(flag);
        sprite.color = Color.green;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (Player.Instance.State.Current.Flags.Contains(flag))
            sprite.color = Color.green;
    }
}
