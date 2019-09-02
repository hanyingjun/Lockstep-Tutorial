using Lockstep.Serialization;

namespace Lockstep.Game {
    public interface IBackup {
        void WriteBackup(Serializer writer);
        void ReadBackup(Deserializer reader);
    }

    public interface IAfterBackup {
        void OnAfterDeserialize();//反序列化后重构
    }
}