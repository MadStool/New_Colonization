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

    private void OnEnable()
    {
        // Подписываемся на события изменения ресурсов
        if (_baseWallet != null)
        {
            // Нужно добавить событие в BaseWallet
            _baseWallet.PointsChanged += OnPointsChanged;
        }

        // Подписываемся на события изменения количества ботов
        if (_base != null)
        {
            // Нужно добавить событие в Base
            _base.BotsCountChanged += OnBotsCountChanged;
        }

        // Подписываемся на события строительства
        if (_baseBuilder != null)
        {
            _baseBuilder.BuildStarted += OnBuildStateChanged;
            _baseBuilder.BuildCompleted += OnBuildStateChanged;
        }

        // Первоначальное обновление
        UpdateUI();
    }

    private void OnDisable()
    {
        if (_baseWallet != null)
            _baseWallet.PointsChanged -= OnPointsChanged;

        if (_base != null)
            _base.BotsCountChanged -= OnBotsCountChanged;

        if (_baseBuilder != null)
        {
            _baseBuilder.BuildStarted -= OnBuildStateChanged;
            _baseBuilder.BuildCompleted -= OnBuildStateChanged;
        }
    }

    private void OnPointsChanged(int newPoints)
    {
        UpdateUI();
    }

    private void OnBotsCountChanged(int newCount)
    {
        UpdateUI();
    }

    private void OnBuildStateChanged()
    {
        UpdateUI();
    }

    private void LateUpdate()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        if (_uiText != null && _mainCamera != null)
            _uiText.transform.rotation = _mainCamera.transform.rotation;
    }

    private void UpdateUI()
    {
        if (_uiText == null || _base == null || _baseWallet == null)
            return;

        int botsCount = _base.BotsCount;
        int resourcesCount = _baseWallet.CurrentPoints;
        bool canBuild = botsCount > 1;

        string buildStatus = canBuild ? "You can build" : "You can't build yet";

        _uiText.text = $"Resources: {resourcesCount}\n" +
                      $"Bots: {botsCount}\n" +
                      $"{buildStatus}";
    }
}