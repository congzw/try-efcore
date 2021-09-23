using System.Collections.Generic;
using System.Linq;
using Common;
using TryEfCore.Libs.Data;

namespace TryEfCore.Libs.Core.Demos
{
    public class GetOrgUserCountsArgs
    {
        /// <summary>
        /// 0,1,2,3
        /// </summary>
        public int Method { get; set; }
    }

    public class OrgUserCount
    {
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public int UserCount { get; set; }
    }

    public partial interface IDemoService
    {
        MessageResult GetOrgUserCounts(GetOrgUserCountsArgs args);
    }

    partial class DemoService : IDemoService
    {
        public MessageResult GetOrgUserCounts(GetOrgUserCountsArgs args)
        {
            var messageResult = new MessageResult();
            messageResult.Message = "错误示例" + args.Method;

            if (args.Method == 1)
            {
                messageResult.Message = "正确示例1";
                messageResult.Success = true;
                messageResult.Data = GetOrgUserCounts1(_dbContext, args);
                return messageResult;
            }

            if (args.Method == 2)
            {
                messageResult.Message = "正确示例2";
                messageResult.Success = true;
                messageResult.Data = GetOrgUserCounts2(_dbContext, args);
                return messageResult;
            }

            if (args.Method == 3)
            {
                messageResult.Data = GetOrgUserCounts3(_dbContext, args);
                return messageResult;
            }

            if (args.Method == 4)
            {
                messageResult.Data = GetOrgUserCounts4(_dbContext, args);
                return messageResult;
            }

            messageResult.Message = "正确示例0";
            messageResult.Success = true;
            messageResult.Data = GetOrgUserCounts0(_dbContext, args);
            return messageResult;
        }

        private static IEnumerable<OrgUserCount> GetOrgUserCounts0(TestDbContext dbContext, GetOrgUserCountsArgs args)
        {
            //org -> user
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => o.Id == u.OrgId).DefaultIfEmpty()
                select new { o, uCount = u == null ? 0 : 1 };

            var groupQ = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgUserCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(g => g.uCount)
                });

            return groupQ.OrderByDescending(x => x.UserCount).ThenBy(x => x.OrgId);
        }

        private static IEnumerable<OrgUserCount> GetOrgUserCounts1(TestDbContext dbContext, GetOrgUserCountsArgs args)
        {
            //org -> user
            var query =
                from o in dbContext.Orgs
                join u in dbContext.Users on o.Id equals u.OrgId into temp
                from u in temp.DefaultIfEmpty()
                select new { o, uCount = u == null ? 0 : 1 };

            var groupQ = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgUserCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(g => g.uCount)
                });

            return groupQ.OrderByDescending(x => x.UserCount).ThenBy(x => x.OrgId); ;
        }
        
        private static IEnumerable<OrgUserCount> GetOrgUserCounts2(TestDbContext dbContext, GetOrgUserCountsArgs args)
        {
            //org -> user
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => o.Id == u.OrgId).DefaultIfEmpty()
                select new { o, u };

            return query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgUserCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(y => y.u == null ? 0 : 1)
                })
                .OrderByDescending(x => x.UserCount)
                .ThenBy(x => x.OrgId);
        }

        private static IEnumerable<OrgUserCount> GetOrgUserCounts3(TestDbContext dbContext, GetOrgUserCountsArgs args)
        {
            //user -> org
            var query =
                from u in dbContext.Users
                from o in dbContext.Orgs.Where(o => o.Id == u.OrgId).DefaultIfEmpty()
                select new { o, uCount = u == null ? 0 : 1 };

            var groupQ = query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgUserCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Sum(g => g.uCount)
                });

            return groupQ.OrderByDescending(x => x.UserCount).ThenBy(x => x.OrgId); ;
        }

        private static IEnumerable<OrgUserCount> GetOrgUserCounts4(TestDbContext dbContext, GetOrgUserCountsArgs args)
        {
            var query =
                from o in dbContext.Orgs
                from u in dbContext.Users.Where(u => o.Id == u.OrgId).DefaultIfEmpty()
                select new { u, o };

            return query
                .GroupBy(x => new { OrgId = x.o.Id, OrgName = x.o.Name })
                .Select(x => new OrgUserCount()
                {
                    OrgId = x.Key.OrgId,
                    OrgName = x.Key.OrgName,
                    UserCount = x.Count()
                })
                .OrderByDescending(x => x.UserCount)
                .ThenBy(x => x.OrgId); ;
        }
    }

}
