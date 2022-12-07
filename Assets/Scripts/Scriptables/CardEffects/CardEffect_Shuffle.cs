using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shuffle", menuName = "Card effects/Shuffle")]
public class CardEffect_Shuffle : CardEffect_SO
{
    public override bool DoEffect(Entity sender, Entity target, int[] values, GameObject fx)
    {
        if (values.Length != 1 || sender == null || fx == null)
            return false;

        Instantiate(fx, sender.transform.position, Quaternion.identity);

        CardManager.Instance.ShuffleAllCards(); 
        return true;
    }
}
