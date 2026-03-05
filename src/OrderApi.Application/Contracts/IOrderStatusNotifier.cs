using OrderApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Contracts;

public interface IOrderStatusNotifier
{
    Task NotifyStatusChangedAsync(Guid orderId, OrderStatus status, int version);
}
