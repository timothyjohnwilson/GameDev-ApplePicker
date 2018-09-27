using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {
    [Header("=== General Attributes ===")]

    [Tooltip("Prefab for instantiating object")]
    public GameObject applePrefab;

    [Tooltip("Speed at which the AppleTree moves")]
    public float speed = 10f;

    [Tooltip("Distance where AppleTree turns around")]
    public float leftAndRightEdge = 10f;

    [Tooltip("Chance that the AppleTree will change directions")]
    public float chanceToChangeDirections = 0.1f;

    [Tooltip("Rate at which Apples will be instantiated")]
    public float secondsBetweenAppleDrops = 1f;

    [Header("=== For Increasing Difficulty ===")]

    [Tooltip("Enable Variable Apple Speed")]
    public bool varyAppleSpeed = false;

    [Tooltip("Max Speed for Instantiated apples")]
    public float maxAppleSpeed = 5f;

    [Tooltip("Enable Stepper Speed (float to integer)")]
    public bool enableStepperSpeed = false;

    [Tooltip("Enable speed change over time")]
    public bool enableSpeedChangeOverTime = false;

    [Tooltip("The number of seconds in between speed increase")]
    public int timeForSpeedIncrease = 30;

    [Tooltip("Vary Apple Color Based on speed")]
    public bool varyColorBasedOnSpeed = false;

    [Tooltip("The color of the corresponding speed, set it equal to the max speed number")]
    public List<Material> speedMaterials;

    [Tooltip("Variable Value based on apple difficulty")]
    public bool varyValueBasedOnSpeed = false;

    [Tooltip("Set the score multiplier, multiplies by the speed value")]
    public float scoreMultiplier = 1000f;


    private float nextTime;

    private int currentSpeed = 1;


	// Use this for initialization
	void Start () {
        //Dropping Apples every second
        Invoke("DropApple", 2f);
        if (enableSpeedChangeOverTime)
            nextTime = timeForSpeedIncrease;
        else
            currentSpeed = (int)maxAppleSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        // Basic Movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Changing Direction
        if(pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed);
        } else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);
        }



        if (enableSpeedChangeOverTime)
        {
            if(Time.realtimeSinceStartup > nextTime && currentSpeed < maxAppleSpeed)
            {
                currentSpeed++;
                nextTime = Time.realtimeSinceStartup + timeForSpeedIncrease;
                Debug.Log("SPEED INCREASED TO " + currentSpeed);
            }
        } 
    }

    
    void FixedUpdate()
    {
        //Change direction randomly
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1;
        }
    }

    void DropApple()
    {
        GameObject apple = Instantiate(applePrefab);
        var value = Random.value;
        if (varyAppleSpeed)
        {
            var tempvalue = value * currentSpeed + 1;
            if (enableStepperSpeed)
                tempvalue = Mathf.FloorToInt(tempvalue);
            apple.GetComponent<Apple>().speed = tempvalue;
            Debug.Log("Spawning Apple with Speed " + tempvalue);
            if (varyColorBasedOnSpeed)
            {
                var subMaterials = apple.GetComponent<MeshRenderer>().materials;
                subMaterials[0] = speedMaterials[(int)tempvalue - 1];
                apple.gameObject.GetComponent<MeshRenderer>().materials = subMaterials;
            }
            if (varyValueBasedOnSpeed)
            {
                apple.GetComponent<Apple>().value = (int)(tempvalue) * (int)scoreMultiplier;
            }
        }
        

        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }
}
