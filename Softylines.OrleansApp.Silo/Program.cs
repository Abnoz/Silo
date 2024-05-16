﻿ 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using System.Net;
using Softylines.OrleansApp.Silo;

try
{
    StartSilo(args);
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    return 1;
}

static void StartSilo(string[ ] args)
{
  

    string sqlServerConnectionString = "Server=192.168.30.35;Database=OrleansDb;User Id=sa;Password=Anis2004#;";
 

    var builder = Host.CreateDefaultBuilder()
        .UseOrleans((context, silo) =>
        {
            silo
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = "System.Data.SqlClient";
                    options.ConnectionString = sqlServerConnectionString;
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "aniss";
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Parse("192.168.0.142");
                    options.SiloPort = 30300;
                    options.GatewayPort = 30000;
                    options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 30300);
                    options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 30000);
                })
                // .ConfigureEndpoints(default, default,  default, listenOnAnyHostAddress: true)
                .AddAdoNetGrainStorage(
                        name: "GrainStore",
                        configureOptions: options =>
                        {
                            options.ConnectionString = sqlServerConnectionString;
                        }) 
                .UseDashboard(options =>
                {
                    options.Username = "username";
                    options.Password = "password";
                    options.Host = "localhost";
                    options.Port = 9000;
                    options.HostSelf = true;
                    options.CounterUpdateIntervalMs = 1000;
                }) 
                .ConfigureLogging(logging =>
                {
                    logging
                        .AddConsole();
                }); 
        });

    var app = builder.Build();
    app.Run();
}
