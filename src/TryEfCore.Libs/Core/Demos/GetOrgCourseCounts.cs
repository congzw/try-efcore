using System.Collections.Generic;
using System.Linq;
using Common;
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
                messageResult.Message = "错误示例1";
                messageResult.Data = GetOrgCourseCounts1(_dbContext, args);
                return messageResult;
            }
            if (args.Method == 2)
            {
                messageResult.Message = "错误示例2";
                messageResult.Data = GetOrgCourseCounts2(_dbContext, args);
                return messageResult;
            }

            messageResult.Success = true;
            messageResult.Message = "正确示例0";
            messageResult.Data = GetOrgCourseCounts0(_dbContext, args);
            return messageResult;
        }
        
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts0(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            //org -> user -> course
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                select new { o, u, cCount = c == null ? 0 : 1};

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
        private IEnumerable<OrgCourseCount> GetOrgCourseCounts1(TestDbContext dbContext, GetOrgCourseCountsArgs args)
        {
            //org -> user -> course
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => u.OrgId == o.Id).DefaultIfEmpty()
                //LEFT JOIN
                //from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                //INNER JOIN
                from c in dbContext.Courses.Where(c => u.Id == c.UserId)
                select new { o, u, cCount = c == null ? 0 : 1};

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
    }
}
