using System;
using System.Threading.Tasks;
using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public DiscountRepository(IConfiguration configuraiton)
        {
            _configuration = configuraiton ?? throw new ArgumentNullException(nameof(configuraiton)); 
            connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
           using var connection = new NpgsqlConnection(connectionString);
           var affected = await connection.ExecuteAsync
            ("insert into Coupon(ProductName, Description, Amount) values (@ProductName, @Description, @Amount)",
            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var connection = Connection();
            var affected = await connection.ExecuteAsync("delete from Coupon where ProductName=@ProductName",
            new { ProductName = productName });

            return affected > 0;
        }

        public async Task<Coupon> GetCoupon(string productName)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("select * from Coupon where ProductName = @ProductName", new { ProductName = productName });
            
            if (coupon == null)
            {
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }
            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var affected = await connection.ExecuteAsync
            ("update set ProductName=@ProductName, Description=@Description, Amoun=@Amount where Id=@Id",
            new { ProducName = coupon.ProductName, Descirption = coupon.Description, Amount = coupon.Amount });

            return affected > 0;
        }

        private NpgsqlConnection Connection()
        {
            var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            using var connection = new NpgsqlConnection(connectionString);
            return connection;
        }
    }
}