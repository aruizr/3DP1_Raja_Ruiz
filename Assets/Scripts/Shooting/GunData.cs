using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Scriptable Objects/Gun Data")]
public class GunData : ScriptableObject
{
    public float damage;
    public float fireRate;
    public float shootingDistance;
    public AmmoClip[] initialAmmoClips;
}
