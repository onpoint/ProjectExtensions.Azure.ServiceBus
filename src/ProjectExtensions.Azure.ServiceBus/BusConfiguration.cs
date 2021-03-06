﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Autofac;
using ProjectExtensions.Azure.ServiceBus.Serialization;

namespace ProjectExtensions.Azure.ServiceBus {

    /// <summary>
    /// Class used for configuration
    /// </summary>
    public class BusConfiguration : ProjectExtensions.Azure.ServiceBus.IBusConfiguration {

        static object lockObject = new object();
        static BusConfiguration configuration;

        IContainer container;
        List<Assembly> registeredAssemblies = new List<Assembly>();
        List<Type> registeredSubscribers = new List<Type>();

        /// <summary>
        /// ctor
        /// </summary>
        internal BusConfiguration() {
            MaxThreads = 1;
            TopicName = "pro_ext_topic";
        }

        /// <summary>
        /// The Service Bus
        /// </summary>
        public IBus Bus {
            get {
                return container.Resolve<IBus>();
            }
        }

        /// <summary>
        /// The IOC Container
        /// </summary>
        public static IContainer Container {
            get {
                return configuration.container;
            }
        }

        /// <summary>
        /// DefaultSerializer
        /// </summary>
        public IServiceBusSerializer DefaultSerializer {
            get {
                return container.Resolve<IServiceBusSerializer>();
            }
        }

        /// <summary>
        /// Instance of BusConfiguration
        /// </summary>
        public static BusConfiguration Instance {
            get {
                //TODO should this throw an exception if it was not created?
                return configuration;
            }
        }

        /// <summary>
        /// Max Threads to call the message handlers from the bus messages being received
        /// </summary>
        public byte MaxThreads {
            get;
            internal set;
        }

        /// <summary>
        /// List of RegisteredAssemblies
        /// </summary>
        public IList<Assembly> RegisteredAssemblies {
            get {
                return this.registeredAssemblies;
            }
        }

        /// <summary>
        /// List of RegisteredSubscribers
        /// </summary>
        public IList<Type> RegisteredSubscribers {
            get {
                return this.registeredSubscribers;
            }
        }

        /// <summary>
        /// ServiceBusApplicationId
        /// </summary>
        public string ServiceBusApplicationId {
            get;
            internal set;
        }

        /// <summary>
        /// ServiceBusNamespace
        /// </summary>
        public string ServiceBusNamespace {
            get;
            internal set;
        }

        /// <summary>
        /// ServiceBusIssuerName
        /// </summary>
        public string ServiceBusIssuerName {
            get;
            internal set;
        }

        /// <summary>
        /// ServiceBusIssuerKey
        /// </summary>
        public string ServiceBusIssuerKey {
            get;
            internal set;
        }

        /// <summary>
        /// ServicePath
        /// </summary>
        public string ServicePath {
            get;
            internal set;
        }

        /// <summary>
        /// TopicName
        /// </summary>
        public string TopicName {
            get;
            internal set;
        }

        internal void Configure(ContainerBuilder builder) {
            if (string.IsNullOrWhiteSpace(ServiceBusApplicationId)) {
                throw new ApplicationException("ApplicationId must be set.");
            }

            builder.Register<AzureBus>(item => new AzureBus(this)).As<IBus>().SingleInstance();

            builder.RegisterType<AzureBusReceiver>().As<IAzureBusReceiver>().SingleInstance();
            builder.RegisterType<AzureBusSender>().As<IAzureBusSender>().SingleInstance();

            container = builder.Build();

            //Set the Bus property so that the receiver will register the end points
            var prime = this.Bus;
        }

        /// <summary>
        /// Get the settings builder
        /// </summary>
        /// <returns></returns>
        public static BusConfigurationBuilder WithSettings() {
            return WithSettings(new Autofac.ContainerBuilder());
        }

        /// <summary>
        /// Get the settings build while passing in your IOC Container
        /// </summary>
        /// <param name="builder">Your IOC container</param>
        /// <returns></returns>
        public static BusConfigurationBuilder WithSettings(Autofac.ContainerBuilder builder) {
            if (builder == null) {
                throw new ArgumentNullException("builder");
            }
            if (configuration == null) {
                lock (lockObject) {
                    if (configuration == null) {
                        configuration = new BusConfiguration();
                    }
                }
            }
            //last one in wins so if one is registered it will be called.
            builder.RegisterType<JsonServiceBusSerializer>().As<IServiceBusSerializer>().SingleInstance();
            return new BusConfigurationBuilder(builder, configuration);
        }

        internal void AddRegisteredAssembly(Assembly value) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }
            if (!this.registeredAssemblies.Contains(value)) {
                this.registeredAssemblies.Add(value);
            }
        }

        internal void AddRegisteredSubscriber(Type value) {
            if (value == null) {
                throw new ArgumentNullException("value");
            }
            if (!this.registeredSubscribers.Contains(value)) {
                this.registeredSubscribers.Add(value);
            }
        }

    }
}
