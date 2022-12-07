using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    private TextMeshProUGUI _name = null;

    [SerializeField]
    private TextMeshProUGUI _cost = null;

    [SerializeField]
    private Image _artwork = null;

    [SerializeField]
    private TextMeshProUGUI _description = null;

    [SerializeField]
    private Animator _anim = null;

    private Card_SO _data = null;

    /// <summary>
    /// Initializes the card interface
    /// </summary>
    public void InitCard(Card_SO data)
    {
        _data = data;

        _name.text = _data.name;
        _cost.text = _data.cost.ToString();
        _artwork.sprite = _data.artwork;
        _description.text = _data.GetDescription();
    }

    /// <summary>
    /// Triggers the animation to destroy the card
    /// </summary>
    public void StartDestoyCard()
    {
        _anim.SetTrigger("Destroy");
    }

    /// <summary>
    /// Actually destroys the card
    /// </summary>
    public void DestroyCard()
    {
        Destroy(gameObject);
        CardManager.Instance.OnCardDestroyed();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Click on the card
        if(BattleManager.Instance.Player.UseEnergy(_data.cost))
        {
            if (_data.Play())
                CardManager.Instance.RemoveCard(this);
            else
                BattleManager.Instance.Player.GiveEnergy(_data.cost);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Mouse over the card
        _anim.SetBool("MouseHover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Mouse exit the card
        _anim.SetBool("MouseHover", false);
    }
}