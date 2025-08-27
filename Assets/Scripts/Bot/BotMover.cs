using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Coroutine _currentMovement;

    public IEnumerator MoveTo(Vector3 targetPosition)
    {
        if (_currentMovement != null)
        {
            StopCoroutine(_currentMovement);
        }

        _currentMovement = StartCoroutine(MoveToCoroutine(targetPosition));
        yield return _currentMovement;
    }

    private IEnumerator MoveToCoroutine(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.forward = targetPosition - transform.position;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _speed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;
        _currentMovement = null;
    }

    public void SetTargetPosition(Vector3 position)
    {
        StartCoroutine(MoveTo(position));
    }
}