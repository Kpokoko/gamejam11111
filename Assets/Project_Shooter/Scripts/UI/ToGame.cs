using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToGame : MonoBehaviour
{
    public Canvas canvas;
    public Sprite nextFrame;
    private bool timerStarted = false;
    private float timer = 1f;
    
    public void Update()
    {
        if (timerStarted)
            timer -= Time.deltaTime;
        if (Input.anyKeyDown)
        {
            canvas.gameObject.GetComponent<Image>().sprite = nextFrame;
            timerStarted = true;
            if (timer <= 0)
                SceneManager.LoadScene("Level_1");
        }
    }
}
