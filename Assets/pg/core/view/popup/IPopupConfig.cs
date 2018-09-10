using System.Collections.Generic;

namespace pg.core.view
{
    public interface IPopupConfig
    {
        string Title { get; set; }
        string Description { get; set; }
        List<IPopupButtonData> ButtonData { get; set; }
    }
}