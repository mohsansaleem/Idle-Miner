using System;
using System.Collections.Generic;
using PG.IdleMiner.Models.DataModels;
using UnityEngine;
using UnityEngine.UI;

namespace PG.IdleMiner.Scenes.Gameplay
 {
    public class GamePlayView : MonoBehaviour
    {
        [SerializeField]
        private CarrierView CerrierViewPrefab;
        [SerializeField]
        private ShaftView ShaftViewPrefab;

        [SerializeField]
        private ElevatorView _elevatorView;
        [SerializeField]
        private WarehouseView _warehouseView;

        [SerializeField]
        private RectTransform _shaftsContainer;

        [SerializeField]
        public Button AddShaftButton;

        private Dictionary<string, ShaftView> _shafts = new Dictionary<string, ShaftView>();

        public ShaftView AddShaft(ShaftRemoteData shaftRemoteData)
        {
            ShaftView shaftView;
            if (_shafts.ContainsKey(shaftRemoteData.ShaftId))
            {
                shaftView = _shafts[shaftRemoteData.ShaftId];
                shaftView.SetShaftRemoteData(shaftRemoteData);
            }
            else
            {
                shaftView = Instantiate(ShaftViewPrefab, _shaftsContainer);
                _shafts[shaftRemoteData.ShaftId] = shaftView;
                shaftView.SetShaftRemoteData(shaftRemoteData);
            }

            return shaftView;
        }
        
        public void UpdateShaft(ShaftRemoteData shaftRemoteData)
        {
            if (_shafts.ContainsKey(shaftRemoteData.ShaftId))
            {
                var shaft = _shafts[shaftRemoteData.ShaftId];
                shaft.SetShaftRemoteData(shaftRemoteData);
            }
            else
            {
                Debug.LogError("Shaft missing.");
            }
        }

        public void SubscribeUpgradeWareHouse(Action action)
        {
            _warehouseView.OnWarehouseUpgradeClick += action;
        }
        
        public void SubscribeUpgradeElevator(Action action)
        {
            _elevatorView.OnElevatorUpgradeClick += action;
        }
        
        public void UpdateWarehouse(WarehouseRemoteData warehouseRemoteData)
        {
            _warehouseView.WarehouseRemoteData = warehouseRemoteData;
        }

        public void UnSubscribeUpgradeWareHouse(Action action)
        {
            _warehouseView.OnWarehouseUpgradeClick -= action;
        }
        
        public void UnSubscribeElevatorHouse(Action action)
        {
            _elevatorView.OnElevatorUpgradeClick -= action;
        }
        
        public void UpdateElevator(ElevatorRemoteData elevatorRemoteData, int height)
        {
            _elevatorView.ElevatorHeight = height;
            _elevatorView.SetElevatorRemoteData(elevatorRemoteData);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

