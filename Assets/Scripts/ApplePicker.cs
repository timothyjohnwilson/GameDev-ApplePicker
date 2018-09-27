using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour {
    [Header("Set in Inspector")]

    public GameObject basketPrefab;
    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;

    public AudioSource hurtBasketClip;

    public List<GameObject> basketList;

    private int flashCount;
    public int numberOfFlashes = 3;

    public Material flashMaterial;
    public Material standardMaterial;

    private bool wasFlashing = false;

	// Use this for initialization
	void Start () {

        basketList = new List<GameObject>();
	    for(int i = 0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;

            basketList.Add(tBasketGO);
        }

        wasFlashing = false;
    }

    void Update()
    {
        if (flashCount > 0 )
        {
            
            if (Time.frameCount % 10 == 0)
            {
                foreach (GameObject basket in basketList)
                {
                    basket.GetComponent<Renderer>().material = flashMaterial;
                    flashCount--;
                }
            }
            else if(Time.frameCount % 17 == 0)
            {
                foreach (GameObject basket in basketList)
                {
                    basket.GetComponent<Renderer>().material = standardMaterial;

                }
            }
            wasFlashing = true;
        }
        else if(wasFlashing)
        {
            foreach (GameObject basket in basketList)
            {
                basket.GetComponent<Renderer>().material = standardMaterial;

            }
            wasFlashing = false;
        }
    }

    public void AppleDestroyed()
    {
        if (basketList.Count > 1)
        {
            hurtBasketClip.Play();
        }

        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach(GameObject tGO in tAppleArray)
        {
            Destroy(tGO);
        }

        int basketIndex = basketList.Count - 1;
        GameObject tBasketGo = basketList[basketIndex];

        basketList.RemoveAt(basketIndex);
        Debug.Log(basketIndex);
        flashCount = numberOfFlashes;

        Destroy(tBasketGo);
        


        if(basketList.Count == 0)
        {
            SceneManager.LoadScene(3);
        }
    }
	
}
