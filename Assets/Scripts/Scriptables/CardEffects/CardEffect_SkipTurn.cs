using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skip Turn", menuName = "Card effects/Skip Turn")]
public class CardEffect_SkipTurn : CardEffect_SO
{
    [SerializeField] private bool targetPlayer;
    public Effect_SO effect = null;

    public override bool DoEffect(Entity sender, Entity target, int[] values, GameObject fx)
    {
        if (values.Length != 1 || sender == null)
            return false;

        if (fx != null)
            Instantiate(fx, sender.transform.position, Quaternion.identity);

        if (targetPlayer) TurnManager.Instance.playerTurnsToSkip++;
        else TurnManager.Instance.enemyTurnsToSkip += values[0];

        return true;
    }
}
