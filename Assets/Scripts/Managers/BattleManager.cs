using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Player")]
    [SerializeField]
    private Entity _player = null;

    [Header("Enemy")]
    [SerializeField]
    private Enemy _enemy = null;

    [SerializeField] private List<Gym_SO> gymsList = new List<Gym_SO>();
    private Queue<Gym_SO> gyms = null;
    private Gym_SO currentGym = null;

    private int currentGymRound = 0;
    private int maxGymRound = 0;

    private bool isInGym = false;

    [SerializeField]
    private Enemy_SO[] _enemyData = null;

    [SerializeField]
    private Enemy _enemyPrefab = null;

    [SerializeField]
    private Transform _enemyParent = null;

    [Header("UI")]
    [SerializeField]
    private EntityUI _playerUI = null;

    [SerializeField]
    private EntityUI _enemyUI = null;

    [field: SerializeField]
    public GameObject gameOverPanel { get; private set; }

    [field: SerializeField]
    public GameObject quitPanel { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _enemiesCountUI = null;

    [SerializeField]
    private Button _startGymButton = null;

    private void Start()
    {
        _player.InitEntity(20, _playerUI);

        SpawnEnemy();

        gyms = new Queue<Gym_SO>(gymsList);
            
        _player.onDeath += GameOver;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) quitPanel.SetActive(!quitPanel.activeSelf);
    }

    private void GameOver() => gameOverPanel.SetActive(true);

    /// <summary>
    /// Spawns a random enemy
    /// </summary>
    public void SpawnEnemy()
    {
        if (currentGym?.enemies.Count == 0) EndGym();

        if (isInGym)
        {
            Enemy_SO data = currentGym.enemies.Dequeue();

            _enemy = Instantiate(_enemyPrefab, _enemyParent);
            _enemy.InitEnemy(data, _enemyUI);
            _enemy.onDeath += OnEnemyDeath;

            currentGymRound++;

            string text = $"{currentGymRound} / {maxGymRound}";
            _enemiesCountUI.text = text;
        }
        else
        {
            Enemy_SO data = _enemyData[Random.Range(0, _enemyData.Length)];

            _enemy = Instantiate(_enemyPrefab, _enemyParent);
            _enemy.InitEnemy(data, _enemyUI);
            _enemy.onDeath += OnEnemyDeath;
        }
    }

    public void StartNextGym()
    {
        _enemy.DealDamage(_enemy.MaxHealth * 2);
        currentGym = gyms.Dequeue();

        if (currentGym == null) return;

        _player.ResetStats();

        currentGym.Init();

        maxGymRound = currentGym.enemies.Count;

        isInGym = true;

        _enemiesCountUI.gameObject.SetActive(true);
        _startGymButton.gameObject.SetActive(false);

        if (gyms.Count == 0) gyms = new Queue<Gym_SO>(gymsList);
        TurnManager.Instance.GoToNextState();
    }

    private void EndGym()
    {
        isInGym = false;
        _enemiesCountUI.gameObject.SetActive(false);
        _startGymButton.gameObject.SetActive(true);
        currentGymRound = 0;
        maxGymRound = 0;
    }

    private void OnEnemyDeath()
    {
        _enemy = null;
    }

    public Entity Player => _player;
    public Enemy Enemy => _enemy;
}