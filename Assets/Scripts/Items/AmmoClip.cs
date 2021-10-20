using System;
using UnityEngine;

[Serializable]
public class AmmoClip
{
    [SerializeField] private int rounds;

    public AmmoClip(int rounds)
    {
        this.rounds = rounds;
    }

    public int Rounds
    {
        get => rounds;
        private set => rounds = value < 0 ? 0 : value;
    }

    public bool RemoveRound()
    {
        if (Rounds == 0) return false;
        Rounds--;
        return true;
    }
}