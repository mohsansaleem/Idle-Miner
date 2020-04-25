using System;
using PG.Core.FSM;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using UniRx;
using Zenject;

namespace PG.IdleMiner.view
{
    public partial class HudMediator : StateMachineMediator
    {
        [Inject] private readonly HudView _view;

        [Inject] private readonly HudModel _hudModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

        public HudMediator()
        {
            _disposables = new CompositeDisposable();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _stateBehaviours.Add(typeof(HudStateStartup), new HudStateStartup(this));

            _remoteDataModel.IdleCash.Subscribe(OnIdleCashUpdate).AddTo(_disposables);
            _remoteDataModel.Cash.Subscribe(OnCashUpdate).AddTo(_disposables);
            _remoteDataModel.SuperCash.Subscribe(OnSuperCashUpdate).AddTo(_disposables);

            _hudModel.HudState.Subscribe(OnHudStateChanged).AddTo(_disposables);
        }

        private void OnIdleCashUpdate(double idleCash)
        {
            _view._idleCashWidget.SetData(idleCash, idleCash);
        }

        private void OnCashUpdate(double cash)
        {
            _view._cashWidget.SetData(cash, cash);
        }

        private void OnSuperCashUpdate(double superCash)
        {
            _view._superCashWidget.SetData(superCash, superCash);
        }

        private void OnHudStateChanged(HudModel.EHudState hudState)
        {
            Type targetType = null;
            switch (hudState)
            {
                case HudModel.EHudState.StartupScreen:
                    targetType = typeof(HudStateStartup);
                    break;
            }

            if (targetType != null &&
                (_currentStateBehaviour == null ||
                 targetType != _currentStateBehaviour.GetType()))
            {
                GoToState(targetType);
            }
        }
    }
}

