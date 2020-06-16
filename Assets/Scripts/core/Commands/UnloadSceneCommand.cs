using PG.Core.Installers;
using Zenject;

namespace PG.Core.Commands
{
    public class UnloadSceneCommand : BaseCommand
    {
        [Inject] private readonly ISceneLoader _sceneLoader;

        public void Execute(UnloadSceneSignal loadParams)
        {
            _sceneLoader.UnloadScene (loadParams.Scene).Done (
                () =>
                {
                    loadParams.OnComplete?.Resolve();
                },
                exception =>
                {
                    loadParams.OnComplete?.Reject(exception);
                }
            );
        }
    }
}
