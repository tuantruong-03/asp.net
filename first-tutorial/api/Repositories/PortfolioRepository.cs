using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext applicationDBContext1;
        public PortfolioRepository(ApplicationDBContext applicationDBContext) 
        {
            this.applicationDBContext1 = applicationDBContext;
        }
        public async Task<List<Stock>> GetUserPortfolios(AppUser user)
        {
            return await applicationDBContext1.Porfolios.Where(p => p.AppUserId == user.Id)
            .Select(p => new Stock{
                Id= p.StockId,
                Symbol = p.Stock.Symbol,
                CompanyName = p.Stock.CompanyName,
                Purchase = p.Stock.Purchase,
                LastDiv = p.Stock.LastDiv,
                Industry = p.Stock.Industry,
                MarketCap = p.Stock.MarketCap,
            }).ToListAsync();
        }
    }
}