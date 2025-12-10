using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGame : MonoBehaviour
{
    public void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Level_1");
        }
    }
}
