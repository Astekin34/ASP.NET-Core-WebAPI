using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;
        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.stocks.Symbol.ToLower() == symbol.ToLower());
            if (portfolioModel == null)
            {
                return null;
            }

            _context.Portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;
        }

        public async Task<List<Stocks>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stocks
            {
                Id = stock.StockId,
                Symbol = stock.stocks.Symbol,
                CompanyName = stock.stocks.CompanyName,
                Purchase = stock.stocks.Purchase,
                LastDiv = stock.stocks.LastDiv,
                Industry = stock.stocks.Industry,
                MarketCap = stock.stocks.MarketCap
            }).ToListAsync();
        }
    }
}