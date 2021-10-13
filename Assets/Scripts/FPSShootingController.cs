using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSShootingController : ExtendedMonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private int rounds;
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform gunFireSource;
    [SerializeField] private RecoilSettings recoilSettings;

    private Transform _camera;
    private Sequence _gunRecoilSequence;
    private bool _isShooting;
    private Vector3 _originalRotation, _originalPosition;
    private float _timer;
    private int _currentRounds;

    private void Awake()
    {
        _camera = Camera.main.transform;
        _originalRotation = gunTransform.localEulerAngles;
        _originalPosition = gunTransform.localPosition;
        _currentRounds = rounds;
        InitGunRecoilSequence();
    }

    private void FixedUpdate()
    {
        if (fireRate == 0 || !_isShooting || _currentRounds <= 0) return;
        _timer += Time.fixedDeltaTime;
        if (!(_timer >= 1 / fireRate)) return;
        Shoot();
        _currentRounds -= 1;
        _timer = 0;
    }

    private void InitGunRecoilSequence()
    {
        _gunRecoilSequence = DOTween.Sequence();
        _gunRecoilSequence.Append(
            DOTween.ToAxis(
                () => gunTransform.localEulerAngles,
                value => gunTransform.localEulerAngles = value,
                _originalRotation.z + recoilSettings.rotation, recoilSettings.startDuration, AxisConstraint.Z));
        _gunRecoilSequence.Join(
            DOTween.ToAxis(
                () => gunTransform.localPosition,
                value => gunTransform.localPosition = value,
                _originalPosition.x + recoilSettings.movement, recoilSettings.startDuration));
        _gunRecoilSequence.Join(_camera.DOShakePosition(0.15f, (Vector3.right + Vector3.up) * 0.075f));
        _gunRecoilSequence.Append(
            DOTween.ToAxis(
                () => gunTransform.localEulerAngles,
                value => gunTransform.localEulerAngles = value,
                _originalRotation.z, recoilSettings.endDuration, AxisConstraint.Z));
        _gunRecoilSequence.Join(
            DOTween.ToAxis(
                () => gunTransform.localPosition,
                value => gunTransform.localPosition = value,
                _originalPosition.x, recoilSettings.endDuration));
        _gunRecoilSequence.SetAutoKill(false);
        _gunRecoilSequence.Pause();
    }

    private void OnShoot(InputValue inputValue)
    {
        if (fireRate == 0 && inputValue.isPressed && _currentRounds > 0)
        {
            _currentRounds -= 1;
            Shoot();
            return;
        }

        _isShooting = inputValue.isPressed;
        if (_isShooting) _timer = 1 / fireRate;
    }

    private void Shoot()
    {
        Instantiate(muzzleFlash, gunFireSource.position, Quaternion.LookRotation(gunFireSource.forward * -1));
        _gunRecoilSequence.Restart();
        if (!Physics.Raycast(_camera.position, _camera.forward, out var hit, range)) return;
        hit.transform.GetComponent<IDamageTaker>()?.TakeDamage(damage);
        var decal = Instantiate(this.bulletDecal, hit.point, Quaternion.LookRotation(hit.normal));
        decal.transform.parent = hit.transform;
        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }

    [Serializable]
    private struct RecoilSettings
    {
        public float rotation, movement, startDuration, endDuration;
    }
}