﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Events;
using IO.Ably.Encryption;
using IO.Ably.Realtime;
using IO.Ably.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace IO.Ably.Core.Tests.Realtime
{
    [Collection("Channel SandBox")]
    [Trait("requires", "sandbox")]
    public class ChannelSandboxSpecs : SandboxSpecs
    {
        [Theory]
        [ProtocolData]
        public async Task TestGetChannel_ReturnsValidChannel(Protocol protocol)
        {
            // Arrange
            var client = await GetRealtimeClient(protocol);

            // Act
            IRealtimeChannel target = client.Channels.Get("test");

            // Assert
            target.Name.ShouldBeEquivalentTo("test");
            target.State.ShouldBeEquivalentTo(ChannelState.Initialized);
        }

        [Theory]
        [ProtocolData]
        public async Task TestAttachChannel_AttachesSuccessfuly(Protocol protocol)
        {
            // Arrange
            var client = await GetRealtimeClient(protocol);
            Semaphore signal = new Semaphore(0, 2);
            var args = new List<ChannelStateChange>();
            IRealtimeChannel target = client.Channels.Get("test");
            target.StateChanged += (s, e) =>
            {
                args.Add(e);
                signal.Release();
            };

            // Act
            target.Attach();

            // Assert
            signal.WaitOne(10000);
            args.Count.ShouldBeEquivalentTo(1);
            args[0].Current.ShouldBeEquivalentTo(ChannelState.Attaching);
            args[0].Error.ShouldBeEquivalentTo(null);
            target.State.ShouldBeEquivalentTo(ChannelState.Attaching);

            signal.WaitOne(10000);
            args.Count.ShouldBeEquivalentTo(2);
            args[1].Current.ShouldBeEquivalentTo(ChannelState.Attached);
            args[1].Error.ShouldBeEquivalentTo(null);
            target.State.ShouldBeEquivalentTo(ChannelState.Attached);
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL1")]
        public async Task SendingAMessageAttachesTheChannel_BeforeReceivingTheMessages(Protocol protocol)
        {
            Logger.LogLevel = LogLevel.Debug;
            ;
            // Arrange
            var client = await GetRealtimeClient(protocol);
            IRealtimeChannel target = client.Channels.Get("test");
            var messagesReceived = new List<Message>();
            target.Subscribe(message =>
            {
                messagesReceived.Add(message);
                _resetEvent.Set();
            });

            // Act
            target.Publish("test", "test data");
            target.State.Should().Be(ChannelState.Attaching);
            _resetEvent.WaitOne(6000);

            // Assert
            target.State.Should().Be(ChannelState.Attached);
            messagesReceived.Count.ShouldBeEquivalentTo(1);
            messagesReceived[0].Name.ShouldBeEquivalentTo("test");
            messagesReceived[0].Data.ShouldBeEquivalentTo("test data");
        }

        //TODO: RTL1 Spec about presence and sync messages

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL4e")]
        public async Task WhenAttachingAChannelWithInsufficientPermissions_ShouldSetItToFailedWithError(
            Protocol protocol)
        {
            var client = await GetRealtimeClient(protocol, (options, settings) =>
            {
                options.Key = settings.KeyWithChannelLimitations;
            });

            var channel = client.Channels.Get("nono_" + protocol);
            var result = await channel.AttachAsync();

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be(40160);
            result.Error.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


        [Theory]
        [ProtocolData]
        public async Task TestAttachChannel_Sending3Messages_EchoesItBack(Protocol protocol)
        {
            // Arrange
            var client = await GetRealtimeClient(protocol);

            ManualResetEvent resetEvent = new ManualResetEvent(false);
            IRealtimeChannel target = client.Channels.Get("test" + protocol);
            target.Attach();

            List<Message> messagesReceived = new List<Message>();
            int count = 0;
            target.Subscribe(message =>
            {
                messagesReceived.Add(message);
                count++;
                if (count == 3)
                    resetEvent.Set();
            });

            // Act
            target.Publish("test1", "test 12");
            target.Publish("test2", "test 123");
            target.Publish("test3", "test 321");

            bool result = resetEvent.WaitOne(8000);
            result.Should().BeTrue();

            // Assert
            messagesReceived.Count.ShouldBeEquivalentTo(3);
            messagesReceived[0].Name.ShouldBeEquivalentTo("test1");
            messagesReceived[0].Data.ShouldBeEquivalentTo("test 12");
            messagesReceived[1].Name.ShouldBeEquivalentTo("test2");
            messagesReceived[1].Data.ShouldBeEquivalentTo("test 123");
            messagesReceived[2].Name.ShouldBeEquivalentTo("test3");
            messagesReceived[2].Data.ShouldBeEquivalentTo("test 321");
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL7f")]
        public async Task TestAttachChannel_SendingMessage_Doesnt_EchoesItBack(Protocol protocol)
        {
            // Arrange
            var client = await GetRealtimeClient(protocol, (o, _) => o.EchoMessages = false);
            var target = client.Channels.Get("test");

            target.Attach();

            List<Message> messagesReceived = new List<Message>();
            target.Subscribe(message =>
            {
                messagesReceived.Add(message);
            });

            // Act
            await target.PublishAsync("test", "test data");

            // Assert
            messagesReceived.Should().BeEmpty();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6b")]
        public async Task With3ClientsAnd60MessagesAndCallbacks_ShouldExecuteAllCallbacks(Protocol protocol)
        {
            List<bool> successes = new List<bool>();
            var client1 = await GetRealtimeClient(protocol);
            var client2 = await GetRealtimeClient(protocol);
            var client3 = await GetRealtimeClient(protocol);
            var messages = new List<Message>();
            for (int i = 0; i < 20; i++)
            {
                messages.Add(new Message("name" + i, "data" + i));
            }

            foreach (var message in messages)
            {
                client1.Channels.Get("test").Publish(new[] { message }, (b, info) =>
               {
                   successes.Add(b);
               });
                client2.Channels.Get("test").Publish(new[] { message }, (b, info) =>
                {
                    successes.Add(b);
                });
                client3.Channels.Get("test").Publish(new[] { message }, (b, info) =>
                {
                    successes.Add(b);
                });
            }

            await Task.Delay(10000);
            successes.Where(x => x == true).Should().HaveCount(60, "Should have 60 successful callback executed");
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6e")]
        [Trait("spec", "RTL6e1")]
        public async Task WithBasicAuthAndAMessageWithClientId_ShouldReturnTheMessageWithThatClientID(Protocol protocol)
        {
            var client = await GetRealtimeClient(protocol);

            client.Connect();
            var channel = client.Channels.Get("test");
            bool messageReceived = false;
            channel.Subscribe(message =>
            {
                message.ClientId.Should().Be("123");
                messageReceived = true;
                _resetEvent.Set();
            });

            await channel.PublishAsync(new Message("test", "withClientId") { ClientId = "123" });

            _resetEvent.WaitOne(4000).Should().BeTrue("Operation timed out");

            messageReceived.Should().BeTrue();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6g1b")]
        public async Task WithAClientIdInOptions_ShouldReceiveMessageWithCorrectClientID(Protocol protocol)
        {
            var clientId = (new Random().Next(1, 100000) % 1000).ToString();
            var client = await GetRealtimeClient(protocol, (opts, _) => opts.ClientId = clientId);

            client.Connect();
            var channel = client.Channels.Get("test");
            int messagesReceived = 0;
            string receivedClienId = "";
            channel.Subscribe(message =>
            {
                receivedClienId = message.ClientId;
                Interlocked.Increment(ref messagesReceived);
            });

            await channel.PublishAsync(new Message("test", "withClientId"));
            receivedClienId.Should().Be(clientId);
            messagesReceived.Should().BeGreaterThan(0);
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6g1b")]
        public async Task WithAnImplicitClientIdFromToken_ShouldReceiveMessageWithCorrectClientID(Protocol protocol)
        {
            Logger.LogLevel = LogLevel.Debug;
            var rest = await GetRestClient(protocol);
            var token = await rest.Auth.RequestTokenAsync(new TokenParams() { ClientId = "1000" });
            var client = await GetRealtimeClient(protocol, (opts, _) => opts.TokenDetails = token);

            client.Connect();
            var channel = client.Channels.Get("test");
            bool messageReceived = false;
            channel.Subscribe(message =>
            {
                message.ClientId.Should().Be("1000");
                messageReceived = true;
            });

            await channel.PublishAsync(new Message("test", "withClientId"));
            messageReceived.Should().BeTrue();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6g2")]
        public async Task WithAClientIdInOptionsAndMatchingClientIdInMessage_ShouldSendAndReceiveMessageWithCorrectClientID(Protocol protocol)
        {
            var client = await GetRealtimeClient(protocol, (opts, _) => opts.ClientId = "999");

            client.Connect();
            var channel = client.Channels.Get("test");
            bool messageReceived = false;
            channel.Subscribe(message =>
            {
                message.ClientId.Should().Be("999");
                messageReceived = true;
                _resetEvent.Set();
            });

            await channel.PublishAsync(new Message("test", "data") { ClientId = "999" });
            _resetEvent.WaitOne(4000).Should().BeTrue("Timed out");

            messageReceived.Should().BeTrue();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6g2")]
        public async Task WithAClientIdInOptionsAndDifferentClientIdInMessage_ShouldNotSendMessageAndResultInAnError(Protocol protocol)
        {
            var client = await GetRealtimeClient(protocol, (opts, _) => opts.ClientId = "999");

            client.Connect();
            var channel = client.Channels.Get("test");
            bool messageReceived = false;
            channel.Subscribe(message =>
            {
                message.ClientId.Should().Be("999");
                messageReceived = true;
            });

            var result = await channel.PublishAsync(new Message("test", "data") { ClientId = "1000" });
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            messageReceived.Should().BeFalse();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6g4")]
        public async Task
            WhenPublishingMessageWithCompatibleClientIdBeforeClientIdHasBeenConfigured_ShouldPublishTheMessageSuccessfully
            (Protocol protocol)
        {
            var clientId = "client1";
            var rest = await GetRestClient(protocol);
            var realtimeClient = await GetRealtimeClient(protocol, (opts, _) =>
            {
                opts.AutoConnect = false;
                opts.AuthCallback = async @params => await rest.Auth.RequestTokenAsync(new TokenParams() { ClientId = clientId });
            });

            var channel = realtimeClient.Channels.Get("test");
            bool messageReceived = false;
            channel.Subscribe(message =>
            {
                messageReceived = true;
                message.ClientId.Should().Be(clientId);
            });

            await channel.PublishAsync(new Message("test", "best") { ClientId = "client1" });

            await Task.Delay(4000);
            messageReceived.Should().BeTrue();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6g4")]
        [Trait("spec", "RTL6g3")]
        [Trait("spec", "RTL6h")] //Look at PublishAsync
        public async Task
            WhenPublishingMessageWithInCompatibleClientIdBeforeClientIdHasBeenConfigured_ShouldPublishTheMessageAndReturnErrorFromTheServerAllowingFurtherMessagesToBePublished
            (Protocol protocol)
        {
            var clientId = "client1";
            var rest = await GetRestClient(protocol);
            var realtimeClient = await GetRealtimeClient(protocol, (opts, _) =>
            {
                opts.AutoConnect = false;
                opts.AuthCallback = async @params => await rest.Auth.RequestTokenAsync(new TokenParams() { ClientId = clientId });
            });

            var channel = realtimeClient.Channels.Get("test");
            bool messageReceived = false;
            channel.Subscribe(message =>
            {
                messageReceived = true;
                message.ClientId.Should().Be(clientId);
            });

            var result = await channel.PublishAsync("test", "best", "client2");
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();

            messageReceived.Should().BeFalse();

            //Send a followup message
            var followupMessage = await channel.PublishAsync("followup", "message");
            followupMessage.IsSuccess.Should().BeTrue();
            messageReceived.Should().BeTrue();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL6f")]
        public async Task ConnectionIdShouldMatchThatOfThePublisher(Protocol protocol)
        {
            var client = await GetRealtimeClient(protocol, (opts, _) => opts.ClientId = "999");
            var connectionId = client.Connection.Id;
            var channel = client.Channels.Get("test");
            bool messageReceived = false;
            channel.Subscribe(message =>
            {
                message.ConnectionId.Should().Be(connectionId);
                messageReceived = true;
                _resetEvent.Set();
            });

            await channel.PublishAsync(new Message("test", "best"));

            _resetEvent.WaitOne();
            messageReceived.Should().BeTrue();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL7c")]
        public async Task WhenSubscribingToAChannelWithInsufficientPermissions_ShouldSetItToFailedWithError(
            Protocol protocol)
        {
            var client = await GetRealtimeClient(protocol, (options, settings) =>
            {
                options.Key = settings.KeyWithChannelLimitations;
            });

            var channel = client.Channels.Get("nono");
            channel.Subscribe(message =>
            {
                //do nothing
            });

            var result = await new ChannelAwaiter(channel, ChannelState.Failed).WaitAsync();
            result.IsSuccess.Should().BeTrue();

            var error = channel.ErrorReason;
            error.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        public static IEnumerable<object[]> FixtureData
        {
            get
            {
                yield return new object[] { Protocol.MsgPack, GetAES128FixtureData() };
                yield return new object[] { Protocol.Json, GetAES128FixtureData() };
                yield return new object[] { Protocol.MsgPack, GetAES256FixtureData() };
                yield return new object[] { Protocol.Json, GetAES256FixtureData() };
            }
        }

        [Theory]
        [MemberData("FixtureData")]
        [Trait("spec", "RTL7d")]
        public async Task ShouldPublishAndReceiveFixtureData(Protocol protocol, JObject fixtureData)
        {
            Logger.LogLevel = LogLevel.Debug;
            var items = (JArray)fixtureData["items"];
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            var client = await GetRealtimeClient(protocol);

            var channel = client.Channels.Get("persisted:test".AddRandomSuffix(), GetOptions(fixtureData));
            var count = 0;
            Message lastMessage = null;
            channel.Subscribe(message =>
            {
                lastMessage = message;
                resetEvent.Set();
            });

            foreach (var item in items)
            {
                var encoded = item["encoded"];
                var encoding = (string)encoded["encoding"];
                var decodedData = DecodeData((string)encoded["data"], encoding);
                await channel.PublishAsync((string)encoded["name"], decodedData);
                var result = resetEvent.WaitOne(10000);
                result.Should().BeTrue("Operation timed out");
                if (lastMessage.Data is byte[])
                    (lastMessage.Data as byte[]).Should().BeEquivalentTo(decodedData as byte[], "Item number {0} data does not match decoded data", count);
                else if (encoding == "json")
                    JToken.DeepEquals((JToken)lastMessage.Data, (JToken)decodedData).Should().BeTrue("Item number {0} data does not match decoded data", count);
                else
                    lastMessage.Data.Should().Be(decodedData, "Item number {0} data does not match decoded data", count);
                count++;
                resetEvent.Reset();
            }
            client.Close();
        }

        [Fact]
        [Trait("bug", "102")]
        public async Task WhenAttachingToAChannelFromMultipleThreads_ItShouldNotThrowAnError()
        {
            Logger.LogLevel = LogLevel.Debug;

            var client1 = await GetRealtimeClient(Protocol.Json);
            var channel = client1.Channels.Get("test");
            var task = Task.Run(() => channel.Attach());
            task.ConfigureAwait(false);
            var task2 = Task.Run(() => channel.Attach());
            task2.ConfigureAwait(false);

            await Task.WhenAll(task, task2);

            var result = await new ChannelAwaiter(channel, ChannelState.Attached).WaitAsync();
            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [ProtocolData]
        [Trait("spec", "RTL10d")]
        public async Task WithOneClientPublishingAnotherShouldBeAbleToRetrieveMessages(Protocol protocol)
        {
            Logger.LogLevel = LogLevel.Debug;

            var client1 = await GetRealtimeClient(protocol);

            var channelName = "persisted:history".AddRandomSuffix();
            var channel = client1.Channels.Get(channelName);
            await channel.AttachAsync();
            var messages = Enumerable.Range(1, 10).Select(x => new Message("name:" + x, "value:" + x));
            await channel.PublishAsync(messages);

            await Task.Delay(2000);

            var client2 = await GetRealtimeClient(protocol);
            var historyChannel = client2.Channels.Get(channelName);
            var history = await historyChannel.HistoryAsync(new HistoryRequestParams() { Direction = QueryDirection.Forwards });

            history.Should().BeOfType<PaginatedResult<Message>>();
            history.Items.Should().HaveCount(10);
            for (int i = 0; i < 10; i++)
            {
                var message = history.Items.ElementAt(i);
                message.Name.Should().Be("name:" + (i + 1));
                message.Data.ToString().Should().Be("value:" + (i + 1));
            }
        }

        private static JObject GetAES128FixtureData()
        {
            return JObject.Parse(ResourceHelper.GetResource("crypto-data-128.json"));
        }

        private static JObject GetAES256FixtureData()
        {
            return JObject.Parse(ResourceHelper.GetResource("crypto-data-256.json"));
        }

        private ChannelOptions GetOptions(JObject data)
        {
            var key = ((string)data["key"]);
            var iv = ((string)data["iv"]);
            var cipherParams = new CipherParams("aes", key, CipherMode.CBC, iv);
            return new ChannelOptions(cipherParams);
        }

        private object DecodeData(string data, string encoding)
        {
            if (encoding == "json")
            {
                return JsonHelper.Deserialize(data);
            }
            if (encoding == "base64")
                return data.FromBase64();

            return data;
        }

        public ChannelSandboxSpecs(AblySandboxFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }
    }
}
