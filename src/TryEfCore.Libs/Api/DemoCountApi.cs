using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.AspNetCore.Mvc;
using TryEfCore.Libs.Core.Demos;

namespace TryEfCore.Libs.Api
{
    [Route("~/Api/Demo/Count/[action]")]
    public class DemoCountApi : AllInOneApi
    {
        private readonly IDemoService _demoService;

        public DemoCountApi(IDemoService demoService)
        {
            _demoService = demoService;
        }
        
        [HttpGet]
        public MessageResult GetOrgUserCounts([FromQuery] GetOrgUserCountsArgs args)
        {
            return _demoService.GetOrgUserCounts(args);
        }

        [HttpGet]
        public MessageResult GetOrgCourseCounts([FromQuery] GetOrgCourseCountsArgs args)
        {
            return _demoService.GetOrgCourseCounts(args);
        }

        /// <summary>
        /// method:[0,1]
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpGet]
        public MessageResult GetUserCourseCounts([FromQuery] GetUserCourseCountsArgs args)
        {
            return _demoService.GetUserCourseCounts(args);
        }
    }
}
