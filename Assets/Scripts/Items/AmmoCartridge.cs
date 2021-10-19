using System;
using UnityEngine;

[Serializable]
public class AmmoCartridge : ICloneable
{
    [SerializeField] private int rounds;

    public AmmoCartridge(int rounds)
    {
        this.rounds = rounds;
    }

    public int Rounds => rounds;

    public object Clone()
    {
        return new AmmoCartridge(Rounds);
    }

    public bool GetBullet()
    {
        rounds = Rounds - 1;
        return Rounds >= 0;
    }

    public bool IsEmpty()
    {
        return Rounds <= 0;
    }
}