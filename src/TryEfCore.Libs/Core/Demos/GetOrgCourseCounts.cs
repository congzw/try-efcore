using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TryEfCore.Libs.Data;

namespace TryEfCore.Libs.Core.Demos
{
    public class GetOrgCourseCountsArgs
    {
        /// <summary>
        /// 0,1,2
        /// </summary>
        public int Method { get; set; }
    }

    public class OrgCourseCount
    {
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public int CourseCount { get; set; }
        public int UserCount { get; set; }
    }

    public partial interface IDemoService
    {
        MessageResult GetOrgCourseCounts(GetOrgCourseCountsArgs args);
    }

    partial class DemoService : IDemoService
    {
        public MessageResult GetOrgCourseCounts(GetOrgCourseCountsArgs args)
        {
            var messageResult = new MessageResult();
            if (args.Method == 1)
            {
                messageResult.Message = "期望结果[Admin(org-2):13, LuRenJia(null):3, Super(org-1):1] => 错误示例";
                messageResult.Data = GetOrgCourseCounts1(_dbContext, args);
                return messageResult;
            }
            if (args.Method == 2)
            {
                messageResult.Message = "期望结果[Admin(org-2):13, LuRenJia(null):3, Super(org-1):1] => 错误示例";
                messageResult.Data = GetOrgCourseCounts2(_dbContext, args);
                return messageResult;
            }
            if (args.Method == 3)
            {
                messageResult.Message = "期望结果[Admin(org-2):13, LuRenJia(null):3, Super(org-1):1] => 错误示例";
                messageResult.Data = GetOrgCourseCounts3(_dbContext, args);
                return messageResult;
            }

            messageResult.Success = true;
            messageResult.Message = "期望结果[Admin(org-1):13, LuRenJia(null):3, Super(org-2):1] => 正确示例";
            messageResult.Data = GetOrgCourseCounts0(_dbContext, args);
            return messageResult;
        }
        
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts0(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            var query =
                from u in dbContext.Users
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                from o in dbContext.Orgs.Where(o => o.Id == u.OrgId)
                select new { o, u, cCount = c == null ? 0 : 1, uCount = u == null ? 0 : 1};

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name, UserId = x.u.Id, UserName = x.u.Name})
                .Select(x => new 
                {
                    UserId = x.Key.UserId,
                    UserName = x.Key.UserName,
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.cCount),
                    UserCount = x.Sum(g => g.uCount)
                });
            
            var temp = groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.OrgId).ToList();
            return temp.GroupBy(x => new {x.OrgId, x.OrgName})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(y => y.UserCount),
                    CourseCount = x.Sum(y => y.CourseCount)
                });
        }
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts000(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            var query =
                from u in dbContext.Users
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                select new { u, cCount = c == null ? 0 : 1};
            var query2 = 
                from o in dbContext.Orgs
                from t in query.Where(t => t.u.OrgId == o.Id).DefaultIfEmpty()
                select new {t, o, uCount = t == null ? 0:1 };

            var groupQuery = query2
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.t.cCount),
                    UserCount = x.Sum(g => g.uCount)
                });
            
            return groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.OrgId);
        }
        
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts00(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            //user -> course -> org
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                select new { o, u, cCount = c == null ? 0 : 1, uCount = u == null ? 0:1 };

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.cCount),
                    UserCount = x.Sum(g => g.uCount)
                });
            
            return groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.OrgId);
        }
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts1(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            //user -> course -> org
            var query =
                from u in dbContext.Users
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                from o in dbContext.Orgs.Where(o => u.OrgId == o.Id)
                select new { o, u, cCount = c == null ? 0 : 1 };

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.cCount)
                });
            
            return groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.OrgId);
        }
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts2(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            //user -> course -> org
            var query =
                from u in dbContext.Users
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                from o in dbContext.Orgs.Where(o => u.OrgId == o.Id).DefaultIfEmpty()
                select new { o, u, cCount = c == null ? 0 : 1 };

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.cCount)
                });
            
            return groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.OrgId);
        }
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts3(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            //user -> course -> org
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                from c in dbContext.Courses.Where(c => u.Id == c.UserId)
                select new { o, u, cCount = c == null ? 0 : 1 };

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name})
                .Select(x => new OrgCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    CourseCount = x.Sum(g => g.cCount)
                });
            
            return groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.OrgId);
        }
    }
}
