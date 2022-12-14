using UnityEngine;

[CreateAssetMenu(fileName = "New acid", menuName = "Effects/Acid")]
public class AcidEffect : Effect_SO
{
    public GameObject fx = null;

    public override void Tick(Entity owner, int nb)
    {
        int remainingDamagesFromAcid = owner.RemoveShield(nb);

        if (remainingDamagesFromAcid > 0) owner.DealDamage(remainingDamagesFromAcid / 2);

        if (!owner.IsDead)
        {
            owner.RemoveStackFromEffect(this, 1);
            Instantiate(fx, owner.transform.position, Quaternion.identity);
        }
    }
}