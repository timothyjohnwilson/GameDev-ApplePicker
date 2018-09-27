using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToTitleScreen : MonoBehaviour {

    public int titleScreenSceneNumber;
    private Button backToTitleScreen;

    void Start()
    {
        backToTitleScreen = GetComponent<Button>();
        backToTitleScreen.onClick.AddListener(LoadTitleScreen);
    }

    void LoadTitleScreen()
    {
        SceneManager.LoadScene(titleScreenSceneNumber);
    }


}
