using System.Collections.Generic;
using System.Linq;
using Common;
using TryEfCore.Libs.Data;

namespace TryEfCore.Libs.Core.Demos
{
    public class GetUserCourseCountsArgs
    {
        public string OrgId { get; set; }
        /// <summary>
        /// 0,1
        /// </summary>
        public int Method { get; set; }
    }

    public class UserCourseCount
    {
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CourseCount { get; set; }
    }

    public partial interface IDemoService
    {
        MessageResult GetUserCourseCounts(GetUserCourseCountsArgs args);
    }

    partial class DemoService : IDemoService
    {
        public MessageResult GetUserCourseCounts(GetUserCourseCountsArgs args)
        {
            var messageResult = new MessageResult();
            if (args.Method == 1)
            {
                messageResult.Message = "错误示例1";
                messageResult.Data = GetUserCourseCounts1(_dbContext, args);
                return messageResult;
            }

            messageResult.Message = "正确示例0";
            messageResult.Success = true;
            messageResult.Data = GetUserCourseCounts0(_dbContext, args);
            return messageResult;
        }
        
        private IEnumerable<UserCourseCount> GetUserCourseCounts0(TestDbContext dbContext, GetUserCourseCountsArgs args)
        {
            //user -> course -> org
            var query =
                from u in dbContext.Users
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                from o in dbContext.Orgs.Where(o => u.OrgId == o.Id).DefaultIfEmpty()
                select new { o, u, cCount = c == null ? 0 : 1 };

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name, UserId = x.u.Id, UserName = x.u.Name })
                .Select(x => new UserCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserId = x.Key.UserId,
                    UserName = x.Key.UserName,
                    CourseCount = x.Sum(g => g.cCount)
                });

            if (!string.IsNullOrWhiteSpace(args.OrgId))
            {
                groupQuery = groupQuery.Where(x => x.OrgId == args.OrgId);
            }

            return groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.UserId);
        }
        
        private IEnumerable<UserCourseCount> GetUserCourseCounts1(TestDbContext dbContext, GetUserCourseCountsArgs args)
        {
            //org -> user -> course
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => o.Id == u.OrgId).DefaultIfEmpty()
                from c in dbContext.Courses.Where(c => u.Id == c.UserId).DefaultIfEmpty()
                select new { o, u, cCount = c == null ? 0 : 1 };

            var groupQuery = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name, UserId = x.u.Id, UserName = x.u.Name })
                .Select(x => new UserCourseCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserId = x.Key.UserId,
                    UserName = x.Key.UserName,
                    CourseCount = x.Sum(g => g.cCount)
                });

            if (!string.IsNullOrWhiteSpace(args.OrgId))
            {
                groupQuery = groupQuery.Where(x => x.OrgId == args.OrgId);
            }
            
            return groupQuery.OrderByDescending(x => x.CourseCount).ThenBy(x => x.UserId);
        }
    }
}
