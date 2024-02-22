using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBehaviour : MonoBehaviour, IHittable_M
{
    public static Action<EnemyBehaviour> OnDie;
    public static Action<EnemyBehaviour> OnReturnToPool;
    public EnemyData Data;
    public Animator MyAnimator;


    [SerializeField] private RandomizeEnemyStyle _rndStyle;
    [SerializeField] private ParticleSystem _hitParticle;
    private NavMeshAgent _agent;
    private List<Arrow_M> _arrowsAttached = new List<Arrow_M>();
    private Transform _transform;
    
    private int _currentLives = 0;

    [Header("Sound")]
    public SoundData HitSound;

    private int _idRunningAnim;
    private int _idDyingAnim;

    private void Awake()
    {
        _rndStyle.Randomize();
        _idRunningAnim = Animator.StringToHash("Running");
        _idDyingAnim = Animator.StringToHash("Dying");
        _transform = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        GameManager_Horde.OnFinishGame += Deactivate;
    }

    private void OnDisable()
    {
        GameManager_Horde.OnFinishGame -= Deactivate;
    }

    public void Activated(Transform whereToSpawn, Transform target)
    {
        //MyAnimator.SetTrigger(_idRunningAnim);
        _currentLives = Data.Lives;
        _agent.speed = Data.Speed;
        Debug.Log("He sido Spawneado en: " + whereToSpawn.name + " posicion: " + whereToSpawn.position);
        _transform.position = whereToSpawn.position;
        //if (NavMesh.SamplePosition(whereToSpawn.position, out NavMeshHit closesthit, 500f, NavMesh.AllAreas))
        //    _transform.position = closesthit.position;
        gameObject.SetActive(true);
        _agent.SetDestination(target.position);
        _agent.isStopped = false;
    }

    public void Hit(Arrow_M arrow)
    {

        _arrowsAttached.Add(arrow);
        SoundManager.Instance?.PlaySound(_transform.position, HitSound);
        _hitParticle?.Play();
        --_currentLives;
        if(_currentLives  == 0)
        Timing.RunCoroutine(Killed(), MEC.Segment.SlowUpdate);
    }

    public IEnumerator<float> Killed()
    {
        MyAnimator.SetTrigger(_idDyingAnim);
        _agent.isStopped = true;
        var time = MyAnimator.GetCurrentAnimatorStateInfo(0).length + 1f;
        yield return Timing.WaitForSeconds(time);
        RemoveHits();
        OnDie?.Invoke(this);
    }

    public void RemoveHits()
    {
        int length = _arrowsAttached.Count;
        Arrow_M arrow = null;
        for (int i = 0; i < length; ++i)
        {
            arrow = _arrowsAttached[i];
            _arrowsAttached.RemoveAt(i);
            Destroy(arrow);
            --i;
        }
    }

    public void Deactivate()
    {
        if (gameObject.activeSelf)
        {
            _agent.isStopped = true;
            OnReturnToPool?.Invoke(this);
        }
    }
}
