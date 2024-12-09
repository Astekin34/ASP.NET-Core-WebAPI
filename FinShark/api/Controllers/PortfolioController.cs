using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly IStockRepository _stockrepo;
        private readonly IPortfolioRepository _portfoliorepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _usermanager = userManager;
            _stockrepo = stockRepo;
            _portfoliorepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var AppUser = await _usermanager.FindByNameAsync(username);
            var userPortfolio = await _portfoliorepo.GetUserPortfolio(AppUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var AppUser = await _usermanager.FindByNameAsync(username);
            var stock = await _stockrepo.GetBySymbolAsync(symbol);

            if (stock == null) return BadRequest("Stock not found");
            var userPortfolio = await _portfoliorepo.GetUserPortfolio(AppUser);
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to the portfolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = AppUser.Id
            };

            await _portfoliorepo.CreateAsync(portfolioModel);

            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var AppUser = await _usermanager.FindByNameAsync(username);

            var userPortfolio = await _portfoliorepo.GetUserPortfolio(AppUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1)
            {
                await _portfoliorepo.DeletePortfolio(AppUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your portfolio");
            }

            return Ok();
        }
    }
}