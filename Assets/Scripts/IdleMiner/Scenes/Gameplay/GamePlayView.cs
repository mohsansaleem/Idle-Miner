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

        public void UpdateShaft(ShaftRemoteData shaftRemoteData)
        {
            if (_shafts.ContainsKey(shaftRemoteData.ShaftId))
            {
                _shafts[shaftRemoteData.ShaftId].ShaftRemoteData = shaftRemoteData;
            }
            else
            {
                _shafts[shaftRemoteData.ShaftId] = Instantiate(ShaftViewPrefab, _shaftsContainer);
                _shafts[shaftRemoteData.ShaftId].ShaftRemoteData = shaftRemoteData;
            }
        }

        public void UpdateWarehouse(WarehouseRemoteData warehouseRemoteData)
        {
            _warehouseView.WarehouseRemoteData = warehouseRemoteData;
        }

        public void UpdateElevator(ElevatorRemoteData elevatorRemoteData, int height)
        {
            _elevatorView.ElevatorHeight = height;
            _elevatorView.SetElevatorRemoteData(elevatorRemoteData);
        }


        public Action OnElevatorUpgradeButtonClicked;
        public Action OnWareHouseUpgradeButtonClicked;

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

