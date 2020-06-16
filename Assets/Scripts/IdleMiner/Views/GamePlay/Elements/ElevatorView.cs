using System;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.DataModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.IdleMiner.Views.GamePlay
{
    public class ElevatorView : MonoBehaviour
    {
        [SerializeField]
        private CarrierView _liftDriver;
        [SerializeField]
        private Button _upgradeButton;
        [SerializeField]
        private TextMeshProUGUI _currentLevelText;
        [SerializeField]
        private TextMeshProUGUI _storedCount;
        [SerializeField]
        private RectTransform _path;

        public Action OnElevatorUpgradeClick;

        private ElevatorRemoteData _elevatorRemoteData;

        public int ElevatorHeight;

        // Locals
        private Vector2 _targetPosition;

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
        }

        private void OnUpgradeButtonClicked()
        {
            OnElevatorUpgradeClick?.Invoke();
        }

        public void SetElevatorRemoteData(ElevatorRemoteData elevatorRemoteData)
        {
            _elevatorRemoteData = elevatorRemoteData;

            SetElevatorPosition(_liftDriver, _elevatorRemoteData.CurrentLocation);
            _liftDriver.LoadStuff(_elevatorRemoteData.LoadedCash, _elevatorRemoteData.ElevatorLevelData.LoadCapacity);
            _storedCount.text = (_elevatorRemoteData.StoredCash.ToShort()).ToString();

            _currentLevelText.text = _elevatorRemoteData.ElevatorLevel.ToString();
        }

        private void SetElevatorPosition(CarrierView carrierView, float location)
        {
            float height = _path.rect.height - carrierView.RectTransform.rect.height;
            float unitDistance = height / ElevatorHeight;
            var targetPos = new Vector2(0f, height - location * unitDistance + carrierView.RectTransform.rect.height);

            carrierView.RectTransform.anchoredPosition = targetPos;
        }
    }
}