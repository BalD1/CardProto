using UnityEngine;

[CreateAssetMenu(fileName = "New deal damage", menuName = "Card effects/Deal damage")]
public class CardEffect_DealDamage : CardEffect_SO
{
    public override bool DoEffect(Entity sender, Entity target, int[] values, GameObject fx)
    {
        if (values.Length != 1 || target == null || fx == null) 
            return false;

        Projectile p = Instantiate(fx).GetComponent<Projectile>();
        if (sender == null || sender.ProjectileSpawnPoint == null)
            p.transform.position = Vector2.zero;
        else
            p.transform.position = sender.ProjectileSpawnPoint.position;

        p.Init(target.transform);

        target.DealDamage(values[0]);

        return true;
    }
}