using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TryEfCore.Libs.Core.Demos
{
    public class SearchOrgsArgs
    {
        public string Search { get; set; }
        public int Method { get; set; }
    }

    partial class DemoService : IDemoService
    {
        public IEnumerable<Org> SearchOrgs(SearchOrgsArgs args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var query = _dbContext.Orgs.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(args.Search))
            {
                var s = args.Search.Trim();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    if (args.Method == 1)
                    {
                        query = query.Where(x => EF.Functions.Like(x.Name, $"%{s}%"));
                    }
                    else
                    {
                        query = query.Where(x => x.Name.Contains(args.Search));
                    }
                }
            }
            
            return query;
        }
    }

    
    public partial interface IDemoService
    {
        IEnumerable<Org> SearchOrgs(SearchOrgsArgs args);
    }

}
