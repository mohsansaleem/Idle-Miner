using UniRx;

namespace PG.IdleMiner.Models.MediatorModels
{
    public class StartupModel
    {
        public enum ELoadingProgress
        {
            NotLoaded = -1,
            Zero = 0,
            PopupLoaded = 20,
            StaticDataLoaded = 30,
            UserNotFound = 50,
            DataSeeded = 70,
            HudLoaded = 80,
            GamePlayLoaded = 100
        }

        public ReactiveProperty<ELoadingProgress> LoadingProgress;

        public StartupModel()
        {
            LoadingProgress = new ReactiveProperty<ELoadingProgress>(ELoadingProgress.Zero);
        }
    }
}

