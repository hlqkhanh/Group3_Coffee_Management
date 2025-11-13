using CoffeeManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeManagement.DAL.DAO
{
    public class PromotionDAO : IPromotionDAO
    {
        // SỬA LỖI 1: Xóa ' = new()'
        private readonly CoffeeManagementDbContext _context;

        // SỬA LỖI 1: Thêm Constructor để NHẬN DbContext
        // (Giống hệt cách OrderDAO.cs và UserDAO.cs đang làm)
        public PromotionDAO(CoffeeManagementDbContext context)
        {
            _context = context;
        }

        public void AddPromotion(Promotion promotion)
        {
            try
            {
                _context.Promotions.Add(promotion);

                // SỬA LỖI 2: Thêm SaveChanges()
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo Promotion: {ex.Message}");
            }
        }

        public void DeletePromotion(int id)
        {
            try
            {
                var promo = _context.Promotions.Find(id);
                if (promo != null)
                {
                    _context.Promotions.Remove(promo);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public List<Promotion> GetAllPromotion()
        {
            return _context.Promotions.ToList();
        }

        public Promotion? GetPromotionByCode(string code)
        {
            // CẢI TIẾN: Thêm 'OrdinalIgnoreCase'
            // để tìm mã "SALE20" và "sale20" như nhau
            return _context.Promotions
                   .FirstOrDefault(p => p.Code.Equals(code));
        }

        public void UpdatePromotion(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            _context.SaveChanges();
        }

        public void IncrementUsage(int id)
        {
            try
            {
                var promo = _context.Promotions.Find(id);
                if (promo != null)
                {
                    promo.CurrentUsage++;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tăng lượt dùng Promotion: {ex.Message}");
            }
        }
    }
}