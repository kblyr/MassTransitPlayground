namespace UserOrchestration.Consumers;

sealed class SendUserEmailVerificationConsumer : IConsumer<SendUserEmailVerification>
{
    readonly ILogger<SendUserEmailVerificationConsumer> _logger;
    readonly IMapper _mapper;

    public SendUserEmailVerificationConsumer(ILogger<SendUserEmailVerificationConsumer> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<SendUserEmailVerification> context)
    {
        _logger.LogInformation("Simulating sending an actual email");
        _logger.LogInformation("Wait for 2 seconds");
        await Task.Delay(2000);

        var @event = _mapper.Map<UserEmailVerificationSent>(context.Message);

        if (context.RequestId.HasValue)
            await context.RespondAsync(@event).ConfigureAwait(false);

        _logger.Event().Publishing<UserEmailVerificationSent>();
        await context.Publish(@event).ConfigureAwait(false);
    }
}
