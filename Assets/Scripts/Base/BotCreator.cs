using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCreator : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot Create()
    {
        Bot bot = Instantiate(_botPrefab, transform.position, Quaternion.identity);
        bot.transform.parent = transform;

        Base baseComponent = GetComponent<Base>();

        if (baseComponent != null)
            bot.SetBase(baseComponent);

        return bot;
    }
}