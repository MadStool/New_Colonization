using System;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class BaseCollector : MonoBehaviour
{
    public event Action<Bot> ResourceDelivered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bot bot))
        {
            Resource resource = bot.GetComponentInChildren<Resource>();

            if (resource != null)
            {
                bot.SubmitResource(resource);
                ResourceDelivered?.Invoke(bot);
            }
        }
    }
}