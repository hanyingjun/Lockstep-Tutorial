namespace vwengame.bephysics
{
    using BEPUphysics.CollisionShapes;
    using BEPUutilities;
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    public class PhyEntityMesh : PhyEntityBase
    {
        [SerializeField] private MeshFilter meshFilter = null;

        protected override UnityEngine.Vector3 GetGizmosSize()
        {
            return UnityEngine.Vector3.one;
        }

        protected override void DrawGizmos()
        {
            if (meshFilter == null)
                return;
            Gizmos.DrawWireMesh(meshFilter.sharedMesh, 0);
        }

        protected override void DoAwake()
        {
            base.DoAwake();
            meshFilter = this.GetComponent<MeshFilter>();
            AffineTransform affineTransform = new AffineTransform(transform.lossyScale, BEPUutilities.Quaternion.Identity, BEPUutilities.Vector3.Zero);
            BEPUutilities.Vector3[] vertices = new BEPUutilities.Vector3[meshFilter.mesh.vertices.Length];
            for (int i = 0, length = vertices.Length; i < length; i++)
            {
                vertices[i] = meshFilter.mesh.vertices[i];
            }

            entity = new BEPUphysics.Entities.Prefabs.MobileMesh(vertices, meshFilter.mesh.triangles, affineTransform, MobileMeshSolidity.DoubleSided, IsRigibody ? mass : 0);
        }
    }
}