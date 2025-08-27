using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseCollector))]
[RequireComponent(typeof(BaseBuilder))]
public class BaseWallet : MonoBehaviour
{
    private int _countPoints;
    private int _countPointsCreateBot = 3;
    private int _countPointsCreateBase = 5;
    private bool _inBaseBeingBuilt = false;
    public int CurrentPoints => _countPoints;

    private BaseCollector _baseResourceCollector;
    private BaseBuilder _baseBuilder;

    public event Action BotCreated;
    public event Action BaseCreated;


    private void Awake()
    {
        _baseResourceCollector = GetComponent<BaseCollector>();
        _baseBuilder = GetComponent<BaseBuilder>();
    }

    private void OnEnable()
    {
        _baseResourceCollector.Collected += AddPoint;
        _baseBuilder.BaseBeganBuilt += Build;
    }

    private void OnDisable()
    {
        _baseResourceCollector.Collected -= AddPoint;
        _baseBuilder.BaseBeganBuilt -= Build;
    }

    private void Build()
    {
        _inBaseBeingBuilt = true;
    }

    private void CanCreateBot()
    {
        if (_countPoints >= _countPointsCreateBot && _inBaseBeingBuilt == false)
        {
            _countPoints -= _countPointsCreateBot;
            BotCreated?.Invoke();
        }
    }

    private void CanCreateBase()
    {
        if (_countPoints >= _countPointsCreateBase && _inBaseBeingBuilt)
        {
            _countPoints -= _countPointsCreateBase;
            _inBaseBeingBuilt = false;
            BaseCreated?.Invoke();
        }
    }

    private void AddPoint()
    {
        _countPoints++;

        CanCreateBot();
        CanCreateBase();
    }
}