using pg.core.installer;
using Zenject;

namespace pg.core.command
{
    public class LoadSceneCommand : BaseCommand
    {
        [Inject] private readonly ISceneLoader _sceneLoader;

        public void Execute(LoadSceneCommandParams loadParams)
        {
            _sceneLoader.LoadScene (loadParams.Scene).Done(
                () =>
                {
                    if (loadParams.OnComplete != null)
                    {
                        loadParams.OnComplete.Resolve();
                    }
                },
                exception =>
                {
                    if (loadParams.OnComplete != null)
                    {
                        loadParams.OnComplete.Reject(exception);
                    }
                }
            );
        }
    }
}
