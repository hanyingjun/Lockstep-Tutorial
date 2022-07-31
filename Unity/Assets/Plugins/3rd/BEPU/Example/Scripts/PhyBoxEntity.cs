using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PhyBoxEntity : MonoBehaviour
{
    public int mass = 1;
    BEPUphysics.Entities.Prefabs.Box box;

    public bool isStatic = false;

    private float width = 1;
    private float height = 1;
    private float length = 1;

    private float centerX = 0, centerY = 0, centerZ = 0;

    void Start()
    {
        BoxCollider boxPhy = this.GetComponent<BoxCollider>();
        this.width = boxPhy.size.x;
        this.height = boxPhy.size.y;
        this.length = boxPhy.size.z;

        this.centerX = boxPhy.center.x;
        this.centerY = boxPhy.center.y;
        this.centerZ = boxPhy.center.z;


        Vector3 pos = this.transform.position;
        pos.x += centerX;
        pos.y += centerY;
        pos.z += centerZ;
        if (isStatic)
        {
            this.box = new BEPUphysics.Entities.Prefabs.Box(new BEPUutilities.Vector3(0, 0, 0), System.Convert.ToDecimal(this.width), System.Convert.ToDecimal(this.height), System.Convert.ToDecimal(this.length));
        }
        else
        {
            this.box = new BEPUphysics.Entities.Prefabs.Box(new BEPUutilities.Vector3(0, 0, 0), System.Convert.ToDecimal(this.width), System.Convert.ToDecimal(this.height), System.Convert.ToDecimal(this.length), mass);
        }

        BEPUPhyMgr.Instance.space.Add(box);
    }

    public void LateUpdate()
    {
        if (this.isStatic)
        {
            return;
        }
        //  FixMath.NET.Fix64 g = 9.81m;
        // Debug.Log(g.ToString());

        BEPUutilities.Vector3 worldPos = this.box.position;
        // Debug.Log(worldPos.ToString());

        double x = System.Convert.ToDouble((decimal)worldPos.X);
        double y = System.Convert.ToDouble((decimal)worldPos.Y);
        double z = System.Convert.ToDouble((decimal)worldPos.Z);


        this.transform.position = new Vector3((float)x - this.centerX, (float)y - this.centerY, (float)z - this.centerY);
    }
}
