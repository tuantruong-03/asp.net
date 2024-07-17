using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.request;
using api.DTOs.response;
using api.Models;

namespace api.Mappers
{
    public static class StockMapper
    {
        public static StockDTO ToStockDTO(this Stock stock) {
            return new StockDTO {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                MarketCap = stock.MarketCap,
                Industry = stock.Industry,
                LastDiv = stock.LastDiv,
                Purchase = stock.Purchase,
                Comments = stock.Comments.Select(c => c.ToCommentDTO()).ToList()
            };
        }
        public static Stock ToStockFromCreateStockDTO(this CreateStockDTO createStockDTO) {
            return new Stock {
                Symbol = createStockDTO.Symbol,
                CompanyName = createStockDTO.CompanyName,
                Purchase = createStockDTO.Purchase,
                LastDiv = createStockDTO.LastDiv,
                Industry = createStockDTO.Industry,
                MarketCap = createStockDTO.MarketCap,
            };
        }
    }
}