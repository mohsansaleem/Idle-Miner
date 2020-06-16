using System;
using PG.Core.Contexts;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using PG.IdleMiner.Views.Hud;
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
            Disposables = new CompositeDisposable();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            StateBehaviours.Add((int)HudModel.EHudState.StartupScreen, new HudStateStartup(this));

            _remoteDataModel.IdleCash.Subscribe(OnIdleCashUpdate).AddTo(Disposables);
            _remoteDataModel.Cash.Subscribe(OnCashUpdate).AddTo(Disposables);
            _remoteDataModel.SuperCash.Subscribe(OnSuperCashUpdate).AddTo(Disposables);

            _hudModel.HudState.Subscribe(OnHudStateChanged).AddTo(Disposables);
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
            GoToState((int)hudState);
        }
    }
}

