using CoffeeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.DAO
{
    public interface IPromotionDAO
    {
        Promotion? GetPromotionByCode(string code);
        List<Promotion> GetAllPromotion();
        void AddPromotion(Promotion promotion);
        void DeletePromotion(int id);
        void UpdatePromotion(Promotion promotion);
        void IncrementUsage(int id);

    }
}
