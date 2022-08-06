using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUutilities;
using FixMath.NET;

namespace BEPUphysics.CollisionShapes
{
    public struct EntityShapeVolumeDescription
    {
        public Matrix3x3 VolumeDistribution;
        /// <summary>
        /// 体积
        /// </summary>
        public Fix64 Volume;
    }
}
