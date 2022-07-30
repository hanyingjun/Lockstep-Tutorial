using Lockstep.Math;
using System;

namespace Lockstep.Game
{
    [Serializable]
    public partial class SpawnerInfo : INeedBackup
    {
        public LFloat spawnInternal = 0;
        public LVector3 spawnPoint;
        public int prefabId;
    }
}