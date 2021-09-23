using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TryEfCore.Libs.Core.Demos
{
    public class SearchUsers
    {
        public string Search { get; set; }
    }
    
    public class SearchUsersArgs
    {
        public string Search { get; set; }
        public int Method { get; set; }
    }
    
    public partial interface IDemoService
    {
        IEnumerable<User> SearchUsers(SearchUsersArgs args);
    }

    partial class DemoService : IDemoService
    {
        public IEnumerable<User> SearchUsers(SearchUsersArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            if (args.Method == 1)
            {
                return SearchUsers1(args);
            }
            return SearchUsers0(args);
        }
        
        private IEnumerable<User> SearchUsers0(SearchUsersArgs args)
        {
            var query =
                from u in _dbContext.Users
                from o in _dbContext.Orgs.Where(o => u.OrgId == o.Id).DefaultIfEmpty()
                select new {u, o};
            
            if (!string.IsNullOrWhiteSpace(args.Search))
            {
                var s = args.Search.Trim();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    query = query.Where(x => EF.Functions.Like(x.u.Name, $"%{s}%") ||
                                             EF.Functions.Like(x.o.Name, $"%{s}%"));
                }
            }
            
            return query.Select(x => x.u);

        }
        
        private IEnumerable<User> SearchUsers1(SearchUsersArgs args)
        {
            var query =
                from u in _dbContext.Users
                join o in _dbContext.Orgs on u.OrgId equals o.Id into temp
                from o in temp.DefaultIfEmpty()
                select new {u, o};
            
            if (!string.IsNullOrWhiteSpace(args.Search))
            {
                var s = args.Search.Trim();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    query = query.Where(x => EF.Functions.Like(x.u.Name, $"%{s}%") ||
                                             EF.Functions.Like(x.o.Name, $"%{s}%"));
                }
            }
            
            return query.Select(x => x.u);

        }
    }
}
