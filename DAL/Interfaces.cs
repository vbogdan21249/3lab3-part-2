namespace DAL
{
    public interface ISerializationProvider<T>
    {
        void Save(List<T> listToSave);
        List<T> Load();
    }
    interface IStudy
    {
        public string Study();
    }
    interface IFly
    {
        public string Fly();
    }
    interface ICompose
    {
        public string Compose();
    }
    interface ISkate
    {
        public string Skate();
    }
}
