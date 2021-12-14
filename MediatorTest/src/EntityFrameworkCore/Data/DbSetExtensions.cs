namespace MediatorTest.Data;

static class DbSetExtensions
{
    public static async Task<bool> NameExistsAsync(this DbSet<Product> products, string name, CancellationToken cancellationToken = default) => await products
        .AsNoTracking()
        .Where(product => product.Name == name)
        .AnyAsync(cancellationToken)
        .ConfigureAwait(false);
}