using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stocks;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stocks> CreateAsync(Stocks stockModel)
        {
            await _context.Stocks
        }

        public async Task<Stocks?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Stocks>> GetAllAsync()
        {
            return await _context.Stock.ToListAsync();
        }

        public async Task<Stocks?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Stocks?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            throw new NotImplementedException();
        }
    }
}