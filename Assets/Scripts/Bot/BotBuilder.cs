using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class BotBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    private Flag _flag;
    private bool _isBuilding = false;

    public event Action Free;

    private void OnTriggerEnter(Collider other)
    {
        if (_isBuilding && other.TryGetComponent<Flag>(out Flag flag) && _flag == flag)
        {
            StartCoroutine(BuildBase(flag));
        }
    }

    private IEnumerator BuildBase(Flag flag)
    {
        _isBuilding = false;

        yield return new WaitUntil(() => Vector3.Distance(transform.position, flag.transform.position) < 0.1f);

        transform.position = flag.transform.position;
        Destroy(flag.gameObject);
        _flag = null;

        Base newBase = Instantiate(_basePrefab, transform.position, Quaternion.identity);
        newBase.ClearBot();
        newBase.transform.parent = transform.GetComponentInParent<Base>().GetComponentInParent<Scanner>().transform;
        newBase.FillFields();

        Base oldBase = transform.parent.GetComponent<Base>();

        if (oldBase != null)
            oldBase.RemoveBot(GetComponent<Bot>());

        Bot bot = GetComponent<Bot>();
        bot.SetBase(newBase);
        newBase.AddBot(bot);

        bot.IsBusy = false;
        Free?.Invoke();
    }

    public void SetTargetFlag(Flag flag)
    {
        _flag = flag;
        _isBuilding = true;
    }
}