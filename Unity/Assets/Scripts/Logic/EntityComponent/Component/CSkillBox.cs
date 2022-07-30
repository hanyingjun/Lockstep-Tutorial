using System;
using System.Collections.Generic;
using Lockstep.Math;
using Debug = Lockstep.Logging.Debug;
#if UNITY_EDITOR
using HideInInspector = UnityEngine.HideInInspector;
#endif

namespace Lockstep.Game
{
    [Serializable]
    public partial class CSkillBox : Component, ISkillEventHandler
    {
        [Backup]
        public int configId;
        [Backup]
        public bool isFiring;
#if UNITY_EDITOR
        [HideInInspector]
#endif
        [NoBackup]
        public SkillBoxConfig config;
        [Backup]
        private int _curSkillIdx = 0;
        [Backup]
        private List<Skill> _skills = new List<Skill>();
        public Skill curSkill
        {
            get
            {
                if (_curSkillIdx >= 0)
                {
                    return _skills[_curSkillIdx];
                }
                return null;
            }
        }

        public override void BindEntity(BaseEntity e)
        {
            base.BindEntity(e);
            config = entity.GetService<IGameConfigService>().GetSkillConfig(configId);
            if (config == null)
            {
                Debug.LogError("Œ¥’“µΩººƒ‹≈‰÷√: " + configId);
                return;
            }

            if (config.skillInfos.Count != _skills.Count)
            {
                _skills.Clear();
                for (int i = 0; i < config.skillInfos.Count; i++)
                {
                    var skill = new Skill();
                    _skills.Add(skill);
                    skill.BindEntity(entity, config.skillInfos[i], this);
                    skill.DoStart();
                }
            }
            else
            {
                for (int i = 0; i < _skills.Count; i++)
                {
                    _skills[i].RebindEntity(entity, config.skillInfos[i], this);
                }
            }
        }

        public override void DoUpdate(LFloat deltaTime)
        {
            foreach (var skill in _skills)
            {
                skill.DoUpdate(deltaTime);
            }
        }

        public bool Fire(int idx)
        {
            if (idx < 0 || idx > _skills.Count)
            {
                return false;
            }

            if (isFiring)
                return false;
            Skill skill = _skills[idx];
            if (skill.Fire())
            {
                _curSkillIdx = idx;
                return true;
            }

            Debug.Log($"TryFire failure {idx} {skill.CdTimer}  {skill.State}");
            return false;
        }

        public void ForceStop(int idx = -1)
        {
            if (idx == -1)
            {
                idx = _curSkillIdx;
            }

            if (idx < 0 || idx > _skills.Count)
            {
                return;
            }

            if (curSkill != null)
            {
                curSkill.ForceStop();
            }
        }

        public void OnSkillStart(Skill skill)
        {
            Debug.Log("OnSkillStart " + skill.SkillInfo.animName);
            isFiring = true;
            entity.isInvincible = true;
        }

        public void OnSkillDone(Skill skill)
        {
            Debug.Log("OnSkillDone " + skill.SkillInfo.animName);
            isFiring = false;
            entity.isInvincible = false;
        }

        public void OnSkillPartStart(Skill skill)
        {
            Debug.Log("OnSkillPartStart " + skill.SkillInfo.animName);
        }

        public void OnDrawGizmos()
        {
            if (config == null) return;
#if UNITY_EDITOR
            foreach (var skill in _skills)
            {
                skill.OnDrawGizmos();
            }
#endif
        }
    }
}