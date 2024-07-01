using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.request;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock existingStock)
        {
            await _context.Stocks.AddAsync(existingStock);
            await _context.SaveChangesAsync();
            return existingStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (existingStock == null) {
                return null;
            }
            _context.Stocks.Remove(existingStock);
            await _context.SaveChangesAsync();
            return existingStock;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockDTO updateStockDTO)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (existingStock == null) {
                return null;
            }
            // DBContext will link "existingStock" to "stock" table in SQLServer
            // What changes in "existingStock" here will change the "stock" table in SQLServer 
            // So we need to SaveChanges(), not  Add()          
            existingStock.Symbol = updateStockDTO.Symbol;
            existingStock.CompanyName = updateStockDTO.CompanyName;
            existingStock.Purchase = updateStockDTO.Purchase;
            existingStock.LastDiv = updateStockDTO.LastDiv;
            existingStock.Industry = updateStockDTO.Industry;
            existingStock.MarketCap = updateStockDTO.MarketCap;
            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}