using UnityEngine;

public class BEPUPhyMgr : MonoBehaviour
{
    public BEPUphysics.Space space;
    public static BEPUPhyMgr Instance = null;

    public void Awake()
    {
        if (BEPUPhyMgr.Instance != null)
        {
            return;
        }
        Physics.autoSimulation = false; // 关闭原来物理引擎迭代;
                                        // Physics.autoSyncTransforms = false; // 关闭射线检测功能
        BEPUPhyMgr.Instance = this; // 初始化单例
        this.space = new BEPUphysics.Space(); // 创建物理世界
        this.space.ForceUpdater.gravity = new BEPUutilities.Vector3(0, -9.81m, 0); // 配置重力
        this.space.TimeStepSettings.TimeStepDuration = 1 / 60m; // 设置迭代时间间隔
    }

    public void Update()
    {
        this.space.Update(); // 模拟迭代物理世界
    }
}