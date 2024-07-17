using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("/api/v1/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController: ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IStockRepository stockRepository;
        private readonly IPortfolioRepository portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            this.userManager = userManager;
            this.stockRepository = stockRepository;
            this.portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocksOfUser () {
            var username = User.GetUsername(); // ControllerBase.User
            var appUser = await userManager.FindByNameAsync(username);
            System.Console.WriteLine(appUser);
            var userPortfolios = await portfolioRepository.GetStocksOfUser(appUser);
            return Ok(userPortfolios);
        }
        [HttpPost]
        public async Task<IActionResult> AddStockToPortfolio(string symbol){
            var username = User.GetUsername();
            var appUser= await userManager.FindByNameAsync(username);
            var stock = await stockRepository.GetBySymbolAsync(symbol);
            if (stock == null) return BadRequest("Stock not found");
            var existingStock = await portfolioRepository.GetStocksOfUser(appUser);
            if (existingStock.Any(s => s.Symbol.ToLower().Equals(symbol.ToLower()))) return BadRequest("Cannot add the same portfolio");
            var portfolioModel = new Portfolio {
                StockId = stock.Id,
                AppUserId = appUser.Id,
            };
            await portfolioRepository.CreateAsync(portfolioModel);
            if (portfolioModel == null) {
                return StatusCode(500, "Could not create");
            } else {
                return Created();
            }
        }   
        [HttpDelete]
        public async Task<IActionResult> DeletPortfolio(string symbol) {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            var stocks = await portfolioRepository.GetStocksOfUser(appUser);
            var stockToDelete = stocks.Where( s=> s.Symbol.ToLower().Equals(symbol.ToLower()));
            if (stockToDelete.Count() ==1){
                await portfolioRepository.DeleteAsync(appUser, symbol);
                return NoContent();
            } else {
                return BadRequest("Stock is not in your portfolio");
            }
        }

    }
}