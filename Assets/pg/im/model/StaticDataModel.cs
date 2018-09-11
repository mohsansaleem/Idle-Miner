using pg.im.model.data;

namespace pg.im.model
{
    public class StaticDataModel
    {
        public MetaData MetaData;

        public void SeedMetaData(MetaData metaData)
        {
            MetaData = metaData;
        }

        public ShaftLevelData GetShaftLevelData(string shaftId, int level)
        {
            if (MetaData.Shafts.ContainsKey(shaftId))
            {
                if (level <= MetaData.Shafts[shaftId].Count)
                {
                    return MetaData.Shafts[shaftId][level - 1];
                }
            }

            return null;
        }

        internal ElevatorLevelData GetElevatorLevelData(int elevatorLevel)
        {
            if (elevatorLevel <= MetaData.Elevator.Count)
            {
                return MetaData.Elevator[elevatorLevel - 1];
            }
            return null;
        }

        internal WarehouseLevelData GetWarehouseLevelData(int warehouseLevel)
        {
            if (warehouseLevel <= MetaData.Warehouse.Count)
            {
                return MetaData.Warehouse[warehouseLevel - 1];
            }
            return null;
        }
    }
}

