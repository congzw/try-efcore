using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TryEfCore.Libs.Core.Demos;

namespace TryEfCore.Libs.Api
{
    [Route("~/Api/Demo/Search/[action]")]
    public class DemoQueryApi : AllInOneApi
    {
        private readonly IDemoService _demoService;

        public DemoQueryApi(IDemoService demoService)
        {
            _demoService = demoService;
        }
        
        [HttpGet]
        public IEnumerable<User> SearchUsers([FromQuery] SearchUsersArgs args)
        {
            return _demoService.SearchUsers(args);
        }
        
        [HttpGet]
        public IEnumerable<Org> SearchOrgs([FromQuery] SearchOrgsArgs args)
        {
            return _demoService.SearchOrgs(args);
        }
    }
}