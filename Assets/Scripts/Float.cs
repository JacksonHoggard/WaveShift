using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float waterLevel;
    public float floatHeight;
    public Vector3 buoyancyCentreOffset;
    public float bounceDamp;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
        var forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
        
        if (forceFactor > 0f) {
            var uplift = -Physics.gravity * (forceFactor - _rigidbody.velocity.y * bounceDamp);
            _rigidbody.AddForceAtPosition(uplift, actionPoint);
        }
    }
}
