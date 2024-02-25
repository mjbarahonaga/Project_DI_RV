using DG.Tweening;
using MEC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager_Horde : MonoBehaviour
{
    public static Action OnFinishGame;
    static GameManager_Horde _instance;
    public static GameManager_Horde Instance => _instance;

    public int EnemyCount;
    public EnemyBehaviour RefEnemyPrefab;
    public List<Transform> Spawns;
    public Transform Target;
    private IObjectPool<EnemyBehaviour> _enemyPool;
    [Space(10)]
    public int GameLives = 10;
    [Header("Horde Data")]
    public int InitNumberOfEnemies = 6;
    public int IncreasePerLevel = 6;
    public float DelayBetweenSpawnMin = 0.3f;
    public float DelayBetweenSpawnMax = 1f;



    #region DataGame
    [Header("Data current game")]
    [SerializeField] private int _currentLives;
    [SerializeField] private int _currentScore = 0;
    [SerializeField] private int _currentLevel = 0;
    [SerializeField] private int _currentEnemiesAlive;
    [SerializeField] private int _enemiesKilled = 0;
    #endregion
    [Space(10)]
    [Header("Objects in scene")]
    public GameObject PushButton;
    public TextMeshProUGUI ScoreText;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        EnemyBehaviour.OnDie += KilledEnemy;
        EnemyBehaviour.OnReturnToPool += ReturnToPool;
    }

    private void OnDestroy()
    {
        EnemyBehaviour.OnDie -= KilledEnemy;
        EnemyBehaviour.OnReturnToPool -= ReturnToPool;
    }

    private void Init()
    {
        _enemyPool = new ObjectPool<EnemyBehaviour>(
            createFunc: () =>
            {
                var obj = Instantiate(RefEnemyPrefab, transform);
                obj.gameObject.SetActive(false);
                return obj;
            },
            actionOnGet: (enemy) => 
                enemy.Activated(Spawns[UnityEngine.Random.Range(0,Spawns.Count)], Target),
            actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
            collectionCheck: false,
            defaultCapacity: EnemyCount,
            maxSize: EnemyCount + EnemyCount
            );

        // Instantiate them all at once at the beginning
        List<EnemyBehaviour> temp = new List<EnemyBehaviour>();
        for (int i = 0; i < EnemyCount; ++i)
        {
            temp.Add(_enemyPool.Get());
        }

        for (int i = 0; i < EnemyCount; ++i)
        {
            _enemyPool.Release(temp[i]);
        }
    }

    public void KilledEnemy(EnemyBehaviour enemy)
    {
        _currentScore += enemy.Data.Reward;
        ++_enemiesKilled;
        ReturnToPool(enemy);

        CheckNextHorde();
    }

    private void CheckNextHorde()
    {
        if (_currentEnemiesAlive == 0) _ = NextHorde();
    }

    public void ReturnToPool(EnemyBehaviour enemy)
    {
        _enemyPool.Release(enemy);
        --_currentEnemiesAlive;
    } 
    
    public void StartGame()
    {
        Timing.RunCoroutine(StartGameCoroutine(), Segment.SlowUpdate);
    }

    public IEnumerator<float> StartGameCoroutine()
    {
        _currentLevel = 0;
        _enemiesKilled = 0;
        _currentLives = GameLives;
        _ = NextHorde();
        PushButton.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.OutBounce);
        yield return Timing.WaitForSeconds(1f);
        PushButton.SetActive(false);
    }

    public void ReduceLives()
    {
        --_currentLives;
        if( _currentLives == 0 )
        {
            EndGame();
        }
        else
        {
            CheckNextHorde();
        }
    }

    public void EndGame()
    {
        PushButton.SetActive(true);
        PushButton.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InBounce);
        OnFinishGame?.Invoke();
        // [Show data]
        // Show score
        // Show hordes
        // Show how many enemies killed


    }

    public async Task NextHorde()
    {
        int amountEnemies = InitNumberOfEnemies + _currentLevel * IncreasePerLevel;
        _currentEnemiesAlive = amountEnemies;
        int min = (int)DelayBetweenSpawnMin * 1000;
        int max = (int)DelayBetweenSpawnMax * 1000;
        int delay = 0;
        for (int i = 0; i < amountEnemies; ++i)
        {
            _enemyPool.Get();
            delay = UnityEngine.Random.Range(min, max);
            await Task.Delay(delay);
        }

        ++_currentLevel;
    }

#if UNITY_EDITOR
    [Header("DEBUG")]
    public bool BStartGame = false;
    public bool BKillEveryOne = false;
    public bool StartedGame = false;
    private void OnValidate()
    {
        if (BStartGame && !StartedGame)
        {
            BStartGame = false;
            StartedGame = true;
            StartGame();
        }

        if (BKillEveryOne)
        {
            BKillEveryOne = false;
            var enemies = FindObjectsOfType<EnemyBehaviour>();
            int length = enemies.Length;
            for (int i = 0; i < length; ++i)
            {
                MEC.Timing.RunCoroutine(enemies[i].Killed(), MEC.Segment.SlowUpdate);
            }
        }
    }
#endif
}
