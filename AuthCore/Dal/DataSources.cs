using System.Reflection;
using Victory.Dao;

namespace AuthCore.Dal
{
    public static class DataSources
    {
        private static IDataMapper _Default;
        private static object Locker = new object();

        static DataSources()
        {

        }

        public static void Init()
        {
            _Default = DataMapperProvider.Build("Dal/SqlConfig.xml", Assembly.GetExecutingAssembly(), "ConnectionString");
        }

        public static IDataMapper Default
        {
            get
            {
                lock (Locker)
                {
                    if (_Default == null)
                    {
                        Init();
                    }
                    return _Default;
                }
            }
        }
    }
}
