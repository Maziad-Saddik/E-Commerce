using Azure.Messaging.ServiceBus;
using E_Commerce.Domain.Queries.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Context;
using Serilog.Core.Enrichers;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Queries.Services.ServiceBus
{
    public class ServiceBusListener : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<ServiceBusListener> _logger;

        private readonly ServiceBusSessionProcessor _processor;

        private readonly ServiceBusProcessor _deadLetterProcessor;

        private readonly ServiceBusOptions _serviceBusOptions;

        public ServiceBusListener(
            IOptions<ServiceBusOptions> serviceBusOptions,

            ILogger<ServiceBusListener> logger,

            ServiceBus serviceBus,

            IServiceProvider serviceProvider
        )
        {
            _logger = logger;

            _serviceProvider = serviceProvider;

            _serviceBusOptions = serviceBusOptions.Value;

            _processor = serviceBus.Client.CreateSessionProcessor(
                topicName: _serviceBusOptions.TopicName,
                subscriptionName: _serviceBusOptions.SubscriptionName,
                options: new ServiceBusSessionProcessorOptions
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 1,
                    MaxConcurrentSessions = 100,
                    MaxConcurrentCallsPerSession = 1
                });

            _processor.ProcessMessageAsync += Processor_ProcessMessageAsync;

            _processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

            _deadLetterProcessor = serviceBus.Client.CreateProcessor(
                topicName: _serviceBusOptions.TopicName,
                subscriptionName: _serviceBusOptions.SubscriptionName,
                options: new ServiceBusProcessorOptions()
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 10,
                    MaxConcurrentCalls = 10,
                    SubQueue = SubQueue.DeadLetter,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            _deadLetterProcessor.ProcessMessageAsync += DeadLetterProcessor_ProcessMessageAsync;

            _deadLetterProcessor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        }

        private async Task DeadLetterProcessor_ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            bool isHandled = await HandleAsync(args.Message);

            if (isHandled)
            {
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                _logger.LogWarning("Message {MessageId} not handled", args.Message.MessageId);

                await Task.Delay(5000);

                await args.AbandonMessageAsync(args.Message);
            }
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError("Message {MessageId} not handled", args.ErrorSource);

            return Task.CompletedTask;
        }

        private async Task Processor_ProcessMessageAsync(ProcessSessionMessageEventArgs args)
        {
            bool isHandled = await HandleAsync(args.Message);

            if (isHandled)
            {
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                _logger.LogWarning("Message {MessageId} not handled", args.Message.MessageId);

                await Task.Delay(5000);

                await args.AbandonMessageAsync(args.Message);
            }
        }

        private async Task<bool> HandleAsync(ServiceBusReceivedMessage message)
        {
            var eventType = new PropertyEnricher(name: "EventType", message.Subject);

            var sessionId = new PropertyEnricher(name: "SessionId", message.SessionId);

            var messageId = new PropertyEnricher(name: "messageId", message.MessageId);

            using (LogContext.Push(eventType, sessionId, messageId))
            {
                _logger.LogInformation("Started handling event .");

                var json = Encoding.UTF8.GetString(message.Body) ?? throw new InvalidOperationException("Failed to deserialize message");

                using var scope = _serviceProvider.CreateScope();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                Event @event = JsonDocument.Parse(json).ToEvent(message.Subject, _logger);

                var isHandled = await mediator.Send(@event);

                _logger.LogInformation("Event handling completed, Result: {Result}", isHandled);

                return isHandled;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _processor.StartProcessingAsync(cancellationToken);

            if (_serviceBusOptions.EnableDeadLetter)
                await _deadLetterProcessor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.CloseAsync(cancellationToken);

            if (_serviceBusOptions.EnableDeadLetter)
                await _deadLetterProcessor.CloseAsync(cancellationToken);
        }
    }
}