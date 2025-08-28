using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBuilder : MonoBehaviour
{
    private Bot _thisBot;
    private BaseSpawner _baseSpawner;
    private Flag _flag;
    private bool _isBuilding = false;

    public event Action Free;

    private void Awake()
    {
        _thisBot = GetComponent<Bot>();
    }

    public void SetBaseSpawner(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
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

        Base newBase = _baseSpawner.SpawnBase(transform.position, _thisBot);

        Base oldBase = _thisBot.transform.parent.GetComponent<Base>();

        if (oldBase != null)
            oldBase.RemoveBot(_thisBot);

        _thisBot.IsBusy = false;
        Free?.Invoke();
    }

    public void SetTargetFlag(Flag flag)
    {
        _flag = flag;
        _isBuilding = true;
    }
}