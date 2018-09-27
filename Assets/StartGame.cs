using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class StartGame : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button startGameButton;

    public Button easyDifficultyButton;
    public Button hardDifficultyButton;
    public float transitionSpeed;

    public GameObject ThemeSong;


    private bool _isShrinking;

    private float _scale;

    void Start()
    {

        //Calls the TaskOnClick/TaskWithParameters method when you click the Button
        startGameButton.onClick.AddListener( ShrinkStartButton); 
        easyDifficultyButton.onClick.AddListener(delegate { DifficultySelect(easyDifficultyButton); });
        hardDifficultyButton.onClick.AddListener(delegate { DifficultySelect(hardDifficultyButton); });
       
    }

    void DifficultySelect(Button button)
    {
        string difficulty = button.GetComponentInChildren<Text>().text;
        Debug.Log("User selected " + difficulty);
        ThemeSong.GetComponent<AudioSource>().loop = false;
        
        switch (difficulty)
        {
            case "Easy":
                SceneManager.LoadScene(1);
                break;
            case "Hard":
                SceneManager.LoadScene(2);
                break;
        }
        
        
    }




    void Update()
    {
        if (_isShrinking)
        {
            Debug.Log("SHRINKING");
            _isShrinking = ScaleRectTransform(startGameButton, _scale) >= 0;
            ScaleRectTransform(easyDifficultyButton, -_scale);
            ScaleRectTransform(hardDifficultyButton, -_scale);
            if (!_isShrinking)
            {
                Debug.Log("Destroying startGameButton");
                Destroy(startGameButton.gameObject);
            }
        } 
    }


    void ShrinkStartButton()
    {
        Debug.Log("Shrinking Start Button");
        _isShrinking = true;
        _scale = 0.1f * transitionSpeed;

    }

    public static float ScaleRectTransform(Button btn, float scalingSpeed)
    {
        btn.GetComponent<RectTransform>().localScale = new Vector3(btn.GetComponent<RectTransform>().localScale.x - scalingSpeed*Time.deltaTime, btn.GetComponent<RectTransform>().localScale.y, btn.GetComponent<RectTransform>().localScale.z);

        return btn.GetComponent<RectTransform>().localScale.x;
;    }


}