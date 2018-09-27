using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

    static public int score = 1000;
    static public bool newHighScore = false;

    void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore");
        }

        PlayerPrefs.SetInt("HighScore", score);
    }
	
	// Update is called once per frame
	void Update () {
        Text gt = GetComponent<Text>();
        gt.text = "High Score: " + score;

        if(score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            newHighScore = true;
        }
	}
}
