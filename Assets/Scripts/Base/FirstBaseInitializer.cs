using UnityEngine;

public class FirstBaseInitializer : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private SpawnerBase _baseSpawner;

    private Base _baseComponent;

    private void Awake()
    {
        _baseComponent = GetComponent<Base>();

        if (_baseComponent != null && _scanner != null && _baseSpawner != null)
        {
            _baseComponent.BaseCreationRequested += _baseSpawner.HandleBaseCreationRequest;
            _baseComponent.Initialize(_scanner);
        }
    }

    private void OnDestroy()
    {
        if (_baseComponent != null && _baseSpawner != null)
        {
            _baseComponent.BaseCreationRequested -= _baseSpawner.HandleBaseCreationRequest;
        }
    }

    private void Start()
    {
        UI uiComponent = GetComponent<UI>();

        if (uiComponent != null)
        {
            Invoke(nameof(ForceUIUpdate), 0.1f);
        }
    }

    private void ForceUIUpdate()
    {
        UI uiComponent = GetComponent<UI>();

        if (uiComponent != null)
        {
            uiComponent.UpdateUI();
        }
    }
}