using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBot : MonoBehaviour
{
    private Bot _thisBot;
    private SpawnerBase _baseSpawner;
    private Flag _flag;
    private bool _isBuilding = false;

    public event Action Free;

    private void Awake()
    {
        _thisBot = GetComponent<Bot>();
    }

    public void SetBaseSpawner(SpawnerBase spawnerBase)
    {
        _baseSpawner = spawnerBase;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isBuilding && other.TryGetComponent<Flag>(out Flag flag) && _flag == flag)
            StartCoroutine(BuildBase(flag));
    }

    private IEnumerator BuildBase(Flag flag)
    {
        _isBuilding = false;

        yield return new WaitUntil(() => Vector3.Distance(transform.position, flag.transform.position) < 0.1f);

        transform.position = flag.transform.position;
        Destroy(flag.gameObject);
        _flag = null;

        if (_baseSpawner == null)
        {
            Debug.LogError("BaseSpawner not set!");
            yield break;
        }

        Base createdBase = _baseSpawner.SpawnBase(transform.position, _thisBot);

        Base originalBase = _thisBot.transform.parent.GetComponent<Base>();

        if (originalBase != null)
            originalBase.RemoveBot(_thisBot);

        Free?.Invoke();
    }

    public void SetTargetFlag(Flag flag)
    {
        _flag = flag;
        _isBuilding = true;
    }
}