using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class InstancedIndirectGrassPosDefine : MonoBehaviour
{
    [SerializeField] private MeshCollider _meshCollider;
    [SerializeField] private float _randomRadius = 0.1f;
    [SerializeField] private Text _countText;

    [Range(1f, 8f)]
    [SerializeField] private float _densityPerUnit;

    private Vector3 _startPosCached;
    private Vector3 _endPosCached;
    private float _densityCached;

    private int cacheCount = -1;

    private bool CacheIsValid()
    {
        return _meshCollider.bounds.min == _startPosCached && _meshCollider.bounds.max == _endPosCached && _densityPerUnit == _densityCached;
    }

    private void CacheValues()
    {
        _startPosCached = _meshCollider.bounds.min;
        _endPosCached = _meshCollider.bounds.max;
        _densityCached = _densityPerUnit;
    }

    void Start()
    {
        cacheCount = -1;
        UpdatePosIfNeeded();
    }

    private void Update()
    {
        UpdatePosIfNeeded();
    }

    private void UpdatePosIfNeeded()
    {
        if (CacheIsValid()) return;

        CacheValues();

        Debug.Log("UpdatePos (Slow)");

        var startX = _meshCollider.bounds.min.x;
        var endX = _meshCollider.bounds.max.x;
        var startZ = _meshCollider.bounds.min.z;
        var endZ = _meshCollider.bounds.max.z;

        var count = 0;

        for (var x = startX; x < endX; x += 1 / _densityPerUnit)
        {
            for (var z = startZ; z < endZ; z += 1 / _densityPerUnit)
            {
                count++;
            }
        }

        _countText.text = count.ToString();

        List<Vector3> positions = new List<Vector3>(count);

        for (var x = startX; x < endX; x += 1 / _densityPerUnit)
        {
            for (var z = startZ; z < endZ; z += 1 / _densityPerUnit)
            {
                var random = Random.insideUnitCircle * _randomRadius;

                positions.Add(new Vector3(x, 0f, z) + new Vector3(random.x, 0f, random.y));
            }
        }

        InstancedIndirectGrassRenderer.instance.allGrassPos = positions;
        cacheCount = positions.Count;
    }

}
