using CoffeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.DAO
{
    public interface IOrderDAO
    {
        void CreateOrder(Order order, List<OrderItem> orderItems);
        Order? GetOrderById(int orderId);
        List<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, byte status);
        void UpdateOrderPaymentStatus(int orderId, bool isPaid, DateTime? paidAt);
        bool DeleteOrder(int orderId);
        Order? GetOrderByStaffId(int staffId);
        Order? GetOrderByCustomerId(int customerId);
        void UpdateOrderStaff(int orderId, int staffId);
    }
}
