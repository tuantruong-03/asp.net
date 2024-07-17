using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext applicationDBContext) 
        {
            this._context = applicationDBContext;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Porfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeleteAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Porfolios.FirstOrDefaultAsync(p => p.AppUserId == appUser.Id 
                                && p.Stock.Symbol.ToLower().Equals(symbol.ToLower()));
            if (portfolioModel == null) {
                return null;
            }
            _context.Porfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stock>> GetStocksOfUser(AppUser user)
        {
            return await _context.Porfolios.Where(p => p.AppUserId == user.Id)
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