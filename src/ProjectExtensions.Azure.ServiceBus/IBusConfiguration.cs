﻿using System;
using System.Collections.Generic;
using System.Reflection;
using ProjectExtensions.Azure.ServiceBus.Serialization;

namespace ProjectExtensions.Azure.ServiceBus {
    
    public interface IBusConfiguration {

        /// <summary>
        /// The Service Bus
        /// </summary>
        IBus Bus {
            get;
        }

        /// <summary>
        /// DefaultSerializer
        /// </summary>
        IServiceBusSerializer DefaultSerializer {
            get;
        }

        /// <summary>
        /// Max Threads to call the message handlers from the bus messages being received
        /// </summary>
        byte MaxThreads {
            get;
        }

        /// <summary>
        /// List of RegisteredAssemblies
        /// </summary>
        IList<Assembly> RegisteredAssemblies {
            get;
        }

        /// <summary>
        /// List of RegisteredSubscribers
        /// </summary>
        IList<Type> RegisteredSubscribers {
            get;
        }

        /// <summary>
        /// ServiceBusApplicationId
        /// </summary>
        string ServiceBusApplicationId {
            get;
        }

        /// <summary>
        /// ServiceBusNamespace
        /// </summary>
        string ServiceBusIssuerKey {
            get;
        }

        /// <summary>
        /// ServiceBusIssuerName
        /// </summary>
        string ServiceBusIssuerName {
            get;
        }

        /// <summary>
        /// ServiceBusIssuerKey
        /// </summary>
        string ServiceBusNamespace {
            get;
        }

        /// <summary>
        /// ServicePath
        /// </summary>
        string ServicePath {
            get;
        }

        /// <summary>
        /// TopicName
        /// </summary>
        string TopicName {
            get;
        }
    }
}
