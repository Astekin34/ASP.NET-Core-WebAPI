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