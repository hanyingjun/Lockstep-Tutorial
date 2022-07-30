#define OPEN_DEBUG_SKILL
#if OPEN_DEBUG_SKILL && UNITY_EDITOR
#define DEBUG_SKILL
#endif
using System;
using System.Collections.Generic;
using Lockstep.Collision2D;
using Lockstep.Math;
#if UNITY_EDITOR
using UnityEngine;
#endif


public enum ESkillState
{
    Idle,
    Firing,
}

namespace Lockstep.Game
{
    public interface ISkillEventHandler
    {
        void OnSkillStart(Skill skill);
        void OnSkillDone(Skill skill);
        void OnSkillPartStart(Skill skill);
    }

    [Serializable]
    public partial class Skill : INeedBackup
    {
        private static readonly HashSet<Entity> _tempEntities = new HashSet<Entity>();
        [NoBackup]
        public ISkillEventHandler eventHandler;
        [NoBackup]
        public Entity entity { get; private set; }
        [NoBackup]
        public SkillInfo SkillInfo { get; private set; }

        [Backup]
        public LFloat CdTimer;
        [Backup]
        public ESkillState State;
        [Backup]
        public LFloat skillTimer;
        public int[] partCounter = new int[0];
        [Backup]
        private int _curPartIdx;

        [NoBackup]
        public SkillPart CurPart
        {
            get
            {
                if (_curPartIdx == -1)
                    return null;
                return Parts[_curPartIdx];
            }
        }
#if DEBUG_SKILL
        private float _showTimer;
#endif
        [NoBackup]
        public LFloat DoneDelay { get { return SkillInfo.doneDelay; } }
        [NoBackup]
        public List<SkillPart> Parts { get { return SkillInfo.parts; } }
        [NoBackup]
        public int TargetLayer { get { return SkillInfo.targetLayer; } }
        [NoBackup]
        public LFloat MaxPartTime { get { return SkillInfo.maxPartTime; } }
        [NoBackup]
        public string AnimName { get { return SkillInfo.animName; } }

        public void ForceStop() { }

        #region 第一次绑定
        public void BindEntity(Entity entity, SkillInfo info, ISkillEventHandler eventHandler)
        {
            this.entity = entity;
            this.SkillInfo = info;
            this.eventHandler = eventHandler;
        }

        public void DoStart()
        {
            skillTimer = MaxPartTime;
            State = ESkillState.Idle;
            _curPartIdx = -1;
            partCounter = new int[Parts.Count];
        }
        #endregion
        #region Backup 绑定
        public void RebindEntity(Entity entity, SkillInfo info, ISkillEventHandler eventHandler)
        {
            this.entity = entity;
            this.SkillInfo = info;
            this.eventHandler = eventHandler;
        }
        #endregion

        public bool Fire()
        {
            if (CdTimer <= 0 && State == ESkillState.Idle)
            {
                CdTimer = SkillInfo.CD;
                skillTimer = LFloat.zero;
                for (int i = 0; i < partCounter.Length; i++)
                {
                    partCounter[i] = 0;
                }

                State = ESkillState.Firing;
                entity.animator.Play(AnimName);
                ((Player)entity).mover.needMove = false;
                OnFire();
                return true;
            }

            return false;
        }

        public void OnFire()
        {
            eventHandler.OnSkillStart(this);
        }

        public void Done()
        {
            eventHandler.OnSkillDone(this);
            State = ESkillState.Idle;
            entity.animator?.Play(AnimDefine.Idle);
        }

        public void DoUpdate(LFloat deltaTime)
        {
            CdTimer -= deltaTime;
            skillTimer += deltaTime;
            if (skillTimer < MaxPartTime)
            {
                for (int i = 0; i < Parts.Count; i++)
                {
                    var part = Parts[i];
                    CheckSkillPart(part, i);
                }

                if (CurPart != null && CurPart.moveSpd != 0)
                {
                    entity.transform.pos += CurPart.moveSpd * deltaTime * entity.transform.forward;
                }
            }
            else
            {
                _curPartIdx = -1;
                if (State == ESkillState.Firing)
                {
                    Done();
                }
            }
        }

