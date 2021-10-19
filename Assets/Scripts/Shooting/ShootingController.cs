using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : ExtendedMonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private AmmoCartridge[] initialAmmo;

    private List<AmmoCartridge> _ammo;
    private bool _isShooting;

    private void Awake()
    {
        _ammo = initialAmmo.Select(e => (AmmoCartridge) e.Clone()).ToList();
    }

    private void Start()
    {
        DisplayAmmoInfo();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onAddAmmoEventName, OnAddAmmoEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onAddAmmoEventName, OnAddAmmoEvent);
    }

    private void DisplayAmmoInfo()
    {
        var currentBullets = _ammo.FirstOrDefault()?.Rounds ?? 0;
        var bulletsLeft = _ammo.GetRange(1, _ammo.Count == 0 ? 0 : _ammo.Count - 1).Sum(cartridge => cartridge.Rounds);
        EventManager.TriggerEvent(EventData.instance.onDisplayAmmoInfoEventName, currentBullets, bulletsLeft);
    }

    private void OnAddAmmoEvent(object[] args)
    {
        _ammo.Add((AmmoCartridge) args[0]);
        DisplayAmmoInfo();
    }

    public void OnShoot(InputValue inputValue)
    {
        _isShooting = inputValue.isPressed;
        if (!_isShooting) return;
        if (fireRate == 0)
        {
            Shoot();
            return;
        }

        Invoke(Shoot).EverySeconds(1 / fireRate).While(() => _isShooting);
    }

    public void OnReload()
    {
        var cartridge = _ammo.FirstOrDefault();
        if (cartridge == null) return;
        _ammo.Remove(cartridge);
        DisplayAmmoInfo();
        EventManager.TriggerEvent(EventData.instance.onReloadEventName);
    }

    private bool CanShoot()
    {
        var cartridge = _ammo.FirstOrDefault();
        if (cartridge == null) return false;
        if (cartridge.IsEmpty()) return false;
        return cartridge.GetBullet();
    }

    private void Shoot()
    {
        if (!CanShoot())
        {
            EventManager.TriggerEvent(EventData.instance.onFailedToShootEventName);
            return;
        }

        DisplayAmmoInfo();
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        EventManager.TriggerEvent(EventData.instance.onShootEventName);
        if (!Physics.Raycast(ray, out var hit, range, layerMask)) return;
        EventManager.TriggerEvent(EventData.instance.onShootHitEventName, hit);
    }
}