using E_Commerce.Domain.Aggregates;
using E_Commerce.Domain.Constants;

namespace E_Commerce.Domain.Extensions
{
    public static class OrderExtensions
    {
        public static void CheckOnStatus(this Order order, OrderStatus orderStatus)
        {
            if (order._status == OrderStatus.Pending && (orderStatus == OrderStatus.Cancelled || orderStatus == OrderStatus.Confirmed))
            {
                return;
            }
            if (order._status == OrderStatus.Confirmed && (orderStatus == OrderStatus.Shipped || orderStatus == OrderStatus.Cancelled))
            {
                return;
            }
            if (order._status == OrderStatus.Shipped && (orderStatus == OrderStatus.Delivered))
            {
                return;
            }
        }
    }
}
