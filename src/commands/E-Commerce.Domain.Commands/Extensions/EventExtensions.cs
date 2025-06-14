﻿using E_Commerce.Domain.Commands;
using E_Commerce.Domain.Constants;
using E_Commerce.Domain.Events;
using E_Commerce.Domain.Events.Data;

namespace E_Commerce.Domain.Extensions
{
    public static class EventExtensions
    {
        public static OrderPlaced ToOrderPlaced(this PlaceOrderCommand Command, OrderStatus orderStatus = OrderStatus.Pending) => new()
        {
            AggregateId = Command.OrderId,
            Sequence = 1,
            DateTime = DateTime.UtcNow,
            Version = 1,
            UserId = Command.UserId,
            Data = new OrderPlacedData
            {
                Customer = Command.Customer,
                OrderItems = Command.OrderItems,
                OrderStatus = orderStatus,
            }
        };

        public static OrderCanceled ToOrderCanceled(this CancelOrderCommand Command) => new()
        {
            AggregateId = Command.OrderId,
            Sequence = 1,
            DateTime = DateTime.UtcNow,
            Version = 1,
            UserId = Command.UserId,
            Data = new()
        };
    }
}
