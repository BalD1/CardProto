using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drop", menuName = "Drop")]
public class Drop_SO : ScriptableObject
{
    [field: SerializeField] public DropWithWeight[] dropTable { get; private set; }

    [System.Serializable]
    public struct DropWithWeight
    {
#if UNITY_EDITOR
        public string editorName;
#endif
        public Card_SO[] objectsToDrop;
        public float weight;
    }

#if UNITY_EDITOR
    [HideInInspector] public float totalWeight;
    [HideInInspector] public int totalDrops;
#endif

    private DropWithWeight GetRandomDrop()
    {
        float totalWeight = 0;
        foreach (DropWithWeight drop in dropTable)
        {
            totalWeight += drop.weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;
        foreach (DropWithWeight d in dropTable)
        {
            currentWeight += d.weight;
            if (currentWeight >= randomValue)
            {
                return d;
            }
        }

        return new DropWithWeight();
    }

    public bool TryDrop()
    {
        DropWithWeight drop = GetRandomDrop();

        if (drop.objectsToDrop.Length <= 0) return false;

        CardManager.Instance.AddCardsToDeck(drop.objectsToDrop);

        Vector2 textPopupPos = BattleManager.Instance.Enemy.transform.position;
        textPopupPos.y += 1;

        StringBuilder popupText = new StringBuilder();

        foreach (var item in drop.objectsToDrop) popupText.AppendFormat($"+1 {item?.cardName} \n");

        TextPopup.Create(textPopupPos, popupText.ToString());

        return true;
    }
}
