using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    private SpriteRenderer _actionIcon = null;

    //[SerializeField]
    //private drop

    private Enemy_SO _data = null;

    private int _actionIndex = 0;

    public void InitEnemy(Enemy_SO data, EntityUI ui)
    {
        _data = data;
        _shipSprite.sprite = _data.sprite;

        _shipSprite.transform.localScale = data.size;

        //Init action to a random one
        _actionIndex = Random.Range(0, _data.actions.Length);
        InitAction();

        InitEntity(_data.maxHealth, ui);
    }

    public override void StartTurn()
    {
        base.StartTurn();

        //Do action
        _data.actions[_actionIndex].effect.DoEffect(this, BattleManager.Instance.Player, _data.actions[_actionIndex].values, _data.actions[_actionIndex].fx);
    }

    public override void EndTurn()
    {
        base.EndTurn();

        InitAction();
    }

    protected override void Die()
    {
        _actionIcon.gameObject.SetActive(false);

        base.Die();
    }

    private void InitAction()
    {
        //Set new action index
        switch (_data.order)
        {
            case EEnemyActionOrder.Random:
                _actionIndex = Random.Range(0, _data.actions.Length);
                break;
            case EEnemyActionOrder.Linear:
                _actionIndex++;

                if (_actionIndex >= _data.actions.Length)
                    _actionIndex = 0;
                break;
        }

        //See what will be the next action
        _actionIcon.sprite = _data.actions[_actionIndex].icon;
    }
}