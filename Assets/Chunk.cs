using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MarchingCubes;
using Unity.Mathematics;

[RequireComponent (typeof(MeshRenderer), typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{
    public int dimension;
    public float cubeSize;

    private int[] field;

    // Start is called before the first frame update
    void Start()
    {
        field = new int[dimension * dimension * dimension];

        float3 worldPos = transform.position;

        Func<float3, float> newFunc = (float3 pos) =>
        {
            pos = pos + worldPos;
            return Perlin.Noise((pos)/25);
        };


        StartCoroutine(JobMarching.GetMesh(transform.position, dimension, cubeSize, newFunc, .001f, 20, SetMesh));
    }

    private void SetMesh(Tuple<Vector3[], int[]> newMeshInfo)
    {
        Mesh m = new Mesh();
        m.vertices = newMeshInfo.Item1;
        m.triangles = newMeshInfo.Item2;
        m.RecalculateNormals();

        var mf = GetComponent<MeshFilter>();
        mf.mesh = m;
    }

    private int CoordToIndex(int x, int y, int z)
    {
        return x + y * dimension + z * dimension * dimension;
    }

}
