using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotation : MonoBehaviour
{
    private void Update() =>transform.Rotate(Vector3.up, Time.deltaTime * 40f);
}
