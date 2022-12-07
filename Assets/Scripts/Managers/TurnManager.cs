using System;
using System.Collections;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField]
    private GameObject _nextTurnButton = null;

    [SerializeField] private GameObject skipTurnEffect;

    private ETurnState _state = ETurnState.PlayerBegin;

    public int playerTurnsToSkip = 0;
    public int enemyTurnsToSkip = 0;

    private void Start()
    {
        StartCoroutine(HandleState());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GoToNextState();
    }

    /// <summary>
    /// Goes to the next turn state and handles it
    /// </summary>
    public void GoToNextState()
    {
        _state++;

        if (_state.GetHashCode() >= Enum.GetValues(typeof(ETurnState)).Length)
            _state = ETurnState.PlayerBegin;

        StartCoroutine(HandleState());
    }

    /// <summary>
    /// Handles what should a state do
    /// </summary>
    private IEnumerator HandleState()
    {
        //Skip one frame
        yield return null;

        switch (_state)
        {
            //Player begin
            case ETurnState.PlayerBegin:
                BattleManager.Instance.Player.BeginTurn();
                GoToNextState();
                break;

            //Player turn
            case ETurnState.PlayerTurn:
                if (playerTurnsToSkip > 0)
                {
                    playerTurnsToSkip--;
                    Instantiate(skipTurnEffect, BattleManager.Instance.Player.transform.position, Quaternion.identity);
                    GoToNextState();
                    break;
                }
                BattleManager.Instance.Player.StartTurn();
                CardManager.Instance.DrawHand();
                _nextTurnButton.SetActive(true);
                break;

            //Player end
            case ETurnState.PlayerEnd:
                BattleManager.Instance.Player.EndTurn();
                CardManager.Instance.ClearHand();
                _nextTurnButton.SetActive(false);

                yield return new WaitForSeconds(1);

                GoToNextState();
                break;

            //Enemy begin
            case ETurnState.EnemyBegin:
                if (BattleManager.Instance.Enemy != null)
                    BattleManager.Instance.Enemy.BeginTurn();

                GoToNextState();
                break;

            //Enemy turn
            case ETurnState.EnemyTurn:
                if (enemyTurnsToSkip > 0)
                {
                    enemyTurnsToSkip--;
                    Instantiate(skipTurnEffect, BattleManager.Instance.Enemy.transform.position, Quaternion.identity);
                    GoToNextState();
                    break;
                }
                if (BattleManager.Instance.Enemy != null)
                    BattleManager.Instance.Enemy.StartTurn();

                yield return new WaitForSeconds(1);

                GoToNextState();
                break;

            //Enemy end
            case ETurnState.EnemyEnd:
                if (BattleManager.Instance.Enemy != null)
                    BattleManager.Instance.Enemy.EndTurn();

                if (BattleManager.Instance.Enemy == null)
                    BattleManager.Instance.SpawnEnemy();

                GoToNextState();
                break;
        }
    }
}