using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.DTOs.Stocks;
using api.Helpers;
using api.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stocks>> GetAllAsync(QueryObject query);
        Task<Stocks?> GetByIdAsync(int id);
        Task<Stocks?> GetBySymbolAsync(string symbol);
        Task<Stocks> CreateAsync(Stocks stockModel);
        Task<Stocks?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stocks?> DeleteAsync(int id);
        Task<bool> StockExists(int id);
    }
}