
using UnityEngine;


public class MouseController : MonoBehaviour {

    public bool enableCursor = true;

	// Use this for initialization
	void Start () {
        Cursor.visible = enableCursor;
    }
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
	
}
