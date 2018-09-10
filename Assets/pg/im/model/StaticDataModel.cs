using Newtonsoft.Json;
using pg.im.model.data;
using pg.im.view.popup;
using UniRx;
using UnityEngine;

namespace pg.im.model
{
    public class StaticDataModel
    {
        private MetaData _metaData;

        public void SeedMetaData(MetaData metaData)
        {
            Debug.LogError("MetaData: " + JsonConvert.SerializeObject(metaData));
            _metaData = metaData;
        }
    }
}

