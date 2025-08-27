using UnityEngine;
using TMPro;

public class BaseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _uiText;

    private Base _base;
    private BaseWallet _baseWallet;
    private BaseBuilder _baseBuilder;
    private Camera _mainCamera;

    private void Awake()
    {
        _base = GetComponent<Base>();
        _baseWallet = GetComponent<BaseWallet>();
        _baseBuilder = GetComponent<BaseBuilder>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateUI();
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        if (_uiText != null && _mainCamera != null)
            _uiText.transform.rotation = _mainCamera.transform.rotation;
    }

    private void UpdateUI()
    {
        if (_uiText == null)
        {
            Debug.Log("UIText is null!");
            return;
        }

        if (_base == null || _baseWallet == null)
        {
            return;
        }

        int botsCount = _base.BotsCount;
        int resourcesCount = _baseWallet.CurrentPoints;
        bool canBuild = botsCount > 1;

        string buildStatus = canBuild ? "You can build" : "You can't build yet";

        _uiText.text = $"Resources: {resourcesCount}\n" +
                      $"Bots: {botsCount}\n" +
                      $"{buildStatus}";
    }
}