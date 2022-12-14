using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card_SO : ScriptableObject
{
    /// <summary>
    /// Name on top of the card
    /// </summary>
    public string cardName = string.Empty;

    /// <summary>
    /// Cost in energy of the card
    /// </summary>
    public int cost = 0;

    /// <summary>
    /// Sprite in the middle of the card
    /// </summary>
    public Sprite artwork = null;

    /// <summary>
    /// Description of the card's effect
    /// </summary>
    [TextArea(1, 6)]
    public string description = string.Empty;

    /// <summary>
    /// Values that can be used by the effect and the description
    /// </summary>
    public int[] values = null;

    /// <summary>
    /// All effects applied when the card is played
    /// </summary>
    public CardEffect[] effects = null;

    /// <summary>
    /// Returns the description with correct values
    /// </summary>
    public string GetDescription()
    {
        //Split the description to know if it needs some values
        string[] cutDescription = description.Split('{');

        //Replace the values in the description if needed
        switch (cutDescription.Length)
        {
            case 1:
                return description;
            case 2:
                if(values.Length >= 1)
                    return string.Format(description, values[0]);
                break;
            case 3:
                if (values.Length >= 2)
                    return string.Format(description, values[0], values[1]);
                break;
            case 4:
                if (values.Length >= 3)
                    return string.Format(description, values[0], values[1], values[2]);
                break;
        }

        return description;
    }

    /// <summary>
    /// Plays the card, do all effects in order
    /// </summary>
    public bool Play()
    {
        bool cardPlayed = true;
        foreach (CardEffect e in effects)
        {
            int[] usedValues = new int[e.valuesIndex.Length];

            for (int i = 0; i < e.valuesIndex.Length; i++)
            {
                usedValues[i] = values[e.valuesIndex[i]];
            }

            bool effectPlayed = e.effect.DoEffect(BattleManager.Instance.Player, BattleManager.Instance.Enemy, usedValues, e.fx);


            if (!effectPlayed)
                cardPlayed = false;
        }

        return cardPlayed;
    }

    [Serializable]
    public struct CardEffect
    {
        public CardEffect_SO effect;
        public int[] valuesIndex;
        public GameObject fx;
    }
}