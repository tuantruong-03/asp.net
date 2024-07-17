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
        public async Task<IActionResult> GetUserPortfolios () {
            var username = User.GetUsername(); // ControllerBase.User
            var appUser = await userManager.FindByNameAsync(username);
            System.Console.WriteLine(appUser);
            var userPortfolios = await portfolioRepository.GetUserPortfolios(appUser);
            return Ok(userPortfolios);
        }
    }
}