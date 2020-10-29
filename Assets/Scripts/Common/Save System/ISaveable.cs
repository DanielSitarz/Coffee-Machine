namespace Machine
{
    public interface ISaveable
    {
        void Save(string baseId);
        void Load(string baseId);
    }
}