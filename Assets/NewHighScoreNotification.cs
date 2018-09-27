using UnityEngine;
using UnityEngine.UI;

public class NewHighScoreNotification : MonoBehaviour {


	void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore") && HighScore.newHighScore)
        {
            Text newHighScore = GetComponent<Text>();
            newHighScore.text = "New High Score: " + PlayerPrefs.GetInt("HighScore");

        }
    }
}
