using System.Collections.Generic;
using System.Linq;

public class Gun
{
    private readonly List<AmmoClip> _clips;
    private AmmoClip _magazine;

    public Gun(GunData gunData)
    {
        Damage = gunData.damage;
        FireRate = gunData.fireRate;
        ShootingDistance = gunData.shootingDistance;
        _clips = gunData.initialAmmoClips.Select(e => new AmmoClip(e.Rounds)).ToList();
        _magazine = GetNextClip() ?? new AmmoClip(0);
    }

    public float Damage { get; }

    public float FireRate { get; }

    public float ShootingDistance { get; }

    public bool Fire()
    {
        return _magazine.RemoveRound();
    }

    private AmmoClip GetNextClip()
    {
        var clip = _clips.FirstOrDefault();
        if (clip == null) return null;
        _clips.Remove(clip);
        return clip;
    }

    public bool Reload()
    {
        var clip = GetNextClip();

        if (clip == null) return false;
        _magazine = clip;
        return true;
    }

    public int GetMagazineRounds()
    {
        return _magazine.Rounds;
    }

    public int GetClipsRounds()
    {
        return _clips.Sum(clip => clip.Rounds);
    }

    public void AddAmmoClip(AmmoClip ammoClip)
    {
        if (ammoClip == null) return;
        _clips.Add(ammoClip);
    }
}