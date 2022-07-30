using Lockstep.Math;
using NetMsg.Common;
using System.Collections.Generic;
using System.Linq;
using Debug = Lockstep.Logging.Debug;

namespace Lockstep.Game
{
    public class World : BaseSystem
    {
        public static World Instance { get; private set; }
        private int _tick = 0;
        public int Tick { get { return _tick; } }
        public PlayerInput[] PlayerInputs => _gameStateService.GetPlayers().Select(a => a.input).ToArray();
        public static Player MyPlayer;
        public static object MyPlayerTrans => MyPlayer?.engineTransform;
        private List<BaseSystem> _systems = new List<BaseSystem>();
        private bool _hasStart = false;

        public void RollbackTo(int tick, int maxContinueServerTick, bool isNeedClear = true)
        {
            if (tick < 0)
            {
                Debug.LogError("Target Tick invalid!" + tick);
                return;
            }

            Debug.Log($" Rollback diff:{Tick - tick} From{Tick}->{tick}  maxContinueServerTick:{maxContinueServerTick} {isNeedClear}");
            _timeMachineService.RollbackTo(tick);
            _commonStateService.SetTick(tick);
            _tick = tick;
        }

        // called by SimulatorService.OnGameCreate() 当所有人都准备好之后创建游戏地图
        public void StartSimulate(IServiceContainer serviceContainer, IManagerContainer mgrContainer)
        {
            Instance = this;
            RegisterSystems();
            if (!serviceContainer.GetService<IConstStateService>().IsVideoMode)
            {
                RegisterSystem(new TraceLogSystem());
            }

            InitReference(serviceContainer, mgrContainer);
            foreach (var mgr in _systems)
            {
                mgr.InitReference(serviceContainer, mgrContainer);
            }

            foreach (var mgr in _systems)
            {
                mgr.DoAwake(serviceContainer);
            }

            DoAwake(serviceContainer);
            foreach (var mgr in _systems)
            {
                mgr.DoStart();
            }

            DoStart();
        }

        // called by SimulatorService.StartSimulate(). At this time everything is readyed.
        public void StartGame(Msg_G2C_GameStartInfo gameStartInfo, int localPlayerId)
        {
            if (_hasStart) return;
            _hasStart = true;
            var playerInfos = gameStartInfo.UserInfos;
            var playerCount = playerInfos.Length;
            string _traceLogPath = "";
#if UNITY_STANDALONE_OSX
            _traceLogPath = $"/tmp/LPDemo/Dump_{localPlayerId}.txt";
#else
            _traceLogPath = $"c:/tmp/LPDemo/Dump_{localPlayerId}.txt";
#endif
            Debug.TraceSavePath = _traceLogPath;

            _debugService.Trace("CreatePlayer " + playerCount);
            //create Players 
            for (int i = 0; i < playerCount; i++)
            {
                var PrefabId = 0; //TODO
                var initPos = LVector2.zero; //TODO
                var player = _gameStateService.CreateEntity<Player>(PrefabId, initPos);
                player.localId = i;
            }

            var allPlayers = _gameStateService.GetPlayers();

            MyPlayer = allPlayers[localPlayerId];
        }

        public override void DoDestroy()
        {
            foreach (var mgr in _systems)
            {
                mgr.DoDestroy();
            }

            Debug.FlushTrace();
        }


        public override void OnApplicationQuit()
        {
            DoDestroy();
        }

        // called by SimulatorService.Step()
        public void Step(bool isNeedGenSnap = true)
        {
            if (_commonStateService.IsPause) return;
            var deltaTime = new LFloat(true, 30);
            foreach (var system in _systems)
            {
                if (system.enable)
                {
                    system.DoUpdate(deltaTime);
                }
            }

            _tick++;
        }

        public void RegisterSystems()
        {
            RegisterSystem(new HeroSystem());
            RegisterSystem(new EnemySystem());
            RegisterSystem(new PhysicSystem());
            RegisterSystem(new HashSystem());
        }

        public void RegisterSystem(BaseSystem mgr)
        {
            _systems.Add(mgr);
        }
    }
}