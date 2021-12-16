using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Color[] _colors;

    public int xSize = 20;
    public int zSize = 20;
    public Gradient gradient;
    public Vector4 waveA = new Vector4(1, 0, 0.5f, 10);
    public Vector4 waveB = new Vector4(0, 1, 0.25f, 20);
    public Vector4 waveC = new Vector4(1, 1, 0.15f, 10);

    private float _minWaveHeight;
    private float _maxWaveHeight;

    private Vector3 _pos;
    
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _pos = transform.position;

        CreateShape();
        UpdateMesh();
    }

    Vector3 GerstnerWave(Vector4 wave, Vector3 p)
    {
        float steepness = wave.z;
        float wavelength = wave.w;
        float k = 2 * Mathf.PI / wavelength;
        float c = Mathf.Sqrt(9.8f / k);
        Vector2 d = new Vector2(wave.x, wave.y).normalized;
        float f = k * (Vector2.Dot(d, new Vector2(p.x, p.z)) - c * Time.fixedTime);
        float a = steepness / k;

        return new Vector3(d.x * (a * Mathf.Cos(f)), a * Mathf.Sin(f), d.y * (a * Mathf.Cos(f)));
    }
    
    void CreateShape()
    {
        _vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= zSize; x++, i++)
            {
                Vector3 temp = new Vector3(x + _pos.x, 0, z + _pos.z);
                temp += GerstnerWave(waveA, temp) + GerstnerWave(waveB, temp) + GerstnerWave(waveC, temp);
                _vertices[i] = new Vector3(temp.x - _pos.x, temp.y, temp.z - _pos.z);

                if (temp.y > _maxWaveHeight)
                    _maxWaveHeight = temp.y;
                if (temp.y < _minWaveHeight)
                    _minWaveHeight = temp.y;
            }
        }

        _triangles = new int[xSize * zSize * 6];
        
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                _triangles[tris] = vert;
                _triangles[tris + 1] = vert + xSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + xSize + 1;
                _triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }

        _colors = new Color[_vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                float height = Mathf.InverseLerp(_minWaveHeight, _maxWaveHeight, _vertices[i].y);
                _colors[i] = gradient.Evaluate(height);
            }
        }
    }

    void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.colors = _colors;
        
        _mesh.RecalculateNormals();
    }
    
    void Update()
    {
        CreateShape();
        UpdateMesh();
    }
}
