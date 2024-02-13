/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Arrow_M : XRGrabInteractable
{
    public float Speed = 1000f;
    public Transform Tip;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;
    private Rigidbody _rb;
    public Collider SphereCollider;

    [Header("Particles")]
    public ParticleSystem TrailParticle;
    public ParticleSystem HitParticle;
    public TrailRenderer TrailRenderer;

    [Header("Sound")]
    public AudioClip LaunchClip;
    public AudioClip HitClip;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_inAir)
        {
            CheckCollision();
            _lastPosition = Tip.position;
        }
    }

    private void CheckCollision()
    {
        if(Physics.Linecast(_lastPosition, Tip.position, out RaycastHit hit))
        {
            if(hit.transform.TryGetComponent(out Rigidbody body) )
            {
                _rb.interpolation = RigidbodyInterpolation.None;
                transform.parent = hit.transform;
                body.AddForce(_rb.velocity, ForceMode.Impulse);
            }
            Stop();
        }
    }

    private void Stop()
    {
        _inAir = false;
        SetPhysics(false);

        ArrowParticles(false);
        ArrowSounds(HitClip, 1.5f, 2, 0.8f, -2);
    }

    public void Release(float value)
    {
        _inAir = true;
        SetPhysics(true);
        MaskAndFire(value);
        StartCoroutine(RotateWithVelocity());

        _lastPosition = Tip.position;

        ArrowParticles(true);
        ArrowSounds(LaunchClip, 4.2f + (0.6f * value), 4.4f + (.6f * value), Mathf.Max(0.7f, value), -1);
    }

    private void SetPhysics(bool usePhysics)
    {
        _rb.useGravity = usePhysics;
        _rb.isKinematic = !usePhysics;
    }

    private void MaskAndFire(float power)
    {
        colliders[0].enabled = false;
        interactionLayerMask = 1 << LayerMask.NameToLayer("Ignore");
        Vector3 force = transform.forward * power * Speed;
        _rb.AddForce(force,ForceMode.Impulse);
        transform.parent = null;
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(_rb.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }

    public void ArrowHaptic(HoverEnterEventArgs args)
    {
        if(args.interactorObject is HandInteractor_M hand)
        {
            if(hand.TryGetComponent(out XRController controller))
                HapticManager_M.Impulse(.7f, 0.5f, controller.inputDevice);
        }
    }

    private void ArrowParticles(bool release)
    {
        if(release)
        {
            TrailParticle.Play();
            TrailRenderer.emitting = true;
        }
        else
        {
            TrailParticle.Stop();
            HitParticle.Play();
            TrailRenderer.emitting = false;
        }
    }

    private void ArrowSounds(AudioClip clip, float minPitch, float maxPitch, float volume, int id)
    {
        //SFXPlayer.Instance.PlaySFX(clip, transform.position, new SFXPlayer.PlayParameters()
        //{
        //    Pitch = Random.Range(minPitch, maxPitch),
        //    Volume = volume,
        //    SourceID = id
        //});
    }
}
