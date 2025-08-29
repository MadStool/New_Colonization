using System;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class CollectorBase : MonoBehaviour
{
    public event Action<Bot> ResourceDelivered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bot bot))
        {
            if (bot.CurrentResource != null)
            {
                bot.SubmitResource(bot.CurrentResource);
                ResourceDelivered?.Invoke(bot);
            }
        }
    }
}