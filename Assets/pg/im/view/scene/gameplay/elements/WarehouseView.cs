using pg.im.model.data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace pg.im.view
{
    public class WarehouseView : MonoBehaviour
    {
        [SerializeField]
        private CarrierView _carrierViewPrefab;

        [SerializeField]
        private Button _upgradeButton;
        [SerializeField]
        private TextMeshProUGUI _currentLevelText;
        [SerializeField]
        private RectTransform _path;

        private List<CarrierView> _transporters = new List<CarrierView>();

        private WarehouseRemoteData _warehouseRemoteData;

        public Action OnWarehouseUpgradeClick;

        public WarehouseRemoteData WarehouseRemoteData
        {
            set
            {
                _warehouseRemoteData = value;

                while (_warehouseRemoteData.Transporters.Count > _transporters.Count)
                    CreateTransporterView();

                for (int c = 0; c < _transporters.Count; c++)
                {
                    _transporters[c].LoadStuff(_warehouseRemoteData.Transporters[c].LoadedCash, _warehouseRemoteData.WarehouseLevelData.LoadPerTransporter);
                    SetTransporterPosition(_transporters[c], _warehouseRemoteData.Transporters[c]);
                }

                _currentLevelText.text = _warehouseRemoteData.WarehouseLevel.ToString();
            }
        }

        private void SetTransporterPosition(CarrierView carrierView, TransporterRemoteData transporterRemoteData)
        {
            float width = _path.rect.width;
            float unitDistance = width / Constants.WarehouseDistance;

            carrierView.RectTransform.anchoredPosition = new Vector2(width - transporterRemoteData.CurrentLocation * unitDistance, 0f);
        }

        private void CreateTransporterView()
        {
            _transporters.Add(Instantiate(_carrierViewPrefab, _path));
        }
    }
}