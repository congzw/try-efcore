using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Shared.Contract.Data;
using Common.Shared.EFCore.Helpers;
using Common.Shared.Seeds;
using TryEfCore.Libs.Core.Demos;
using TryEfCore.Libs.Data;

namespace TryEfCore.Libs.Core.Seeds
{
    public class SeedForAll : ISeedProvider
    {
        private readonly TestDbContext _dbContext;

        public SeedForAll(TestDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Order { get; set; }

        public void Setup(SeedRegistry registry)
        {
            //orgs x4
            var orgsSeed = registry.GetTestSeed(typeof(Org), true);
            orgsSeed.AddItem(new Org{Id = "org-1", Name = "组织1", ParentId = null});
            orgsSeed.AddItem(new Org{Id = "org-1.1", Name = "组织1.1", ParentId = "org-1"});
            orgsSeed.AddItem(new Org{Id = "org-2", Name = "组织2", ParentId = null});
            orgsSeed.AddItem(new Org{Id = "org-2.1", Name = "组织2.1", ParentId = "org-2"});
            
            //users: x(5 + 1)
            var userSeed = registry.GetTestSeed(typeof(User), true);
            userSeed.AddItem(new User { Id = "user-super", Name = "Super", OrgId = "org-1" });
            userSeed.AddItem(new User { Id = "user-admin", Name = "Admin", OrgId = "org-2" });
            userSeed.AddItem(new User { Id = "user-no-org", Name = "LuRenJia", OrgId = null});
            userSeed.AddItem(new User { Id = "user-no-org2", Name = "LuRenYi", OrgId = "org-not-exist"});
            for (int i = 1; i <= 3; i++)
            {
                userSeed.AddItem(new User { Id = $"user-{i:000}", Name = $"用户{i:000}", OrgId = "org-1.1"});
            }

            //courses: x(14 + 3)
            var courseSeed = registry.GetTestSeed(typeof(Course), true);
            courseSeed.AddItem(new Course{Id = "course-a", Title = "小马过河-1", UserId = "user-super"});
            courseSeed.AddItem(new Course{Id = "course-b", Title = "小马过河-2", UserId = "user-admin"});
            for (int i = 1; i <= 12; i++)
            {
                courseSeed.AddItem(new Course{Id = $"course-{i:000}", Title = $"课程-{i:000}",UserId = "user-admin"});
            }
            for (int i = 13; i <= 15; i++)
            {
                courseSeed.AddItem(new Course{Id = $"course-{i:000}", Title = $"课程-{i:000}",UserId = "user-no-org"});
            }
        }

        public void Seed(SeedRegistry registry, string category)
        {
            SeedFor<Org>(registry, category);
            SeedFor<User>(registry, category);
            SeedFor<Course>(registry, category);
        }
        
        private void SeedFor<T>(SeedRegistry registry, string category) where T : BaseEntity
        {
            var seedType = typeof(T);
            var theSeed = registry.GetSeed(category, seedType, false);
            if (theSeed == null || theSeed.Items.IsNullOrEmpty())
            {
                return;
            }

            var tableExist = _dbContext.TableExist<T>();
            if (tableExist)
            {
                var seedItems = theSeed.AsItems<T>();
                var existRecords = _dbContext.Set<T>().ToList();
                var needAddItems = new List<T>();

                foreach (var seedItem in seedItems)
                {
                    var theOne = existRecords.SingleOrDefault(x => x.Id.MyEquals(seedItem.Id));
                    if (theOne == null)
                    {
                        needAddItems.Add(seedItem);
                    }
                }

                if (needAddItems.Count > 0)
                {
                    _dbContext.Set<T>().AddRange(needAddItems);
                    _dbContext.SaveChanges(true);
                }
            }
        }
    }
}
