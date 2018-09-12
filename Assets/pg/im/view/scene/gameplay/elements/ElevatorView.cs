using pg.im.model.data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace pg.im.view
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

        public ElevatorRemoteData ElevatorRemoteData
        {
            set
            {
                _elevatorRemoteData = value;

                SetElevatorPosition(_liftDriver, _elevatorRemoteData.CurrentLocation);
                _liftDriver.LoadStuff(_elevatorRemoteData.LoadedCash, _elevatorRemoteData.ElevatorLevelData.LoadCapacity);
                _storedCount.text = _elevatorRemoteData.StoredCash.ToShort();

                _currentLevelText.text = _elevatorRemoteData.ElevatorLevel.ToString();
            }
        }

        private void SetElevatorPosition(CarrierView carrierView, int location)
        {
            float height = _path.rect.height- carrierView.RectTransform.rect.height;
            float unitDistance = height / ElevatorHeight;

            carrierView.RectTransform.anchoredPosition = new Vector2(0f, height - location * unitDistance + carrierView.RectTransform.rect.height);
        }
    }
}