using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Material _materialPrefab;
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private Base _basePrefab;

    private Material _startMaterial;
    private MeshRenderer _renderer;
    private bool isFlag = false;
    private Flag _newFlag;

    public bool IsBuilding => isFlag;

    public event Action BuildStarted;
    public event Action BuildCompleted;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _startMaterial = _renderer.material;
    }

    public void PutUpFlag(Vector3 position)
    {
        if (isFlag)
        {
            if (_newFlag == null)
            {
                _newFlag = Instantiate(_flagPrefab, position, Quaternion.identity);
                BuildStarted?.Invoke();
            }
            else
            {
                _newFlag.transform.position = position;
            }
        }
    }

    public void CreateBase(Bot bot)
    {
        bot.CreateBase(_newFlag);
        _renderer.material = _startMaterial;
        isFlag = false;
        BuildCompleted?.Invoke();
    }

    public void Work()
    {
        _renderer.material = _materialPrefab;
        isFlag = true;
    }
}