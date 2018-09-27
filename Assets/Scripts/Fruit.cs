using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fruit : MonoBehaviour {

    abstract public int Value { get;  }

    abstract public int RotationSpeed { get; }
}
