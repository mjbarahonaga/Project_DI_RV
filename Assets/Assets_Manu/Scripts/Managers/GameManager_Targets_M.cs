using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using System.Linq;
using DG.Tweening;

public class GameManager_Targets_M : MonoBehaviour
{
    [Header("Game Info")]
    public float GameTime = 180f;
    [SerializeField] private float _activeTargetTime = 4f;
    [SerializeField] private int _currentScore = 0;
    private bool _gameStarted = false;
    private float _timeToFinish = 0f;
    [Header("Time range to call targets")]
    public float Min = .1f;
    public float Max = 2f;

    [Header("Objects in scene")]
    public GameObject PushButton;

    private CoroutineHandle _coroutine;

    [SerializeField] private bool _findTargetsInScene = false;
    [SerializeField] private List<TargetBehaviour_M> _targetsRef = new List<TargetBehaviour_M>();
    private List<TargetBehaviour_M> _targetsDeactivated = new List<TargetBehaviour_M>();
    private List<TargetBehaviour_M> _targetsActivated = new List<TargetBehaviour_M>();

    private void Awake()
    {
        if (_targetsRef.Count == 0) _targetsRef = FindObjectsOfType<TargetBehaviour_M>().ToList();
        _targetsDeactivated = new List<TargetBehaviour_M>(_targetsRef);
    }

    private void Start()
    {
        TargetBehaviour_M.OnScore += AddScore;
        TargetBehaviour_M.OnDeactivatedTarget += DeactivatedTarget;
    }

    private void OnDestroy()
    {
        TargetBehaviour_M.OnScore -= AddScore;
        TargetBehaviour_M.OnDeactivatedTarget -= DeactivatedTarget;
    }

    public void StartGame()
    {
        if(_gameStarted) return;
        _gameStarted = true;
        _currentScore = 0;
        _coroutine = Timing.RunCoroutine(UpdateGameCoroutine());
        PushButton.transform.DOScale(Vector3.zero,1f).SetEase(Ease.OutBounce);
    }

    private IEnumerator<float> UpdateGameCoroutine()
    {
        _timeToFinish = Time.time + GameTime;
        float timeToCallTarget = Random.Range(Min, Max);
        float currentTime = 0f;
        while(_timeToFinish >= Time.time)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeToCallTarget)
            {
                currentTime = 0f;
                timeToCallTarget = Random.Range(Min, Max);

                //call inactive target
                ActivateTarget();
            }
            yield return Timing.WaitForOneFrame;
        }

        FinishGame();

    }

    private void ActivateTarget()
    {
        if (_targetsDeactivated.Count == 0) return;
        int index = Random.Range(0, _targetsDeactivated.Count);

        _targetsDeactivated[index].Activate(_activeTargetTime);
        
        _targetsActivated.Add(_targetsDeactivated[index]);
        _targetsDeactivated.RemoveAt(index);
    }

    private void DeactivatedTarget(TargetBehaviour_M target)
    {
        _targetsDeactivated.Add(target);
        _targetsActivated.Remove(target);
    }

    private void FinishGame()
    {
        Timing.KillCoroutines(_coroutine);
        int length = _targetsActivated.Count;
        for (int i = 0; i < length; ++i)
        {
            _targetsActivated[i].Deactivate(hitted: false);
        }
        _gameStarted = false;
        _targetsActivated.Clear();
        _targetsActivated = new List<TargetBehaviour_M>(_targetsRef);
        PushButton.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InBounce);
        // Show data

    }

    private void AddScore(int score)
    {
        _currentScore += score;
        // Update Canvas
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_findTargetsInScene)
        {
            _findTargetsInScene = false;
            _targetsRef = FindObjectsOfType<TargetBehaviour_M>().ToList();
        }
    }
#endif
}
