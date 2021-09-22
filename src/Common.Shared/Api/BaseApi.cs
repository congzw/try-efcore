using System;
using Microsoft.AspNetCore.Mvc;

namespace Common.Shared.Api
{
    public interface IApi { }

    [IgnoreAntiforgeryToken]
    [ApiController]
    public abstract class BaseApi : IApi
    {
        [ApiExplorerSettings(GroupName = "HideGroup")]
        [HttpGet]
        public virtual string GetStatus()
        {
            return this.GetType().FullName;
        }

        [ApiExplorerSettings(GroupName = "HideGroup")]
        [HttpGet]
        public virtual DateTimeOffset GetNow()
        {
            return DateTimeOffset.Now;
        }
    }
}
