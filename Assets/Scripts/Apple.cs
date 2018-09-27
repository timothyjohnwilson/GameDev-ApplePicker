
using UnityEngine;


public class Apple :  Fruit{

    public static float bottomY = -20f;

    public int value = 1000;


    public int rotationSpeed = 3;

    public override int RotationSpeed
    {
        get
        {
            return rotationSpeed;
        }
    }

    public override int Value
    {
        get
        {
            return value;
        }
    }

    public float speed = 1f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Physics.gravity * speed;
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.back * Time.deltaTime * rotationSpeed);

        if (transform.position.y <= bottomY)
        {
            Destroy(this.gameObject);
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.AppleDestroyed();
        }
	}
}
