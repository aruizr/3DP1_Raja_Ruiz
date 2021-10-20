using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : ExtendedMonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private LayerMask layerMask;

    private Gun _gun;
    private bool _isShooting;

    private void Awake()
    {
        _gun = new Gun(gunData);
    }

    private void Start()
    {
        NotifyAmmoUpdate();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onGivePlayerAmmo, OnAddAmmo);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onGivePlayerAmmo, OnAddAmmo);
    }

    private void OnAddAmmo(Dictionary<string, object> message)
    {
        var ammoClip = (AmmoClip) message["ammoClip"];
        _gun.AddAmmoClip(ammoClip);
        NotifyAmmoUpdate();
    }

    public void OnReload()
    {
        if (!_gun.Reload()) return;
        NotifyAmmoUpdate();
        NotifyGunReloaded();
    }

    public void OnShoot(InputValue inputValue)
    {
        _isShooting = inputValue.isPressed;
        if (!_isShooting) return;
        if (_gun.FireRate == 0)
        {
            Shoot();
            return;
        }

        Invoke(Shoot).EverySeconds(1 / _gun.FireRate).While(() => _isShooting);
    }

    private void Shoot()
    {
        if (!_gun.Fire())
        {
            NotifyGunFailedToShoot();
            return;
        }

        NotifyGunShot();
        NotifyAmmoUpdate();
        DoRayCast();
    }

    private void DoRayCast()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (!Physics.Raycast(ray, out var hit, _gun.ShootingDistance, layerMask)) return;

        hit.collider.gameObject.GetComponent<IDamageTaker>()?.TakeDamage(_gun.Damage);

        NotifyGunShotHit(hit);
    }

    private void NotifyAmmoUpdate()
    {
        var magazineRounds = _gun.GetMagazineRounds();
        var clipsRounds = _gun.GetClipsRounds();

        EventManager.TriggerEvent(EventData.instance.onUpdateAmmo, new Dictionary<string, object>
        {
            {"magazineRounds", magazineRounds},
            {"clipsRounds", clipsRounds}
        });
    }

    private void NotifyGunReloaded()
    {
        EventManager.TriggerEvent(EventData.instance.onReload, null);
    }

    private void NotifyGunShot()
    {
        EventManager.TriggerEvent(EventData.instance.onShoot, null);
    }

    private void NotifyGunFailedToShoot()
    {
        EventManager.TriggerEvent(EventData.instance.onFailedToShoot, null);
    }

    private void NotifyGunShotHit(RaycastHit hit)
    {
        EventManager.TriggerEvent(EventData.instance.onShootHit, new Dictionary<string, object>
        {
            {"hit", hit}
        });
    }
}