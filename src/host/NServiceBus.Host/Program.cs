﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NServiceBus.Host.Internal;
using NServiceBus.Host.Internal.Arguments;
using Topshelf;
using Topshelf.Configuration;
using System.Configuration;
using Topshelf.Internal;

namespace NServiceBus.Host
{
    /// <summary>
    /// Entry point to the process.
    /// </summary>
    public class Program
    {
        private static void Main(string[] args)
        {
                Parser.Args commandLineArguments = Parser.ParseArgs(args);
                var arguments = new HostArguments(commandLineArguments);

                if (arguments.Help != null)
                {
                    DisplayHelpContent();

                    return;
                }

                Type endpointConfigurationType = GetEndpointConfigurationType();

                AssertThatEndpointConfigurationTypeHasDefaultConstructor(endpointConfigurationType);

                string endpointConfigurationFile = GetEndpointConfigurationFile(endpointConfigurationType);

                if (!File.Exists(endpointConfigurationFile))
                {
                    throw new InvalidOperationException("No configuration file found at: " + endpointConfigurationFile);
                }

                var endpointConfiguration = Activator.CreateInstance(endpointConfigurationType);

                EndpointId = GetEndpointId(endpointConfiguration);

                AppDomain.CurrentDomain.SetupInformation.AppDomainInitializerArguments = args;

                IRunConfiguration cfg = RunnerConfigurator.New(x =>
                {
                    x.ConfigureServiceInIsolation<GenericHost>(endpointConfigurationType.AssemblyQualifiedName, c =>
                    {
                        c.ConfigurationFile(endpointConfigurationFile);
                        c.CommandLineArguments(args, () => SetHostServiceLocatorArgs);
                        c.WhenStarted(service => service.Start());
                        c.WhenStopped(service => service.Stop());
                        c.CreateServiceLocator(() => new HostServiceLocator());
                    });

                    if (arguments.Username != null && arguments.Password != null)
                    {
                        x.RunAs(arguments.Username.Value, arguments.Password.Value);
                    }
                    else
                    {
                        x.RunAsLocalSystem();
                    }

                    if (arguments.StartManually != null)
                    {
                        x.DoNotStartAutomatically();
                    }

                    x.SetDisplayName(arguments.DisplayName != null ? arguments.DisplayName.Value : EndpointId);
                    x.SetServiceName(arguments.ServiceName != null ? arguments.ServiceName.Value : EndpointId);
                    x.SetDescription(arguments.Description != null ? arguments.Description.Value : "NServiceBus Message Endpoint Host Service");

                    var serviceCommandLine = commandLineArguments.CustomArguments.AsCommandLine();

                    if (arguments.ServiceName != null)
                    {
                        serviceCommandLine += " /serviceName:\"" + arguments.ServiceName.Value + "\"";
                    }

                    x.SetServiceCommandLine(serviceCommandLine);

                    x.DependencyOnMsmq();

                    if (arguments.DependsOn != null)
                    {
                        var dependencies = arguments.DependsOn.Value.Split(',');

                        foreach (var dependency in dependencies)
                        {
                            if (dependency.ToUpper() == KnownServiceNames.Msmq)
                            {
                                continue;
                            }

                            x.DependsOn(dependency);
                        }
                    }
                });

                Runner.Host(cfg, args);
        }

        private static void DisplayHelpContent()
        {
            try
            {
                var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("NServiceBus.Host.Content.Help.txt");

                if (stream != null)
                {
                    var helpText = new StreamReader(stream).ReadToEnd();

                    Console.WriteLine(helpText);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Gives an identifier for this endpoint
        /// </summary>
        public static string EndpointId { get; set; }

        private static void SetHostServiceLocatorArgs(string[] args)
        {
            HostServiceLocator.Args = args;
        }

        private static void AssertThatEndpointConfigurationTypeHasDefaultConstructor(Type type)
        {
            var constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
                throw new InvalidOperationException("Endpoint configuration type needs to have a default constructor: " + type.FullName);
        }

        private static string GetEndpointConfigurationFile(Type endpointConfigurationType)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                endpointConfigurationType.Assembly.ManifestModule.Name + ".config");
        }

        /// <summary>
        /// Gives a string which serves to identify the endpoint.
        /// </summary>
        /// <param name="endpointConfiguration"></param>
        /// <returns></returns>
        public static string GetEndpointId(object endpointConfiguration)
        {
            string endpointName = endpointConfiguration.GetType().FullName;
            return string.Format("{0}_v{1}", endpointName, endpointConfiguration.GetType().Assembly.GetName().Version);
        }

        private static Type GetEndpointConfigurationType()
        {
            string endpoint = ConfigurationManager.AppSettings["EndpointConfigurationType"];
            if (endpoint != null)
                return Type.GetType(endpoint, true);

            IEnumerable<Type> endpoints = ScanAssembliesForEndpoints();

            ValidateEndpoints(endpoints);

            return endpoints.First();
        }

        private static IEnumerable<Type> ScanAssembliesForEndpoints()
        {
            foreach (var assembly in AssemblyScanner.GetScannableAssemblies())
                foreach (Type type in assembly.GetTypes().Where(t => typeof(IConfigureThisEndpoint).IsAssignableFrom(t) && t != typeof(IConfigureThisEndpoint)))
                {
                    yield return type;
                }
        }

        private static void ValidateEndpoints(IEnumerable<Type> endpointConfigurationTypes)
        {
            if (endpointConfigurationTypes.Count() == 0)
            {
                throw new InvalidOperationException("No endpoint configuration found in scanned assemblies. " +
                    "This usually happens when NServiceBus fails to load your assembly contaning IConfigureThisEndpoint." +
                    " Try specifying the type explicitly in the NServiceBus.Host.exe.config using the appsetting key: EndpointConfigurationType" +
                    "Scanned path: " + AppDomain.CurrentDomain.BaseDirectory);
            }

            if (endpointConfigurationTypes.Count() > 1)
            {
                throw new InvalidOperationException("Host doesn't support hosting of multiple endpoints. " +
                                                    "Endpoint classes found: " +
                                                    string.Join(", ",
                                                                endpointConfigurationTypes.Select(
                                                                    e => e.AssemblyQualifiedName).ToArray()) +
                                                    " You may have some old assemblies in your runtime directory." +
                                                    " Try right-clicking your VS project, and selecting 'Clean'."
                                                    );

            }
        }
    }
}