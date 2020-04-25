using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.DataModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PG.IdleMiner.Scenes.Gameplay
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
        private EMinerState _currentMinerState = EMinerState.Idle;
        float _width;
        float _unitDistance;
        Vector2 _mineEndPos;
        readonly Vector2 _mineStartPos = Vector2.zero;
        
        public Action<ShaftRemoteData> OnShaftUpgradeClick;

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(ShaftUpgradeButtonClicked);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(ShaftUpgradeButtonClicked);
        }

        private void ShaftUpgradeButtonClicked()
        {
            OnShaftUpgradeClick?.Invoke(_shaftRemoteData);
        }

        public void SetShaftRemoteData(ShaftRemoteData shaftRemoteData)
        {
            _shaftRemoteData = shaftRemoteData;

            _width = _path.rect.width;
            _unitDistance = _width / Constants.MineLength;
            _mineEndPos = new Vector2(_width, 0f);

            while (_shaftRemoteData.Miners.Count > _miners.Count)
            {
                CreateMinerView();
            }

            for (int c = 0; c < _miners.Count; c++)
            {
                _miners[c].LoadStuff(_shaftRemoteData.Miners[c].MinedCash,
                    _shaftRemoteData.ShaftLevelData.WorkerCapacity);
                SetMinerPosition(_miners[c], _shaftRemoteData.Miners[c]);
            }

            _binCount.text = _shaftRemoteData.BinCash.ToShort();

            _currentLevelText.text = _shaftRemoteData.ShaftLevel.ToString();
            
        }

        private void SetMinerPosition(CarrierView carrierView, MinerRemoteData minerRemoteData)
        {
            carrierView.RectTransform.anchoredPosition = new Vector2(_unitDistance * minerRemoteData.CurrentLocation, 0);
        }

        private void CreateMinerView()
        {
            _miners.Add(Instantiate(_carrierViewPrefab, _path));
        }
    }
}