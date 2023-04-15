using System;
using System.Collections.Generic;

public enum Ability
{
    MeleeAttack,
    RangeAttack,
    Jump,
}

public enum Flag
{
    RockBroken,
}

[Serializable]
public class SaveState
{
    public int LevelIndex;
    public int SpawnPoint;
    public List<Ability> Abilities;
    public List<Flag> Flags;
}
