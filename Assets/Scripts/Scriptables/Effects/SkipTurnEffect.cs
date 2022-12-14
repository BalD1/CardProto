using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skip Turn", menuName = "Effects/SkipTurn")]
public class SkipTurnEffect : Effect_SO
{
    public GameObject fx = null;

    [SerializeField] private bool targetPlayer;

    override public void Tick(Entity owner, int nb)
    {
        if (targetPlayer) TurnManager.Instance.skipNextPlayerTurn = true;
        else TurnManager.Instance.skipNextEnemyTurn = true;

        owner.RemoveStackFromEffect(this, 1);

        if (fx != null)
            Instantiate(fx, owner.transform.position, Quaternion.identity);

    }
}
