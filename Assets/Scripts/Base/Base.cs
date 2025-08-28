using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseCreatedBot))]
[RequireComponent(typeof(BaseWallet))]
[RequireComponent(typeof(BaseBuilder))]
public class Base : MonoBehaviour
{
    [SerializeField] private BaseSpawner _baseSpawner;

    private Scanner _scanner;
    private List<Bot> _bots = new List<Bot>();
    private BaseCreatedBot _baseCreatedBot;
    private BaseWallet _baseWallet;
    private BaseBuilder _baseBuilder;
    private bool _isBaseBuilding = false;

    public int BotsCount => _bots.Count;

    private void Awake()
    {
        _baseCreatedBot = GetComponent<BaseCreatedBot>();
        _baseWallet = GetComponent<BaseWallet>();
        _baseBuilder = GetComponent<BaseBuilder>();
    }

    public void Initialize(Scanner scanner, BaseSpawner baseSpawner, Bot builderBot = null)
    {
        _scanner = scanner;
        _baseSpawner = baseSpawner;

        if (builderBot != null)
            AddBot(builderBot);
        else
            StartCoroutine(DelayedCreateBot());
    }

    private IEnumerator DelayedCreateBot()
    {
        yield return null;
        CreateBot();
    }

    private void OnEnable()
    {
        _baseWallet.BotCreated += CreateBot;
        _baseWallet.BaseCreated += CreateBase;
    }

    private void OnDisable()
    {
        _baseWallet.BotCreated -= CreateBot;
        _baseWallet.BaseCreated -= CreateBase;
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
                    freeBot.IsBusy = true;
                    Build(freeBot);
                    _isBaseBuilding = false;
                }
                else
                {
                    freeBot.IsBusy = true;
                    freeBot.GoAfterResource(_scanner.GetResource());
                }
            }
        }
    }

    private Bot FindFreeBot()
    {
        foreach (Bot bot in _bots)
            if (bot.IsBusy == false)
                return bot;

        return null;
    }

    public void AddBot(Bot bot)
    {
        if (_bots.Contains(bot) == false)
        {
            _bots.Add(bot);
            bot.SetBase(this);

            if (_baseSpawner != null)
                bot.SetBaseSpawner(_baseSpawner);
        }
    }

    public void ClearBot()
    {
        foreach (Bot bot in _bots)
            Destroy(bot.gameObject);

        _bots.Clear();
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
        Bot newBot = _baseCreatedBot.Create();

        if (_bots.Contains(newBot) == false)
        {
            _bots.Add(newBot);

            if (_baseSpawner != null)
                newBot.SetBaseSpawner(_baseSpawner);
        }
    }

    public void RemoveBot(Bot bot)
    {
        if (_bots.Contains(bot))
            _bots.Remove(bot);
    }
}