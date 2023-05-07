using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class InstancedIndirectGrassPosDefine : MonoBehaviour
{
    [SerializeField] private MeshCollider _boundsMeshCollider;
    [SerializeField] private float _randomPositionRadius = 0.1f;
    [SerializeField] private Text _instanceCountMesh;
    [SerializeField] private string _instanceCountTextFormat = "Grass Instances: {0}";

    [Range(1f, 8f)]
    [SerializeField] private float _densityPerUnit;

    private Vector3 _startPosCached;
    private Vector3 _endPosCached;
    private float _densityCached;

    private bool CacheIsValid()
    {
        return _boundsMeshCollider.bounds.min == _startPosCached && _boundsMeshCollider.bounds.max == _endPosCached && _densityPerUnit == _densityCached;
    }

    private void CacheValues()
    {
        _startPosCached = _boundsMeshCollider.bounds.min;
        _endPosCached = _boundsMeshCollider.bounds.max;
        _densityCached = _densityPerUnit;
    }

    void Start()
    {
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

        var startX = _boundsMeshCollider.bounds.min.x;
        var endX = _boundsMeshCollider.bounds.max.x;
        var startZ = _boundsMeshCollider.bounds.min.z;
        var endZ = _boundsMeshCollider.bounds.max.z;

        var count = 0;

        for (var x = startX; x < endX; x += 1 / _densityPerUnit)
        {
            for (var z = startZ; z < endZ; z += 1 / _densityPerUnit)
            {
                count++;
            }
        }

        _instanceCountMesh.text = string.Format(_instanceCountTextFormat, count);

        List<Vector3> positions = new List<Vector3>(count);

        for (var x = startX; x < endX; x += 1 / _densityPerUnit)
        {
            for (var z = startZ; z < endZ; z += 1 / _densityPerUnit)
            {
                var random = Random.insideUnitCircle * _randomPositionRadius;

                positions.Add(new Vector3(x, 0f, z) + new Vector3(random.x, 0f, random.y));
            }
        }

        InstancedIndirectGrassRenderer.instance.allGrassPos = positions;
    }

}
