using CoffeeManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.DAO
{
    public class OrderDAO : IOrderDAO
    {
        private readonly CoffeeManagementDbContext _context;

        public OrderDAO(CoffeeManagementDbContext context)
        {
            _context = context;
        }
        // Example: CoffeeManagement.DAL.DAO.OrderDAO.CreateOrder
        public void CreateOrder(Order order, List<OrderItem> orderItems)
        {
            try
            {
                using (var ctx = new CoffeeManagementDbContext()) // create new context inside DAO
                {
                    // Build a new Order entity (do not use entities tracked by caller)
                    var newOrder = new Order
                    {
                        CustomerId = order.CustomerId,
                        StaffId = order.StaffId,
                        CreatedAt = order.CreatedAt,
                        Status = order.Status,
                        IsPaid = order.IsPaid,
                        Note = order.Note,
                        Subtotal = order.Subtotal,
                        DiscountAmount = order.DiscountAmount,
                        PromotionId = order.PromotionId,
                        TotalAmount = order.TotalAmount,
                        OrderItems = new List<OrderItem>()
                    };

                    foreach (var oi in orderItems)
                    {
                        // Validate MenuItemId exists (optional, but helpful)
                        var menuExists = ctx.MenuItems.Any(m => m.Id == oi.MenuItemId);
                        if (!menuExists)
                            throw new InvalidOperationException($"MenuItem with Id {oi.MenuItemId} does not exist.");

                        var newOi = new OrderItem
                        {
                            MenuItemId = oi.MenuItemId,
                            Quantity = oi.Quantity,
                            UnitPrice = oi.UnitPrice
                        };
                        newOrder.OrderItems.Add(newOi);
                    }

                    ctx.Orders.Add(newOrder);
                    ctx.SaveChanges();
                }
            }
            catch (DbUpdateException dbEx)
            {
                // Include inner details to help debug
                var inner = dbEx.InnerException;
                var msg = new System.Text.StringBuilder();
                msg.AppendLine("DbUpdateException when creating order: " + dbEx.Message);
                while (inner != null)
                {
                    msg.AppendLine("  Inner: " + inner.Message);
                    inner = inner.InnerException;
                }
                System.Diagnostics.Debug.WriteLine(msg.ToString());
                throw new Exception("Error when creating order: " + dbEx.Message + ". See Output for details.", dbEx);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CreateOrder failed: " + ex);
                throw;
            }
        }


        public bool DeleteOrder(int orderId)
        {
            var order = _context.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                return _context.SaveChanges() > 0;
            }

            return false;
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        public Order? GetOrderByCustomerId(int customerId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(o => o.CustomerId == customerId);
        }

        public Order? GetOrderById(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .FirstOrDefault(o => o.Id == orderId);
        }

        public Order? GetOrderByStaffId(int staffId)
        {
            return _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(o => o.StaffId == staffId);
        }

        public void UpdateOrderPaymentStatus(int orderId, bool isPaid, DateTime? paidAt)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.IsPaid = isPaid;
                order.PaidAt = paidAt;
                order.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void UpdateOrderStatus(int orderId, byte status)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                order.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
        }
        public void UpdateOrderStaff(int orderId, int staffId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.StaffId = staffId;
                _context.SaveChanges();
            }
        }

    }
}
