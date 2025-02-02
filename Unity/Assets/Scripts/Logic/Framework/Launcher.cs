using Lockstep.Math;
using Lockstep.Network;
using Lockstep.Util;
using NetMsg.Common;
using System;
using System.Threading;
using Debug = UnityEngine.Debug;

namespace Lockstep.Game
{
    /*
     *          ServiceReferenceHolder     IService     ILifeCycle      ITimeMachine        IHashCode       IDumpStr
     *                  |                    |                |               |                 |                 | 
     *                  ---------------------^--------------------------------^-----------------------------------
     *                                    |  |                                ^---------------------------------<
     *                                    |  ^-----------------------<------------------------<----------------<|    
     *                                    |                          |                        |                 |       
     *                              BaseService             IEventRegisterService       IGameViewService   ITimeMachineService
     *            ----------------------|                            |
     *            |             |                                    |
     *      BaseGameService  NetworkService                 EventRegisterService
     *            |
     *      SimulatorService
     */
    [Serializable]
    public class Launcher : ILifeCycle
    {
        public int CurTick { get { return _serviceContainer.GetService<ICommonStateService>().Tick; } }

        public static Launcher Instance { get; private set; }

        private ServiceContainer _serviceContainer;
        private ManagerContainer _mgrContainer;
        private TimeMachineContainer _timeMachineContainer;

        public string RecordPath;
        public int MaxRunTick = int.MaxValue;
        public Msg_G2C_GameStartInfo GameStartInfo;
        public Msg_RepMissFrame FramesInfo;

        public int JumpToTick = 10;

        private IEventRegisterService _registerService;
        private SimulatorService _simulatorService = null;
        private NetworkService _networkService = null;
        private IConstStateService _constStateService = null;

        public bool IsRunVideo { get { return _constStateService.IsRunVideo; } }
        public bool IsVideoMode { get { return _constStateService.IsVideoMode; } }
        public bool IsClientMode { get { return _constStateService.IsClientMode; } }

        private OneThreadSynchronizationContext _syncContext;

        public void DoAwake(IServiceContainer services)
        {
            _syncContext = new OneThreadSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(_syncContext);
            Utils.StartServices();
            if (Instance != null)
            {
                Debug.LogError("LifeCycle Error: Awake more than once!!");
                return;
            }

            Instance = this;
            _serviceContainer = services as ServiceContainer;
            _registerService = new EventRegisterService();
            _mgrContainer = new ManagerContainer();
            _timeMachineContainer = new TimeMachineContainer();

            //AutoCreateManagers;
            var svcs = _serviceContainer.GetAllServices();
            foreach (var service in svcs)
            {
                _timeMachineContainer.RegisterTimeMachine(service as ITimeMachine);
                if (service is BaseService baseService)
                {
                    _mgrContainer.RegisterManager(baseService);
                }
            }

            _serviceContainer.RegisterService(_timeMachineContainer);
            _serviceContainer.RegisterService(_registerService);
        }

        public void DoStart()
        {
            foreach (var mgr in _mgrContainer.AllMgrs)
            {
                mgr.InitReference(_serviceContainer, _mgrContainer);
            }

            //bind events
            foreach (var mgr in _mgrContainer.AllMgrs)
            {
                _registerService.RegisterEvent<EEvent, GlobalEventHandler>("OnEvent_", "OnEvent_".Length,
                    EventHelper.AddListener, mgr);
            }

            foreach (var mgr in _mgrContainer.AllMgrs)
            {
                mgr.DoAwake(_serviceContainer);
            }

            _DoAwake(_serviceContainer);

            foreach (var mgr in _mgrContainer.AllMgrs)
            {
                mgr.DoStart();
            }

            _DoStart();
        }

        public void _DoAwake(IServiceContainer serviceContainer)
        {
            _simulatorService = serviceContainer.GetService<ISimulatorService>() as SimulatorService;
            _networkService = serviceContainer.GetService<INetworkService>() as NetworkService;
            _constStateService = serviceContainer.GetService<IConstStateService>();

            if (IsVideoMode)
            {
                _constStateService.SnapshotFrameInterval = 20;
                //OpenRecordFile(RecordPath);
            }
        }

        public void _DoStart()
        {
            //_debugService.Trace("Before StartGame _IdCounter" + BaseEntity.IdCounter);
            //if (!IsReplay && !IsClientMode) {
            //    netClient = new NetClient();
            //    netClient.Start();
            //    netClient.Send(new Msg_JoinRoom() {name = Application.dataPath});
            //}
            //else {
            //    StartGame(0, playerServerInfos, localPlayerId);
            //}

            if (IsVideoMode)
            {
                EventHelper.Trigger(EEvent.BorderVideoFrame, FramesInfo);
                EventHelper.Trigger(EEvent.OnGameCreate, GameStartInfo);
            }
            else if (IsClientMode)
            {
                GameStartInfo = _serviceContainer.GetService<IGameConfigService>().ClientModeInfo;
                EventHelper.Trigger(EEvent.OnGameCreate, GameStartInfo);
                EventHelper.Trigger(EEvent.LevelLoadDone, GameStartInfo);
            }
        }

        public void DoUpdate(float fDeltaTime)
        {
            _syncContext.Update();
            Utils.UpdateServices();
            var deltaTime = fDeltaTime.ToLFloat();
            _networkService.DoUpdate(deltaTime);
            if (IsVideoMode && IsRunVideo && CurTick < MaxRunTick)
            {
                _simulatorService.RunVideo();
                return;
            }

            if (IsVideoMode && !IsRunVideo)
            {
                _simulatorService.JumpTo(JumpToTick);
            }

            _simulatorService.DoUpdate(fDeltaTime);
        }

        public void DoDestroy()
        {
            if (Instance == null) return;
            foreach (var mgr in _mgrContainer.AllMgrs)
            {
                mgr.DoDestroy();
            }

            Instance = null;
        }

        public void OnApplicationQuit()
        {
            DoDestroy();
        }
    }
}