using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer _shipSprite = null;

    [SerializeField]
    private GameObject _shieldSprite = null;

    [SerializeField]
    private Transform _projectileSpawnPoint = null;

    [SerializeField]
    private GameObject _explosionFx = null;

    public Action onDeath = null;

    protected EntityUI _ui = null;

    protected Dictionary<Effect_SO, int> _beginTurnEffects = null;
    protected Dictionary<Effect_SO, int> _endTurnEffects = null;

    protected int _health = 0;
    protected int _maxHealth = 0;
    protected int _shield = 0;
    protected int _energy = 0;
    protected bool _isDead = false;

    protected virtual void Start()
    {
        _beginTurnEffects= new Dictionary<Effect_SO, int>();
        _endTurnEffects= new Dictionary<Effect_SO, int>();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    /// <summary>
    /// Initialize the entity and it's UI
    /// </summary>
    public virtual void InitEntity(int maxHealth, EntityUI ui)
    {
        _ui = ui;
        _maxHealth = maxHealth;
        _health = maxHealth;
        _shield = 0;
        _energy = 0;

        _ui.ChangeHealth(_health, maxHealth);
        _ui.ChangeShield(_shield);
        _ui.ChangeEnergy(_energy);

        _shieldSprite.SetActive(false);
    }

    #region Health/Shield/Energy
    /// <summary>
    /// Deals damages to the entity
    /// </summary>
    public void DealDamage(int amount)
    {
        if (amount <= 0)
            return;

        _health -= RemoveShield(amount);

        if (_health <= 0)
        {
            _health = 0;
            Die();
        }

        _ui.ChangeHealth(_health, _maxHealth);
    }

    /// <summary>
    /// Heals by the amount, can't go over the max health value
    /// </summary>
    public void Heal(int amount)
    {
        if (amount <= 0)
            return;

        _health += amount;
        _health = Mathf.Min(_health, _maxHealth);
        _ui.ChangeHealth(_health, _maxHealth);
    }

    /// <summary>
    /// Adds shield, no limit
    /// </summary>
    public void AddShield(int amount)
    {
        if (amount <= 0)
            return;

        if (_shield == 0)
            SpawnShield();

        _shield += amount;
        _ui.ChangeShield(_shield);
    }

    /// <summary>
    /// Removes shield, breaks the shield if it goes under 0
    /// Returns the damage amount left if the shield is destroyed
    /// </summary>
    public int RemoveShield(int amount)
    {
        if (amount <= 0 || _shield <= 0)
            return amount;

        int diff = _shield - amount;

        if (diff <= 0)
        {
            _shield = 0;
            BreakShield();
            _ui.ChangeShield(_shield);
            return Mathf.Abs(diff);
        }
        else
        {
            _shield -= amount;
            _ui.ChangeShield(_shield);
            return 0;
        }
    }

    /// <summary>
    /// Adds some energy
    /// </summary>
    public void GiveEnergy(int amount)
    {
        _energy += amount;
    }

    /// <summary>
    /// Try use some energy, returns true if it can be used
    /// </summary>
    public bool UseEnergy(int amount)
    {
        if (_energy < amount)
            return false;

        _energy -= amount;
        _ui.ChangeEnergy(_energy);
        return true;
    }

    /// <summary>
    /// Kills the entity
    /// </summary>
    protected virtual void Die()
    {
        _isDead = true;
        Instantiate(_explosionFx, transform.position, Quaternion.identity);
        _shipSprite.gameObject.SetActive(false);
        _shipSprite.gameObject.SetActive(false);
        _beginTurnEffects.Clear();
        _endTurnEffects.Clear();
        _ui.ClearEffects();
        onDeath?.Invoke();
        StartCoroutine(DieCorout());
    }

    private IEnumerator DieCorout()
    {
        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    /// <summary>
    /// Set the shield to be visible
    /// </summary>
    private void SpawnShield()
    {
        _shieldSprite.SetActive(true);
    }

    /// <summary>
    /// Animate the breaking of the shield
    /// </summary>
    private void BreakShield()
    {
        _shieldSprite.SetActive(false);
    }
    #endregion

    #region Turn
    /// <summary>
    /// Called before the actual start of the turn
    /// </summary>
    public virtual void BeginTurn()
    {
        //Create new dictionary to avoid direct reference
        Dictionary<Effect_SO, int> effects = new Dictionary<Effect_SO, int>();
        foreach (Effect_SO e in _beginTurnEffects.Keys)
        {
            effects.Add(e, _beginTurnEffects[e]);
        }

        foreach (Effect_SO effect in effects.Keys)
        {
            effect?.Tick(this, _beginTurnEffects[effect]);
        }
    }

    /// <summary>
    /// Starts the turn
    /// </summary>
    public virtual void StartTurn()
    {
        _energy += 7;
        
        if (_energy > 15) _energy = 15;
        
        _shield /= 2;
        _ui.ChangeEnergy(_energy);
    }

    /// <summary>
    /// Ends the turn
    /// </summary>
    public virtual void EndTurn()
    {
        _ui.ChangeEnergy(_energy);

        //Create new dictionary to avoid direct reference
        Dictionary<Effect_SO, int> effects = new Dictionary<Effect_SO, int>();
        foreach (Effect_SO e in _endTurnEffects.Keys)
        {
            effects.Add(e, _endTurnEffects[e]);
        }

        foreach (Effect_SO effect in _endTurnEffects.Keys)
        {
            effect.Tick(this, _endTurnEffects[effect]);
        }
    }
    #endregion

    #region Effects
    /// <summary>
    /// Adds an effect, if there is already one of this effect, just adds the stacks
    /// </summary>
    public void AddEffect(Effect_SO effect, int nb)
    {
        Dictionary<Effect_SO, int> effects = effect.order == EEffectOrder.BeginTurn ? _beginTurnEffects: _endTurnEffects;

        if (effects.ContainsKey(effect))
        {
            //If there is already this effect, just add the number to the stack
            effects[effect] += nb;

            _ui.UpdateEffect(effect, effects[effect]);
        }
        else
        {
            //Create a new effect
            effects.Add(effect, nb);

            _ui.AddEffect(effect, nb);
        }
    }

    /// <summary>
    /// Removes the specifed effect if possible
    /// </summary>
    public void RemoveEffect(Effect_SO effect)
    {
        Dictionary<Effect_SO, int> effects = effect.order == EEffectOrder.BeginTurn ? _beginTurnEffects : _endTurnEffects;

        if(effects.ContainsKey(effect))
        {
            effects.Remove(effect);
            _ui.RemoveEffect(effect);
        }
    }

    /// <summary>
    /// Removes some stacks to the effect, removes the effect if necessary
    /// </summary>
    public void RemoveStackFromEffect(Effect_SO effect, int nb)
    {
        Dictionary<Effect_SO, int> effects = effect.order == EEffectOrder.BeginTurn ? _beginTurnEffects : _endTurnEffects;

        if (effects.ContainsKey(effect))
        {
            effects[effect] -= nb;
            _ui.UpdateEffect(effect, effects[effect]);

            if (effects[effect] <= 0)
                RemoveEffect(effect);
        }
    }

    /// <summary>
    /// Returns the number of stacks of the specified effect, 0 if the effect is not present
    /// </summary>
    public int GetStackNumber(Effect_SO effect)
    {
        Dictionary<Effect_SO, int> effects = effect.order == EEffectOrder.BeginTurn ? _beginTurnEffects : _endTurnEffects;

        if(effects.ContainsKey(effect))
            return effects[effect];
        else
            return 0;
    }
    #endregion

    #region Getters & Setters
    public Transform ProjectileSpawnPoint => _projectileSpawnPoint;
    public Transform ShieldSpriteTransform => _shieldSprite.transform;

    public int Health => _health;
    public int MaxHealth => _maxHealth;
    public int Shield => _shield;
    public int Energy => _energy;
    public bool IsDead => _isDead;
    #endregion
}