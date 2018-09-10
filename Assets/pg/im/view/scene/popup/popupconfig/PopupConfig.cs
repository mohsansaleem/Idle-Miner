using System;
using System.Collections.Generic;
using pg.core.view;

namespace pg.im.view.popup.popupconfig
{
    public abstract class PopupConfig : IPopupConfig
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IPopupButtonData> ButtonData { get; set; }


        protected static IPopupConfig PopulatedConfigInstance(IPopupConfig popupConfig, string title, string description, params string[] buttonsTexts)
        {
            popupConfig.Title = title;
            popupConfig.Description = description;
            popupConfig.ButtonData = new List<IPopupButtonData>();

            if (buttonsTexts != null)
            {
                foreach (string buttonText in buttonsTexts)
                {
                    popupConfig.ButtonData.Add(new PopupButtonData(buttonText));
                }
            }

            return popupConfig;
        }

        public abstract IPopupResult GetPopupResult();
    }
}