        void CheckSkillPart(SkillPart part, int idx)
        {
            if (partCounter[idx] > part.otherCount)
                return;
            if (skillTimer > part.NextTriggerTimer(partCounter[idx]))
            {
                TriggerPart(part, idx);
                partCounter[idx]++;
            }
        }

        void TriggerPart(SkillPart part, int idx)
        {
            eventHandler.OnSkillPartStart(this);
            _curPartIdx = idx;
#if DEBUG_SKILL
            _showTimer = Time.realtimeSinceStartup + 0.1f;
#endif

            var col = part.collider;
#if NO_DEBUG_NO_COLLISION
            if (col.radius > 0) {
                //circle
                PhysicSystem.QueryRegion(TargetLayer, entity.transform.TransformPoint(col.pos), col.radius,
                    _OnTriggerEnter);
            }
            else {
                //aabb
                PhysicSystem.QueryRegion(TargetLayer, entity.transform.TransformPoint(col.pos), col.size,
                    entity.transform.forward,
                    _OnTriggerEnter);
            }

#else
            //TODO Ignore CollisionSystem
            if (col.radius > 0)
            {
                var colPos = entity.transform.TransformPoint(col.pos);
                foreach (var e in entity.GameStateService.GetEnemies())
                {
                    var targetCenter = e.transform.pos;
                    if ((targetCenter - colPos).sqrMagnitude < col.radius * col.radius)
                    {
                        _tempEntities.Add(e);
                    }
                }
            }
#endif
            foreach (var other in _tempEntities)
            {
                other.TakeDamage(entity, CurPart.damage, other.transform.pos.ToLVector3());
            }

            //add force
            if (part.needForce)
            {
                var force = part.impulseForce;
                var forward = entity.transform.forward;
                var right = forward.RightVec();
                var z = forward * force.z + right * force.x;
                force.x = z.x;
                force.z = z.y;
                foreach (var other in _tempEntities)
                {
                    other.rigidbody.AddImpulse(force);
                }
            }

            if (part.isResetForce)
            {
                foreach (var other in _tempEntities)
                {
                    other.rigidbody.ResetSpeed(new LFloat(3));
                }
            }

            _tempEntities.Clear();
        }

        //private static readonly HashSet<Entity> _tempEntities = new HashSet<Entity>();
        private void _OnTriggerEnter(ColliderProxy other)
        {
            if (CurPart.collider.IsCircle && CurPart.collider.deg > 0)
            {
                var deg = (other.Transform2D.pos - entity.transform.pos).ToDeg();
                var degDiff = entity.transform.deg.Abs() - deg;
                if (LMath.Abs(degDiff) <= CurPart.collider.deg)
                {
                    _tempEntities.Add(other.Entity);
                }
            }
            else
            {
                _tempEntities.Add(other.Entity);
            }
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR && DEBUG_SKILL
            float tintVal = 0.3f;
            Gizmos.color = new Color(0, 1.0f - tintVal, tintVal, 0.25f);
            if (Application.isPlaying)
            {
                if (entity == null) return;
                if (CurPart == null) return;
                if (_showTimer < Time.realtimeSinceStartup)
                {
                    return;
                }

                ShowPartGizmons(CurPart);
            }
            else
            {
                foreach (var part in Parts)
                {
                    if (part._DebugShow)
                    {
                        ShowPartGizmons(part);
                    }
                }
            }

            Gizmos.color = Color.white;
#endif
        }

        private void ShowPartGizmons(SkillPart part)
        {
#if UNITY_EDITOR
            var col = part.collider;
            if (col.radius > 0)
            {
                //circle
                var pos = entity?.transform.TransformPoint(col.pos) ?? col.pos;
                Gizmos.DrawSphere(pos.ToVector3XZ(LFloat.one), col.radius.ToFloat());
            }
            else
            {
                //aabb
                var pos = entity?.transform.TransformPoint(col.pos) ?? col.pos;
                Gizmos.DrawCube(pos.ToVector3XZ(LFloat.one), col.size.ToVector3XZ(LFloat.one));
                DebugExtension.DebugLocalCube(Matrix4x4.TRS(
                        pos.ToVector3XZ(LFloat.one),
                        Quaternion.Euler(0, entity.transform.deg.ToFloat(), 0),
                        Vector3.one),
                    col.size.ToVector3XZ(LFloat.one), Gizmos.color);
            }
#endif
        }
    }
}