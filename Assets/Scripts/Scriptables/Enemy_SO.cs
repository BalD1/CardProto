using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Enemy")]
public class Enemy_SO : ScriptableObject
{
    /// <summary>
    /// Spite of the enemy ship
    /// </summary>
    public Sprite sprite = null;

    public Vector2 size = Vector2.one;

    /// <summary>
    /// Health of the enemy
    /// </summary>
    public int maxHealth = 0;

    public Drop_SO enemyDrops = null;

    /// <summary>
    /// How will the enemy will play the actions
    /// </summary>
    public EEnemyActionOrder order = EEnemyActionOrder.Random;

    /// <summary>
    /// List of all the possible actions
    /// </summary>
    public EnemyAction[] actions = null;

    [Serializable]
    public struct EnemyAction
    {
        public Sprite icon;
        public CardEffect_SO effect;
        public int[] values;
        public GameObject fx;
    }
}