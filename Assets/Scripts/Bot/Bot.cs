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

    public bool IsBusy { get; set; }

    public void SetBase(Base baseObject)
    {
        _currentBase = baseObject;
        _targetBase = baseObject.transform.position;

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        _botMover = GetComponent<BotMover>();
        _botCollector = GetComponent<BotCollector>();
        _botBuilder = GetComponent<BotBuilder>();

        _botCollector.ResourceCollected += AssignResourceBase;
        _botBuilder.Free += GetFree;
    }

    private void OnDisable()
    {
        _botCollector.ResourceCollected -= AssignResourceBase;
        _botBuilder.Free -= GetFree;
    }

    public void CreateBase(Flag flag)
    {
        IsBusy = true;
        _botBuilder.SetTargetFlag(flag);
        _botMover.SetTargetPosition(flag.transform.position);
    }

    public void SubmitResource(Resource resource)
    {
        Destroy(resource.gameObject);
    }

    public void GoAfterResource(Resource resource)
    {
        IsBusy = true;
        _botCollector.SetTargetResource(resource);
        _botMover.SetTargetPosition(resource.transform.position);
    }

    private void GetFree()
    {
        IsBusy = false;
    }

    private void AssignResourceBase()
    {
        _botMover.SetTargetPosition(_targetBase);
        StartCoroutine(CheckReturnToBase());
    }

    private IEnumerator CheckReturnToBase()
    {
        while (Vector3.Distance(transform.position, _targetBase) > 0.5f)
            yield return null;

        IsBusy = false;
    }
}