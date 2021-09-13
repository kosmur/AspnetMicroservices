using System.Threading.Tasks;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
    interface ICouponRepository
    {
        Task<Coupon> GetCoupon(string productName);

        Task<bool> CreateDiscount(Coupon coupon);

        Task<bool> UpdateDiscount(Coupon coupon);

        Task<bool> DeleteDiscount(string productName);
    }
}