using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
[RequireComponent(typeof(BotCollector))]
[RequireComponent(typeof(BotBuilder))]
public class Bot : MonoBehaviour
{
    private Base _currentBase;
    private BotMover _botMover;
    private BotCollector _botCollector;
    private BotBuilder _botBuilder;
    private Vector3 _targetBase;
    private Coroutine _currentAction;
    private bool _isInitialized = false;
    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    public void SetBase(Base baseObject)
    {
        _currentBase = baseObject;
        _targetBase = baseObject.transform.position;
        InitializeComponents();
    }

    public void SetBaseSpawner(BaseSpawner baseSpawner)
    {
        if (_botBuilder != null)
            _botBuilder.SetBaseSpawner(baseSpawner);
    }

    private void InitializeComponents()
    {
        _botMover = GetComponent<BotMover>();
        _botCollector = GetComponent<BotCollector>();
        _botBuilder = GetComponent<BotBuilder>();

        _botCollector.ResourceCollected += OnResourceCollected;
        _botBuilder.Free += OnBuildingComplete;

        _isInitialized = true;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _botCollector.ResourceCollected -= OnResourceCollected;
        _botBuilder.Free -= OnBuildingComplete;
        StopCurrentAction();
    }

    private void StopCurrentAction()
    {
        if (_currentAction != null)
        {
            StopCoroutine(_currentAction);
            _currentAction = null;
        }
    }

    private void StartNewAction(IEnumerator coroutine)
    {
        StopCurrentAction();
        _currentAction = StartCoroutine(coroutine);
    }

    public void CreateBase(Flag flag)
    {
        SetBusy(true);
        _botBuilder.SetTargetFlag(flag);
        StartNewAction(MoveToPosition(flag.transform.position));
    }

    public void SubmitResource(Resource resource)
    {
        Destroy(resource.gameObject);
    }

    public void GoAfterResource(Resource resource)
    {
        SetBusy(true);
        _botCollector.SetTargetResource(resource);
        StartNewAction(MoveToPosition(resource.transform.position));
    }

    private void OnResourceCollected()
    {
        StartNewAction(ReturnToBase());
    }

    private void OnBuildingComplete()
    {
        SetBusy(false);
    }

    private IEnumerator MoveToPosition(Vector3 position)
    {
        yield return _botMover.MoveTo(position);

        if (_isBusy == false)
            yield break;
    }

    private IEnumerator ReturnToBase()
    {
        yield return _botMover.MoveTo(_targetBase);
        SetBusy(false);
    }

    private void SetBusy(bool busy)
    {
        _isBusy = busy;
    }
}