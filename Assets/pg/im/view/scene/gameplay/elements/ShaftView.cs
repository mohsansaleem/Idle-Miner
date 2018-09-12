using pg.im.model.data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace pg.im.view
{
    public class ShaftView : MonoBehaviour
    {
        [SerializeField]
        private CarrierView _carrierViewPrefab;

        [SerializeField]
        private Button _upgradeButton;
        [SerializeField]
        private TextMeshProUGUI _currentLevelText;
        [SerializeField]
        private TextMeshProUGUI _binCount;
        [SerializeField]
        private RectTransform _path;

        private List<CarrierView> _miners = new List<CarrierView>();

        private ShaftRemoteData _shaftRemoteData;
        public Action<ShaftRemoteData> OnShaftUpgradeClick;

        public ShaftRemoteData ShaftRemoteData
        {
            set
            {
                _shaftRemoteData = value;

                while (_shaftRemoteData.Miners.Count > _miners.Count)
                    CreateMinerView();

                for (int c = 0; c < _miners.Count; c++)
                {
                    _miners[c].LoadStuff(_shaftRemoteData.Miners[c].MinedCash, _shaftRemoteData.ShaftLevelData.WorkerCapacity);
                    SetMinerPosition(_miners[c], _shaftRemoteData.Miners[c]);
                }

                _binCount.text = _shaftRemoteData.BinCash.ToShort();

                _currentLevelText.text = _shaftRemoteData.ShaftLevel.ToString();
            }
        }

        private void SetMinerPosition(CarrierView carrierView, MinerRemoteData minerRemoteData)
        {
            float width = _path.rect.width;
            float unitDistance = width / Constants.MineLength;

            carrierView.RectTransform.anchoredPosition = new Vector2(minerRemoteData.CurrentLocation * unitDistance, 0f);
        }

        private void CreateMinerView()
        {
            _miners.Add(Instantiate(_carrierViewPrefab, _path));
        }
    }
}