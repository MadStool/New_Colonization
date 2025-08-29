using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BotCreator))]
[RequireComponent(typeof(Wallet))]
[RequireComponent(typeof(BulderBase))]
[RequireComponent(typeof(CollectorBase))]
public class Base : MonoBehaviour
{
    [SerializeField] private SpawnerBase _baseSpawner;
    [SerializeField] private int _botCost = 3;
    [SerializeField] private int _baseCost = 5;

    private bool _inBaseBeingBuilt = false;
    private Scanner _scanner;
    private List<Bot> _bots = new List<Bot>();
    private BotCreator _baseCreatedBot;
    private Wallet _baseWallet;
    private BulderBase _baseBuilder;
    private CollectorBase _baseCollector;
    private bool _isBaseBuilding = false;

    public int BotsCount => _bots.Count;

    public event Action<int> BotsCountChanged;

    private void Awake()
    {
        _baseCreatedBot = GetComponent<BotCreator>();
        _baseWallet = GetComponent<Wallet>();
        _baseBuilder = GetComponent<BulderBase>();
        _baseCollector = GetComponent<CollectorBase>();
    }

    private void OnEnable()
    {
        _baseCollector.ResourceDelivered += OnResourceCollected;
        _baseBuilder.BuildStarted += OnBaseBuildStarted;
    }

    private void OnDisable()
    {
        _baseCollector.ResourceDelivered -= OnResourceCollected;
        _baseBuilder.BuildStarted -= OnBaseBuildStarted;
    }

    private void Update()
    {
        if (_scanner != null && _scanner.TryHereResources())
        {
            Bot freeBot = FindFreeBot();

            if (freeBot != null)
            {
                if (_isBaseBuilding)
                {
                    Build(freeBot);
                    _isBaseBuilding = false;
                }
                else
                {
                    freeBot.GoAfterResource(_scanner.GetResource());
                }
            }
        }
    }

    public void Initialize(Scanner scanner, SpawnerBase baseSpawner, Bot builderBot = null)
    {
        _scanner = scanner;
        _baseSpawner = baseSpawner;

        if (builderBot != null)
            AddBot(builderBot);
        else
            StartCoroutine(DelayedCreateBot());
    }

    public void RemoveBot(Bot bot)
    {
        if (_bots.Contains(bot))
        {
            _bots.Remove(bot);
            BotsCountChanged?.Invoke(_bots.Count);
        }
    }

    private IEnumerator DelayedCreateBot()
    {
        yield return null;
        CreateBot();
    }

    private void OnResourceCollected(Bot bot)
    {
        _baseWallet.AddPoint();
        TryCreateBot();
        TryCreateBase();
    }

    private void OnBaseBuildStarted()
    {
        _inBaseBeingBuilt = true;
    }

    private void TryCreateBot()
    {
        if (_inBaseBeingBuilt == false && _baseWallet.TrySpend(_botCost))
            CreateBot();
    }

    private void TryCreateBase()
    {
        if (_inBaseBeingBuilt && _baseWallet.TrySpend(_baseCost))
        {
            _inBaseBeingBuilt = false;
            CreateBase();
        }
    }

    private Bot FindFreeBot()
    {
        foreach (Bot bot in _bots)
            if (bot.IsBusy == false)
                return bot;

        return null;
    }

    private void AddBot(Bot bot)
    {
        if (_bots.Contains(bot) == false)
        {
            _bots.Add(bot);
            bot.SetBase(this);

            if (_baseSpawner != null)
                bot.SetBaseSpawner(_baseSpawner);

            BotsCountChanged?.Invoke(_bots.Count);
        }
    }

    private void Build(Bot bot)
    {
        _baseBuilder.CreateBase(bot);
    }

    private void CreateBase()
    {
        _isBaseBuilding = true;
    }

    private void CreateBot()
    {
        Bot bot = _baseCreatedBot.Create();

        if (_bots.Contains(bot) == false)
        {
            _bots.Add(bot);

            if (_baseSpawner != null)
                bot.SetBaseSpawner(_baseSpawner);

            BotsCountChanged?.Invoke(_bots.Count);
        }
    }
}