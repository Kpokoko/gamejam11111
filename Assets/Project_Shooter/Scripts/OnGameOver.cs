using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGameOver : MonoBehaviour
{
    public void OnGameOverEventHandler()
    {
        Debug.Log("тварь");
        SceneManager.LoadScene("GameOverScene");
    }
}
