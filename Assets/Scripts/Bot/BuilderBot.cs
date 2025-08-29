using System;
using System.Collections;
using UnityEngine;

public class BuilderBot : MonoBehaviour
{
    private Bot _thisBot;
    private Flag _flag;
    private bool _isBuilding = false;
    private Base _currentBase;

    public event Action Free;

    private void Awake()
    {
        _thisBot = GetComponent<Bot>();
    }

    public void SetBase(Base baseObject)
    {
        _currentBase = baseObject;
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

        if (_currentBase != null)
        {
            Base oldBase = _currentBase;
            _currentBase.RequestBaseCreation(flag.transform.position, _thisBot);
        }
        else
        {
            Debug.LogError("Current base not found!");
        }

        Destroy(flag.gameObject);
        _flag = null;
        Free?.Invoke();
    }

    public void SetTargetFlag(Flag flag)
    {
        _flag = flag;
        _isBuilding = true;
    }
}