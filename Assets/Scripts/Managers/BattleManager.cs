using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Player")]
    [SerializeField]
    private Entity _player = null;

    [Header("Enemy")]
    [SerializeField]
    private Enemy _enemy = null;

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
        Enemy_SO data = _enemyData[Random.Range(0, _enemyData.Length)];
        _enemy = Instantiate(_enemyPrefab, _enemyParent);
        _enemy.InitEnemy(data, _enemyUI);
        _enemy.onDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath()
    {
        _enemy = null;
    }

    public Entity Player => _player;
    public Enemy Enemy => _enemy;
}