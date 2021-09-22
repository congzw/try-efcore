using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Common.Shared.Boots
{
    public abstract class MyStartupBase : StartupBase
    {
        protected MyStartupBase()
        {
            var startupHelper = StartupHelper.Instance;
            var guessOrder = startupHelper.GuessOrder(this, 1000, 1000);
            Order = guessOrder.Order;
            ConfigureOrder = guessOrder.ConfigureOrder;
        }

        #region readme
        
        //ConfigureServices会被调用3次，而Configure只会被调用1次
        //第3次ConfigureServices调用可能在Configure之后
        //"20210827-094043:937 => MainStartup.MainConfigureService",
        //"20210827-094044:312 => MainStartup.MainConfigure",
        //"20210827-094044:650 => NbSites.App.Main.Startup.ConfigureServices 1 called with services => OrchardCore.Environment.Shell.Builders.FeatureAwareServiceCollection[@]67100588",
        //"20210827-094044:651 =>     Common.Shared.Boots.MyStartupBase.ConfigureServices",
        //"20210827-094044:651 =>     NbSites.App.Main.Startup.ConfigureServices",
        //"20210827-094044:651 =>     OrchardCore.Environment.Shell.Builders.ShellContainerFactory.CreateContainer",
        //"20210827-094044:651 =>     OrchardCore.Environment.Shell.Builders.ShellContextFactory.CreateDescribedContextAsync",
        //"20210827-094044:651 =>     OrchardCore.Environment.Shell.Builders.ShellContextFactory.OrchardCore.Environment.Shell.Builders.IShellContextFactory.CreateShellContextAsync",
        //"20210827-094044:651 =>     OrchardCore.Environment.Shell.Distributed.DistributedShellHostedService.CreateDistributedContextAsync",
        //"20210827-094044:651 =>     OrchardCore.Environment.Shell.Distributed.DistributedShellHostedService.LoadingAsync",
        //"20210827-094044:683 => NbSites.App.Main.Startup.ConfigureServices 1 called with services => OrchardCore.Environment.Shell.Builders.FeatureAwareServiceCollection[@]39387817",
        //"20210827-094044:684 =>     Common.Shared.Boots.MyStartupBase.ConfigureServices",
        //"20210827-094044:684 =>     NbSites.App.Main.Startup.ConfigureServices",
        //"20210827-094044:684 =>     OrchardCore.Environment.Shell.Builders.ShellContainerFactory.CreateContainer",
        //"20210827-094044:684 =>     OrchardCore.Environment.Shell.Builders.ShellContextFactory.CreateDescribedContextAsync",
        //"20210827-094044:684 =>     OrchardCore.Environment.Shell.Builders.ShellContextFactory.OrchardCore.Environment.Shell.Builders.IShellContextFactory.CreateShellContextAsync",
        //"20210827-094044:684 =>     OrchardCore.Environment.Shell.ShellHost.CreateShellContextAsync",
        //"20210827-094044:684 =>     OrchardCore.Environment.Shell.ShellHost.GetOrCreateShellContextAsync",
        //"20210827-094044:684 =>     OrchardCore.Environment.Shell.ShellHost.GetScopeAsync",
        //"20210827-094044:709 => NbSites.App.Main.Startup.Configure 1 called by => Microsoft.AspNetCore.Builder.ApplicationBuilder[@]55201310",
        //"20210827-094045:294 => NbSites.App.Main.Startup.ConfigureServices 1 called with services => OrchardCore.Environment.Shell.Builders.FeatureAwareServiceCollection[@]6608938",
        //"20210827-094045:295 =>     Common.Shared.Boots.MyStartupBase.ConfigureServices",
        //"20210827-094045:295 =>     NbSites.App.Main.Startup.ConfigureServices",
        //"20210827-094045:295 =>     OrchardCore.Environment.Shell.Builders.ShellContainerFactory.CreateContainer",
        //"20210827-094045:295 =>     OrchardCore.Environment.Shell.Builders.ShellContextFactory.CreateDescribedContextAsync",
        //"20210827-094045:295 =>     OrchardCore.Environment.Shell.Distributed.DistributedShellHostedService.CreateDistributedContextAsync",
        //"20210827-094045:295 =>     OrchardCore.Environment.Shell.Distributed.DistributedShellHostedService.GetOrCreateDistributedContextAsync"


        #endregion
        public override int Order { get; }
        public override int ConfigureOrder { get; }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            //StartupLogHelper.Instance.Log($"{this.GetType().FullName}.ConfigureServices {this.Order} called with services => {services.GetHashCodeMessage()}");
            //var methodInvokes = new List<string>();
            //var stackTrace = new StackTrace();
            //var stackFrames = stackTrace.GetFrames();
            //foreach (var stackFrame in stackFrames)
            //{
            //    if (stackFrame != null)
            //    {
            //        var method = stackFrame.GetMethod();
            //        if (method != null)
            //        {
            //            methodInvokes.Add($"{method.GetMethodDescription()}");
            //        }
            //    }
            //}
            //foreach (var methodInvoke in methodInvokes)
            //{
            //    if (methodInvoke.StartsWith("System") || methodInvoke.EndsWith(".MoveNext"))
            //    {
            //        continue;
            //    }
            //    StartupLogHelper.Instance.Log($"    {methodInvoke}");
            //}


            //StartupLogHelper.Instance.Log($"{this.GetType().FullName}.ConfigureServices {this.Order} called by => {services.GetHashCodeMessage()}");
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(builder, routes, serviceProvider);
            //StartupLogHelper.Instance.Log($"{this.GetType().FullName}.Configure {this.Order} called by => {builder.GetHashCodeMessage()}");
        }
    }
}
