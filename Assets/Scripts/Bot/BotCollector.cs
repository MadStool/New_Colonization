﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCollector : MonoBehaviour
{
    private Resource _resource;

    public event Action ResourceCollected;

    private void OnTriggerEnter(Collider other)
    {
        float height = 0.5f;

        if (other.TryGetComponent(out Resource resource) && _resource == resource)
        {
            resource.transform.SetParent(transform);
            resource.transform.localPosition = new Vector3(0, height, 0);
            ResourceCollected?.Invoke();
        }
    }

    public void SetTargetResource(Resource resource)
    {
        _resource = resource;
    }
}