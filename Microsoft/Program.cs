using Microsoft;
using System.ServiceProcess;

var service = new CustomService();
if(Environment.UserInteractive)
{
    service.Start();
    Console.ReadKey();
    service.Stop();
}
else
{
    ServiceBase.Run(service);
}