using Topshelf;
using TopShelfService;

HostFactory.Run(x =>
{
    x.Service<CustomService>();
    x.SetServiceName("TopShelfServiceExample2");
    x.SetDisplayName("TopShelf Service Example 2");

    x.SetDescription("An example Windows service using Topshelf.");

    x.EnableServiceRecovery(r =>
    {
        r.RestartService(1)// Restart the service after 1 minute
        .RestartService(5)
        .TakeNoAction();
        
        r.SetResetPeriod(1); // Reset the failure count after 1 day
    });

    x.DependsOn("Service.Microsoft");

    x.RunAsLocalSystem();
    x.StartAutomaticallyDelayed();


});