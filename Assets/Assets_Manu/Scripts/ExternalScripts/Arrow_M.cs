/// Tutorial from Unity: Creating bow and arrow gameplay in VR!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using MEC;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System;

public class Arrow_M : XRGrabInteractable
{
    public float Speed = 1000f;
    public float MaxTimeInAir = 15f;
    public float TimeToDestroy = 8f;
    public Transform Tip;
    public Collider SphereCollider;
    [SerializeField] protected LayerMask _layersToAvoidOnHit;
    [Header("Particles")]
    public ParticleSystem TrailParticle;
    public ParticleSystem HitParticle;
    public TrailRenderer TrailRenderer;

    [Header("Sound")]
    public AudioClip LaunchClip;
    public AudioClip HitClip;

    protected bool _inAir = false;
    protected Vector3 _lastPosition = Vector3.zero;
    protected CoroutineHandle _updateCoroutine;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected int _changeToIgnoreLayer = -1;
    protected override void Awake()
    {
        base.Awake();
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
        if (_changeToIgnoreLayer == -1)
        {
            _changeToIgnoreLayer = LayerMask.NameToLayer("Ignore");
        }
    }

    protected virtual IEnumerator<float> UpdateCoroutine()
    {
        float timeInAir = 0f;

        while (_inAir)
        {
            CheckCollision();
            _lastPosition = Tip.position;
            timeInAir += Time.deltaTime;
            if (timeInAir > MaxTimeInAir) Stop(hitted: false);
            yield return Timing.WaitForOneFrame;
        }
        yield return 0f;
    }

    protected virtual void CheckCollision()
    {
        if(Physics.Linecast(_lastPosition, Tip.position, out RaycastHit hit))
        {
            if ((_layersToAvoidOnHit.value & (1 << hit.collider.gameObject.layer)) > 0) return;
            if(hit.transform.TryGetComponent(out Rigidbody body) )
            {
                _rb.interpolation = RigidbodyInterpolation.None;
                transform.parent = hit.transform;
                body.AddForce(_rb.velocity, ForceMode.Impulse);
                Stop();
            }

            if(hit.transform.TryGetComponent(out IHittable_M hittable))
            {
                _rb.interpolation = RigidbodyInterpolation.None;
                transform.parent = hit.transform;
                hittable.Hit(this);
                Stop();
            }
            if(hit.transform.TryGetComponent(out BoxCollider collider))
            {
                _rb.interpolation = RigidbodyInterpolation.None;
                transform.parent = hit.transform;
                Stop();
            }
        }
    }

    protected virtual void Stop(bool hitted = true)
    {
        _inAir = false;
        Timing.KillCoroutines(_updateCoroutine);
        SetPhysics(false);

        ArrowParticles(false);
        if(hitted)
            ArrowSounds(HitClip, 1.5f, 2, 0.8f);
        Timing.RunCoroutine(ToDestroy(TimeToDestroy));
    }

    public virtual void Release(float value)
    {
        _inAir = true;
        SetPhysics(true);
        MaskAndFire(value);
        Timing.RunCoroutine(RotateWithVelocity());
        _lastPosition = Tip.position;
        _updateCoroutine = Timing.RunCoroutine(UpdateCoroutine(), Segment.FixedUpdate);


        ArrowParticles(true);
        ArrowSounds(LaunchClip, 4.2f + (0.6f * value), 4.4f + (.6f * value), Mathf.Max(0.7f, value));
        
    }

    protected void SetPhysics(bool usePhysics)
    {
        _rb.useGravity = usePhysics;
        _rb.isKinematic = !usePhysics;
    }

    protected virtual void MaskAndFire(float power)
    {
        
        colliders[0].enabled = false;
        interactionLayers = 1 << LayerMask.NameToLayer("Ignore");
        var value = power * Speed > 0 ? power * Speed : 1f;
        Vector3 force = transform.forward * value;
        _rb.AddForce(force,ForceMode.Impulse);
        transform.parent = null;
    }

    protected IEnumerator<float> RotateWithVelocity()
    {
        
        while (_inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(_rb.velocity, transform.up);
            transform.rotation = newRotation;
            yield return Timing.WaitForOneFrame;
        }
        yield return 0f;
    }

    public void ArrowHaptic(HoverEnterEventArgs args)
    {
        if(args.interactorObject is HandInteractor_M hand)
        {
            if(hand.TryGetComponent(out XRController controller))
                HapticManager_M.Impulse(.7f, 0.5f, controller.inputDevice);
        }
    }

    protected void ArrowParticles(bool release)
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

    protected void ArrowSounds(AudioClip clip, float minPitch, float maxPitch, float volume)
    {
        SoundManager.Instance?.PlaySound(
            transform.position,
            clip,
            volume,
            UnityEngine.Random.Range(minPitch, maxPitch));
    }

    protected virtual IEnumerator<float> ToDestroy(float time)
    {
        yield return Timing.WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    public void ChangeLayer()
    {
        gameObject.layer = _changeToIgnoreLayer;
    }

#if UNITY_EDITOR

    // Pre load data
    protected void OnValidate()
    {
        if(_changeToIgnoreLayer == -1)
        {
            _changeToIgnoreLayer = LayerMask.NameToLayer("Ignore");
        }
        if(_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
#endif
}
