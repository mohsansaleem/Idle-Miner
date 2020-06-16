using UniRx;

namespace PG.IdleMiner.Models.MediatorModels
{
    public class StartupModel
    {
        public enum ELoadingProgress
        {
            NotLoaded = -1,
            LoadPopup = 0,
            LoadStaticData = 20,
            LoadUserData = 30,
            CreateUserData = 50,
            LoadHud = 70,
            LoadGamePlay = 80,
            GamePlay = 100
        }

        public ReactiveProperty<ELoadingProgress> LoadingProgress;

        public StartupModel()
        {
            LoadingProgress = new ReactiveProperty<ELoadingProgress>(ELoadingProgress.LoadPopup);
        }
    }
}

