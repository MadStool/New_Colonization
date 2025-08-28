using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRayCast : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private BaseBuilder _currentBaseBuilder;
    private bool _isBuildingMode = false;

    private void OnDisable()
    {
        if (_currentBaseBuilder != null)
        {
            _currentBaseBuilder.BuildCompleted -= FinishPositioning;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleMouseClick();
    }

    private void FinishPositioning()
    {
        _isBuildingMode = false;
        _currentBaseBuilder = null;
    }

    private void HandleMouseClick()
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (_isBuildingMode == false && hit.transform.TryGetComponent(out Base baseComponent))
            {
                TryStartBuildingMode(baseComponent);
            }
            else if (_isBuildingMode && hit.transform.TryGetComponent(out SpawnResources spawnResources))
            {
                _currentBaseBuilder.PutUpFlag(hit.point);
            }
        }
    }

    private void TryStartBuildingMode(Base baseComponent)
    {
        int minBotsRequired = 2;

        if (baseComponent.BotsCount >= minBotsRequired)
        {
            BaseBuilder baseBuilder = baseComponent.GetComponent<BaseBuilder>();

            if (baseBuilder != null)
            {
                _isBuildingMode = true;
                _currentBaseBuilder = baseBuilder;
                _currentBaseBuilder.BuildCompleted += FinishPositioning;
                _currentBaseBuilder.Work();
            }
        }
        else
        {
            Debug.Log("Not enough bots to build! You need at least 2.");
        }
    }
}