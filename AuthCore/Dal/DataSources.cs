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
            _Default = DataMapperProvider.Build("SqlConfig.xml", Assembly.GetExecutingAssembly(), "ConnectionString");
        }

        public static IDataMapper Default
        {
            get
            {
                lock (Locker)
                {
                    if (_Default == null)
                    {
                        _Default = DataMapperProvider.Build("SqlConfig.xml", Assembly.GetExecutingAssembly(), "ConnectionString");
                        //throw new Exception("初始化DataSources数据源失败");
                    }
                    return _Default;
                }
            }
        }
    }
}
