using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : ExtendedMonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform fireSource;

    private Gun _gun;
    private bool _isShooting, _isReloading;

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
        EventManager.StartListening(EventData.Instance.onGivePlayerAmmo, OnAddAmmo);
        EventManager.StartListening(EventData.Instance.onReloadFinish, OnReloadFinish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onGivePlayerAmmo, OnAddAmmo);
        EventManager.StopListening(EventData.Instance.onReloadFinish, OnReloadFinish);
    }

    private void OnReloadFinish(Dictionary<string, object> obj)
    {
        _isReloading = false;
    }

    private void OnAddAmmo(Dictionary<string, object> message)
    {
        var ammoClip = (AmmoClip) message["ammoClip"];
        _gun.AddAmmoClip(ammoClip);
        NotifyAmmoUpdate();
    }

    public void OnReload()
    {
        if (_isReloading) return;
        if (_isShooting) return;
        if (!_gun.Reload()) return;
        _isReloading = true;
        _isShooting = false;
        NotifyAmmoUpdate();
        NotifyGunReloaded();
    }

    public void OnShoot(InputValue inputValue)
    {
        if (_isReloading) return;
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

        EventManager.TriggerEvent(EventData.Instance.onUpdateAmmo, new Dictionary<string, object>
        {
            {"magazineRounds", magazineRounds},
            {"clipsRounds", clipsRounds}
        });
    }

    private void NotifyGunReloaded()
    {
        EventManager.TriggerEvent(EventData.Instance.onReload, null);
    }

    private void NotifyGunShot()
    {
        EventManager.TriggerEvent(EventData.Instance.onShoot, new Dictionary<string, object>()
        {
            {"source", fireSource}
        });
    }

    private void NotifyGunFailedToShoot()
    {
        EventManager.TriggerEvent(EventData.Instance.onFailedToShoot, null);
    }

    private void NotifyGunShotHit(RaycastHit hit)
    {
        EventManager.TriggerEvent(EventData.Instance.onShootHit, new Dictionary<string, object>
        {
            {"hit", hit}
        });
    }
}