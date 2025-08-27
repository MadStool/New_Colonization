using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreatedBot : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot Create()
    {
        Bot bot = Instantiate(_botPrefab, transform.position, Quaternion.identity);
        bot.transform.parent = transform;
        bot.SetBase(GetComponent<Base>());

        return bot;
    }
}