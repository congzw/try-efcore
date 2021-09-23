using System.Collections.Generic;
using System.Linq;
using Common;
using TryEfCore.Libs.Data;

namespace TryEfCore.Libs.Core.Demos
{
    public class OrgComplexCount
    {
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public int UserCount { get; set; }
        public int CourseCount { get; set; }
    }

    public class GetOrgComplexCountsArgs
    {
        public int Method { get; set; }
    }

    public partial interface IDemoService
    {
        MessageResult GetOrgComplexCounts(GetOrgComplexCountsArgs args);
    }

    partial class DemoService : IDemoService
    {
        public MessageResult GetOrgComplexCounts(GetOrgComplexCountsArgs args)
        {
            var messageResult = new MessageResult();
            if (args.Method == 1)
            {
                messageResult.Message = "错误示例1";
                messageResult.Data = GetOrgComplexCounts1(_dbContext, args);
                return messageResult;
            }

            if (args.Method == 2)
            {
                messageResult.Message = "错误示例2";
                messageResult.Data = GetOrgComplexCounts2(_dbContext, args);
                return messageResult;
            }

            messageResult.Success = true;
            messageResult.Message = "正确示例0";
            messageResult.Data = GetOrgComplexCounts0(_dbContext, args);
            return messageResult;
        }


        private IEnumerable<OrgComplexCount> GetOrgComplexCounts0(TestDbContext dbContext, GetOrgComplexCountsArgs args)
        {
            //org -> user -> course
            var queryOrgUser =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                select new { o, u, uCount = u == null ? 0 : 1};
            
            var groupOrgUser = queryOrgUser
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgUserCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(g => g.uCount)
                }).ToList();
            
            var queryOrgCourse =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                select new { o, u, cCount = c == null ? 0 : 1 };
            
            var groupOrgCourse = queryOrgCourse
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.cCount)
                }).ToList();

            //join in memory
            var query =
                from ouCount in groupOrgUser
                from ocCount in groupOrgCourse.Where(x => x.OrgId == ouCount.OrgId)
                select new OrgComplexCount()
                {
                    OrgId =ouCount.OrgId,
                    OrgName = ouCount.OrgName,
                    UserCount =ouCount.UserCount,
                    CourseCount = ocCount.CourseCount
                };

            return query.OrderBy(x => x.OrgId);
        }

        private IEnumerable<OrgComplexCount> GetOrgComplexCounts1(TestDbContext dbContext, GetOrgComplexCountsArgs args)
        {
            //org -> user -> course
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                select new { o, u, uCount = u == null ? 0 : 1, cCount = c == null ? 0 : 1 };

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgComplexCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(g => g.uCount),
                    CourseCount = x.Sum(g => g.cCount)
                });

            return groupQuery.OrderBy(x => x.OrgId);
        }
        
        private IEnumerable<OrgComplexCount> GetOrgComplexCounts2(TestDbContext dbContext, GetOrgComplexCountsArgs args)
        {
            //org -> user -> course
            var queryOrgUser =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                select new { o, u, uCount = u == null ? 0 : 1};
            
            var groupOrgUser = queryOrgUser
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgUserCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(g => g.uCount)
                });


            var queryOrgCourse =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                select new { o, u, cCount = c == null ? 0 : 1 };
            
            var groupOrgCourse = queryOrgCourse
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.cCount)
                });


            var query =
                from ouCount in groupOrgUser
                from ocCount in groupOrgCourse.Where(x => x.OrgId == ouCount.OrgId)
                select new OrgComplexCount()
                {
                    OrgId =ouCount.OrgId,
                    OrgName = ouCount.OrgName,
                    UserCount =ouCount.UserCount,
                    CourseCount = ocCount.CourseCount
                };

            return query.OrderBy(x => x.OrgId);
        }

    }
}
