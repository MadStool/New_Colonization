using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseCreatedBot))]
[RequireComponent(typeof(BaseWallet))]
[RequireComponent(typeof(BaseBuilder))]
public class Base : MonoBehaviour
{
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
        FillFields();
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
        if (_scanner.TryHereResources())
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
        _bots.Add(bot);
        bot.SetBase(this);
    }

    public void ClearBot()
    {
        foreach (Bot bot in _bots)
            Destroy(bot.gameObject);

        _bots.Clear();
    }

    public void FillFields()
    {
        _scanner = transform.GetComponentInParent<Scanner>();
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
        _bots.Add(newBot);
    }

    public void RemoveBot(Bot bot)
    {
        if (_bots.Contains(bot))
            _bots.Remove(bot);
    }
}