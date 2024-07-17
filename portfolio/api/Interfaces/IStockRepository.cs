using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.request;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock?> GetByIdAsync(int id); //FirstOrDefault
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockDTO updateStockDTO);
        Task<Stock?> DeleteAsync(int id);

        Task<bool> IsStockExisted(int id);
    }
}