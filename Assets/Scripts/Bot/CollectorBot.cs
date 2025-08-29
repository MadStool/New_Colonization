using System;
using UnityEngine;

public class CollectorBot : MonoBehaviour
{
    private Resource _targetResource;
    private Bot _bot;

    public event Action<Resource> ResourceCollected;

    private void Awake()
    {
        _bot = GetComponent<Bot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        float height = 0.5f;

        if (other.TryGetComponent(out Resource resource) && _targetResource == resource)
        {
            resource.transform.SetParent(transform);
            resource.transform.localPosition = new Vector3(0, height, 0);
            ResourceCollected?.Invoke(resource);
        }
    }

    public void SetTargetResource(Resource resource)
    {
        _targetResource = resource;
    }
}