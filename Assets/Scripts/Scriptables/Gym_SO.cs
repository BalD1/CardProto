using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gym", menuName = "Gym")]
public class Gym_SO : ScriptableObject
{
    [field: SerializeField] public List<Enemy_SO> enemiesList;

    [field: SerializeField] public Queue<Enemy_SO> enemies;

    public void Init() => enemies = new Queue<Enemy_SO>(enemiesList);
}
