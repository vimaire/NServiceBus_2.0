using NServiceBus.Unicast;
using NServiceBus.Unicast.Transport;
using NServiceBus.Unicast.Transport.Msmq;
using NUnit.Framework;
using NBehave.Spec.NUnit;

namespace NServiceBus.Host.Tests
{
     
    [TestFixture]
    public class When_configuring_an_endpoint_as_a_server
    {
        //private MsmqTransport transport;
        //private Configure configure;
        
        //[SetUp]
        //public void SetUp()
        //{
        //    configure = Util.Init<ServerEndpointConfig>();

        //    transport = configure.Builder.Build<ITransport>() as MsmqTransport;
        //}

        //[Test]
        //public void Transport_should_be_msmq()
        //{
        //    transport.ShouldNotBeNull();
        //}

        //[Test]
        //public void Transport_should_be_transactional()
        //{
        //    transport.IsTransactional.ShouldBeTrue();
        //}

        //[Test]
        //public void Transport_should_not_be_purged_on_startup()
        //{
        //    transport.PurgeOnStartup.ShouldBeFalse();
        //}

     
        //[Test]
        //public void The_bus_should_impersonate_the_sender()
        //{
        //    var unicastbus = configure.Builder.Build<UnicastBus>();
            
        //    unicastbus.ShouldNotBeNull();
        //    unicastbus.ImpersonateSender.ShouldBeTrue();
        //}



    }
}