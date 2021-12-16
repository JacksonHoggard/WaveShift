using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject camera;
    public GameObject wavePrefab;
    private Transform _cameraTransform;
    private GameObject[] _waves;

    void Start()
    {
        _cameraTransform = camera.GetComponent<Transform>();

        int offsetX = -200;
        int offsetZ = -200;
        for (int i = 0; i < 16; i++)
        {
            wavePrefab.transform.position = new Vector3(offsetX, 0, offsetZ);

            offsetX += 100;

            if (offsetX > 100)
            {
                offsetX = -200;
                offsetZ += 100;
            }

            Instantiate(wavePrefab);
        }
    }

    private void Update()
    {
        // foreach (var g in _waves)
        // {
        //     if (!g.GetComponent<MeshRenderer>().isVisible)
        //         g.SetActive(false);
        //     else
        //         g.SetActive(true);
        // }
    }
}
