using pg.im.model.data;
using System.Collections.Generic;

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

        public ShaftRemoteData UnlockShaft(int unlockedCount)
        {
            if (unlockedCount < MetaData.Shafts.Count)
            {
                ShaftRemoteData shaft = new ShaftRemoteData();

                // PATCH: MetaData is not designed properly.
                shaft.ShaftId = "S" + (unlockedCount + 1);

                ShaftLevelData data = MetaData.Shafts[shaft.ShaftId][0];

                shaft.ShaftLevel = 1;

                shaft.Miners = new List<MinerRemoteData>();
                for (int i = 0; i < data.Miners; i++)
                    shaft.Miners.Add(new MinerRemoteData());

                // PATCH: Remove this after adding Assign and Hire Managers functionality.
                shaft.Manager = "M1";


                return shaft;
            }
            return null;
        }
    }
}

