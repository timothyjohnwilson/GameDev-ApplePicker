using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour {

    public Text scoreGT;

    void Start()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<Text>();

        scoreGT.text = "0";
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos2D = Input.mousePosition;

        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
	}

    void OnCollisionEnter(Collision coll)
    {
        GameObject collidedWith = coll.gameObject;
        if(collidedWith.tag == "Apple")
        {
            Fruit fruit = collidedWith.GetComponent<Apple>();
           
            //HANDLE THE AUDIO PLAYBACK
            AudioSource audio = collidedWith.GetComponent<AudioSource>();
            audio.Play();
            Destroy(collidedWith.GetComponent<Renderer>());
            Destroy(collidedWith.GetComponent<Rigidbody>());
            Destroy(collidedWith.gameObject, audio.clip.length);
            int score = int.Parse(scoreGT.text);
            score += fruit.Value;
            scoreGT.text = score.ToString();

            //track high score
            if(score > HighScore.score)
            {
                HighScore.score = score;
            }
        }
    }
}
