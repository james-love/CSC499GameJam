using System;

public enum Ability
{
    MeleeAttack,
    RangeAttack,
    Jump,
}

public enum Flags
{
    RockBroken,
}

[Serializable]
public class SaveState
{
    public int LevelIndex;
    public int SpawnPoint;
    public Ability[] Abilities;
    public Flags[] Flags;
}
