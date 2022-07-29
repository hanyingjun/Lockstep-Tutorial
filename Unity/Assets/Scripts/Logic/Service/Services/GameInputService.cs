using Lockstep.Math;
using Lockstep.Serialization;
using Lockstep.Util;
using NetMsg.Common;
using System.Collections.Generic;

namespace Lockstep.Game
{
    public class GameInputService : IInputService
    {
        public static PlayerInput CurGameInput = new PlayerInput();

        public void Execute(InputCmd cmd, object entity)
        {
            PlayerInput input = new Deserializer(cmd.content).Parse<PlayerInput>();
            PlayerInput playerInput = entity as PlayerInput;
            playerInput.mousePos = input.mousePos;
            playerInput.inputUV = input.inputUV;
            playerInput.isInputFire = input.isInputFire;
            playerInput.skillId = input.skillId;
            playerInput.isSpeedUp = input.isSpeedUp;
        }

        public List<InputCmd> GetInputCmds()
        {
            if (CurGameInput.Equals(PlayerInput.Empty))
            {
                return null;
            }

            return new List<InputCmd>() {
                new InputCmd() {
                    content = CurGameInput.ToBytes()
                }
            };
        }

        public List<InputCmd> GetDebugInputCmds()
        {
            return new List<InputCmd>() {
                new InputCmd() {
                    content = new PlayerInput() {
                        inputUV = new LVector2(LRandom.Range(-1,2),LRandom.Range(-1,2)),
                        skillId = LRandom.Range(0,3)
                    }.ToBytes()
                }
            };
        }
    }
}