namespace MediatorTest.Consumers;

sealed class AddProductConsumer : IConsumer<AddProduct>
{
    readonly ILogger<AddProductConsumer> _logger;
    readonly IDbContextFactory<ProductDbContext> _contextFactory;
    readonly IMapper _mapper;

    public AddProductConsumer(ILogger<AddProductConsumer> logger, IDbContextFactory<ProductDbContext> contextFactory, IMapper mapper)
    {
        _logger = logger;
        _contextFactory = contextFactory;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AddProduct> context)
    {
        using var dbContext = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        using var transaction = await dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);

        if (await dbContext.Products.NameExistsAsync(context.Message.Name).ConfigureAwait(false))
        {
            var responseMessage = _mapper.Map<AddProductFailed>(_mapper.Map<ProductNameAlreadyExists>(context.Message));
            await context.RespondAsync(responseMessage).ConfigureAwait(false);
            return;
        }

        var product = _mapper.Map<Product>(context.Message);
        var sendEndpoint = await context.GetSendEndpoint(new Uri(""));
        // context.Send();

        await dbContext.SaveChangesAsync().ConfigureAwait(false);
        await transaction.CommitAsync().ConfigureAwait(false);
    }
}
