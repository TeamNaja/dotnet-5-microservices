namespace Discount.Entity.Repositories
{
    using Dapper;
    using Discount.Entity.Entities;
    using Microsoft.Extensions.Configuration;
    using Npgsql;
    using System.Threading;
    using System.Threading.Tasks;

    public class DiscountRepository : IDiscountRepository
    {
        private readonly string connectionString;

        public DiscountRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetSection("DatabaseSettings:ConnectionString").Value;
        }

        public async Task<Coupon> GetDiscountAsync(string productName, CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(this.connectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon is null)
            {
                return new Coupon() { ProductName = "No Discount", Amount = 0, Description = "No Discount Description" };
            }

            return coupon;
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(this.connectionString);

            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Amount, coupon.Description });

            return affected > 0;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(this.connectionString);

            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET ProductName = @ProductName, Amount = @Amount, Description = @Description WHERE Id = @Id",
                (coupon.ProductName, coupon.Amount, coupon.Description, coupon.Id));

            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName, CancellationToken cancellationToken = default)
        {
            using var connection = new NpgsqlConnection(this.connectionString);

            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            return affected > 0;
        }
    }
}
