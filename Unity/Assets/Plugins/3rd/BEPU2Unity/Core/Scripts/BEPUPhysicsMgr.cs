namespace vwengame.bephysics
{
    using BEPUphysics;
    using System.Collections.Generic;
    using UnityEngine;

    public class BEPUPhysicsMgr : MonoBehaviour
    {
        private BEPUphysics.Space space = null;
        public static BEPUPhysicsMgr Instance = null;
        public PhyCharacterController controller = null;

        private bool _useLocalUpdate = true;
        /// <summary>
        /// 是否使用本地Update
        /// </summary>
        public bool UseLocalUpdate { get { return _useLocalUpdate; } set { _useLocalUpdate = value; } }

        static Dictionary<long, PhyEntityBase> entitys = new Dictionary<long, PhyEntityBase>();
        static Dictionary<long, ISpaceObject> awakeList = new Dictionary<long, ISpaceObject>();
        static Dictionary<long, ISpaceObject> destroyList = new Dictionary<long, ISpaceObject>();

        public void Reset()
        {
            entitys.Clear();
            awakeList.Clear();
            destroyList.Clear();
        }

        private void Awake()
        {
            Physics.autoSimulation = false; // 关闭原来物理引擎迭代;
            Physics.autoSyncTransforms = false; // 关闭射线检测功能
            BEPUPhysicsMgr.Instance = this; // 初始化单例
            Debug.LogError("222");
            this.space = new BEPUphysics.Space(); // 创建物理世界
            this.space.ForceUpdater.Gravity = new BEPUutilities.Vector3(0, -9.81m, 0); // 配置重力
            this.space.TimeStepSettings.TimeStepDuration = 1 / 60m; // 设置迭代时间间隔
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (controller != null)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    controller.controller.Jump();
                }

                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");
                controller.HorizontalMoveDir = new BEPUutilities.Vector2(h, v);
            }

            if (UseLocalUpdate)
                UpdateSpace();
        }

        public static void AddPyhsicsEntity(PhyEntityBase entityBase)
        {
            if (entitys.ContainsKey(entityBase.Id))
                return;
            entitys[entityBase.Id] = entityBase;
            ISpaceObject spaceObject = null;
            if (entityBase is PhyCharacterController)
            {
                PhyCharacterController controller = entityBase as PhyCharacterController;
                spaceObject = controller.controller;
            }
            else
            {
                spaceObject = entityBase.entity;
            }

            if (Instance == null)
            {
                awakeList.Add(entityBase.Id, spaceObject);
            }
            else
            {
                Instance.space.Add(spaceObject);
            }
        }

        public static void RemovePhysicsEntity(PhyEntityBase entityBase)
        {
            if (entitys.ContainsKey(entityBase.Id))
                entitys.Remove(entityBase.Id);

            ISpaceObject spaceObject = null;
            if (entityBase is PhyCharacterController)
            {
                PhyCharacterController controller = entityBase as PhyCharacterController;
                spaceObject = controller.controller;
            }
            else
            {
                spaceObject = entityBase.entity;
            }

            if (Instance == null)
            {
                destroyList.Add(entityBase.Id, spaceObject);
            }
            else
            {
                Instance.space.Remove(spaceObject);
            }
        }

        public void UpdateSpace()
        {
            if (this.space != null)
                this.space.Update(); // 模拟迭代物理世界
        }

        private void LateUpdate()
        {
            foreach (var item in awakeList)
            {
                space.Add(item.Value);
            }
            awakeList.Clear();
            foreach (var item in destroyList)
            {
                space.Remove(item.Value);
            }
            destroyList.Clear();

            foreach (var item in entitys)
            {
                item.Value.OnLateUpdate();
            }
        }
    }
}