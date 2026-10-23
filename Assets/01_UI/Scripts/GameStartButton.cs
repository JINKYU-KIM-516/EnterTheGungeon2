using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartButton : MonoBehaviour
{    
    public void PlayButtonPress()
    {
        SceneManager.LoadScene("InGame");
    }
}
