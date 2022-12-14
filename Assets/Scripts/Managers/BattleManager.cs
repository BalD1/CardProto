using System.Text;
using TMPro;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Player")]
    [SerializeField]
    private Entity _player = null;

    [Header("Enemy")]
    [SerializeField]
    private Enemy _enemy = null;

    [SerializeField] private int enemiesToBattleBeforeBoss = 5;
    private int currentEnemiesCount = 0;

    [SerializeField]
    private Enemy_SO[] _bossData = null;

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

    [SerializeField]
    private TextMeshProUGUI _enemiesCountUI = null;

    private void Start()
    {
        _player.InitEntity(20, _playerUI);

        SpawnEnemy();
    }

    /// <summary>
    /// Spawns a random enemy
    /// </summary>
    public void SpawnEnemy()
    {
        Enemy_SO data;

        bool bossBattle = currentEnemiesCount >= enemiesToBattleBeforeBoss;
        if (bossBattle)
        {
            data = _bossData[Random.Range(0, _bossData.Length)];
            currentEnemiesCount = 0;
        }
        else data = _enemyData[Random.Range(0, _enemyData.Length)];
        
        _enemy = Instantiate(_enemyPrefab, _enemyParent);
        _enemy.InitEnemy(data, _enemyUI);
        _enemy.onDeath += OnEnemyDeath;

        if (bossBattle)
        {
            _enemiesCountUI.text = "/!\\";
        }
        else
        {
            currentEnemiesCount++;

            string text = $"{currentEnemiesCount} / {enemiesToBattleBeforeBoss}";
            _enemiesCountUI.text = text;
        }
            
    }

    private void OnEnemyDeath()
    {
        _enemy = null;
    }

    public Entity Player => _player;
    public Enemy Enemy => _enemy;
}