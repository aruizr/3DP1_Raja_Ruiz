using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHitCollider : MonoBehaviour, IDamageTaker
{
    private static readonly Dictionary<BodyPart, float> DamageMultiplier = new Dictionary<BodyPart, float>()
    {
        {BodyPart.Body, 0f},
        {BodyPart.Helix, 1f},
        {BodyPart.Head, 2f},
    };

    private enum BodyPart {Helix, Head, Body}

    [SerializeField] private BodyPart bodyPart;
    [SerializeField] private HealthSystem droneHealthSystem;

    public void TakeDamage(float amount)
    {
        droneHealthSystem.TakeDamage(amount * DamageMultiplier[bodyPart]);
    }
}
