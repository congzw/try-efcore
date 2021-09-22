using System.Collections.Generic;
using System.Linq;
using Common.Utilities;
using OrchardCore.Modules;

namespace Common.Shared.Boots
{
    public class StartupHelper
    {
        #region for di extensions

        [LazySingleton]
        public static StartupHelper Instance => LazySingleton.Instance.Resolve<StartupHelper>(() => new StartupHelper());

        #endregion
        
        public List<StartupOrderItem> Items { get; set; } = new List<StartupOrderItem>();

        public StartupOrderItem GuessOrder(StartupBase startup, int defaultOrder, int defaultConfigureOrder)
        {
            var name = startup.GetType().FullName;
            var theOne = Items.SingleOrDefault(x => x.Name.MyEquals(name));
            if (theOne == null)
            {
                theOne = StartupOrderItem.Create(name, defaultOrder, defaultConfigureOrder);
                Items.Add(theOne);
            }
            return theOne;
        }

        private readonly IFileDbHelper _fileHelper = FileDbHelper.Instance;
        public void SaveToFile(List<StartupOrderItem> items)
        {
            var fileName = _fileHelper.MakeFileName<StartupOrderItem>();
            var filePath = _fileHelper.MakeAppDataFilePath(fileName);
            _fileHelper.Save(filePath, items);
        }
        public List<StartupOrderItem> LoadFromFile()
        {
            var fileName = _fileHelper.MakeFileName<StartupOrderItem>();
            var filePath = _fileHelper.MakeAppDataFilePath(fileName);
            return _fileHelper.Read<StartupOrderItem>(filePath);
        }

        public List<StartupOrderItem> CreateDefault()
        {
            var items = new List<StartupOrderItem>();

            items.Add(StartupOrderItem.Create("NbSites.Web.Boots.EntryOrchardStartup", 0,0));
            items.Add(StartupOrderItem.Create("NbSites.Base.Startup", 100,100));
            items.Add(StartupOrderItem.Create("NbSites.Base.Dic.Startup", 101,101));
            items.Add(StartupOrderItem.Create("NbSites.Base.Ds.Startup", 102,102));
            items.Add(StartupOrderItem.Create("NbSites.Base.Auth.Startup", 103,103));
            items.Add(StartupOrderItem.Create("NbSites.Base.User.Startup", 104,104));
            items.Add(StartupOrderItem.Create("NbSites.Base.Res.Startup", 105,105));
            
            return items;
        }
    }

    public class StartupOrderItem
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public int ConfigureOrder { get; set; }
        
        public static StartupOrderItem Create(string name, int order, int configureOrder)
        {
            return new StartupOrderItem { Name = name, Order = order, ConfigureOrder = configureOrder };
        }
    }
}
