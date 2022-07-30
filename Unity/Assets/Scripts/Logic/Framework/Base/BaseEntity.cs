using Lockstep.Collision2D;
using Lockstep.Math;
using System;
using System.Collections.Generic;
using Debug = Lockstep.Logging.Debug;

namespace Lockstep.Game
{
    [Serializable]
    [NoBackup]
    public partial class BaseEntity : BaseLifeCycle, IEntity, ILPTriggerEventHandler
    {
        [Backup]
        public int EntityId;
        [Backup]
        public int PrefabId;
        [Backup]
        public CTransform2D transform
        {
            get { return this.GetComponent<CTransform2D>(); }
        }
        [NoBackup]
        public object engineTransform;

        [NonSerialized]
        private Dictionary<Type, bool> allComponentKeys = null;
        [NonSerialized]
        private List<BaseComponent> allComponents = null;
        [NonSerialized]
        private Dictionary<Type, IComponent> _allComponents = null;

        [ReRefBackup] public IGameStateService GameStateService { get; set; }
        [ReRefBackup] public IServiceContainer ServiceContainer { get; set; }
        [ReRefBackup] public IDebugService DebugService { get; set; }

        [ReRefBackup] public IEntityView EntityView;

        #region Impletetion BaseLiseCycle

        public override void DoAwake()
        {
            for (int i = 0, length = allComponents.Count; i < length; i++)
            {
                allComponents[i].DoAwake();
            }
        }

        public override void DoStart()
        {
            for (int i = 0, length = allComponents.Count; i < length; i++)
            {
                allComponents[i].DoStart();
            }
        }

        public override void DoUpdate(LFloat deltaTime)
        {
            for (int i = 0, length = allComponents.Count; i < length; i++)
            {
                allComponents[i].DoUpdate(deltaTime);
            }
        }

        public override void DoDestroy()
        {
            for (int i = 0, length = allComponents.Count; i < length; i++)
            {
                allComponents[i].DoDestroy();
            }
        }

        #endregion

        public T GetService<T>() where T : IService
        {
            return ServiceContainer.GetService<T>();
        }

        public void AddComponent<T>() where T : IComponent
        {
            T com = Activator.CreateInstance<T>();
            this.AddComponent(com);
        }

        public void AddComponent(IComponent comp)
        {
            Type type = comp.GetType();
            if (comp is BaseComponent)
            {
                if (_allComponents.ContainsKey(type))
                {
                    Debug.LogError(type.Name + " component has add.");
                }
                else
                {
                    BaseComponent com = comp as BaseComponent;
                    allComponents.Add(com);
                }
            }

            if (_allComponents.ContainsKey(type))
            {
                Debug.LogError(type.Name + " component has add.");
            }
            else
            {
                _allComponents.Add(type, comp);
            }
        }

        public void Initialize()
        {
            if (allComponentKeys != null)
                allComponentKeys.Clear();
            else
                allComponentKeys = new Dictionary<Type, bool>();
            if (allComponents != null)
                allComponents.Clear();
            else
                allComponents = new List<BaseComponent>();

            if (_allComponents != null)
                _allComponents.Clear();
            else
                _allComponents = new Dictionary<Type, IComponent>();
            this.DoInitialize();
        }

        protected virtual void DoInitialize()
        {
            this.AddComponent(new CTransform2D());
        }

        /// <summary>
        /// 填充组件配置信息，对于backup的不需要执行
        /// </summary>
        public void FillComponentsConfig()
        {
            DoFillComponentsConfig();
        }

        protected virtual void DoFillComponentsConfig() { }

        public void DoBindRef()
        {
            for (int i = 0, length = allComponents.Count; i < length; i++)
            {
                allComponents[i].BindEntity(this);
            }
        }

        public T GetComponent<T>() where T : IComponent
        {
            Type t = typeof(T);
            if (_allComponents == null || !_allComponents.ContainsKey(t))
                return default(T);
            return (T)_allComponents[t];
        }

        public virtual void OnRollbackDestroy()
        {
            if (EntityView != null)
                EntityView.OnRollbackDestroy();
            EntityView = null;
            engineTransform = null;
        }

        public virtual void OnLPTriggerEnter(ColliderProxy other) { }

        public virtual void OnLPTriggerStay(ColliderProxy other) { }

        public virtual void OnLPTriggerExit(ColliderProxy other) { }
    }
}