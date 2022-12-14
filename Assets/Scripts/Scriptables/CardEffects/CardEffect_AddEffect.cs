using UnityEngine;

[CreateAssetMenu(fileName = "New add effect", menuName = "Card effects/Add effect")]
public class CardEffect_AddEffect : CardEffect_SO
{
    public Effect_SO effect = null;

    public override bool DoEffect(Entity sender, Entity target, int[] values, GameObject fx)
    {
        if (values.Length != 1 || sender == null || target == null)
            return false;

        target.AddEffect(effect, values[0]); 

        if (fx != null)
            Instantiate(fx, target.transform.position, Quaternion.identity);
        
        return true;
    }
}