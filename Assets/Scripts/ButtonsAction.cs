using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsAction : MonoBehaviour
{
    public void PlayAgain() => SceneManager.LoadScene("Game");

    public void Quit() => Application.Quit();

    public void DeactivateQuitPanel() => BattleManager.Instance.quitPanel.SetActive(false);
}
