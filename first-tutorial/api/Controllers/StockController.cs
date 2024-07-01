using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.request;
using api.DTOs.response;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("v1/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            this._context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            // Select like "map" in Java

            var stocks = await _stockRepository.GetAllAsync();
            var stockDTOs = stocks.Select(stock => StockMapper.ToStockDTO(stock));
            // .Select(stock => stock.ToStockDTO()); // Extension method
            
            return Ok(stockDTOs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null) {
                return NotFound();
            }
            return Ok(stock.ToStockDTO()); 
            // "stock.ToStockDTO()" is extension method
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDTO createStockDTO) {
            var stockModel = createStockDTO.ToStockFromCreateStockDTO();
            await _stockRepository.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, stockModel);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDTO updateStockDTO) {
            var stockModel = await _stockRepository.UpdateAsync(id, updateStockDTO);
            if (stockModel == null) {
                return NotFound();
            }
            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            var stockModel = await _stockRepository.DeleteAsync(id);
            if (stockModel == null) {
                return NotFound();
            }
            return NoContent();
        }
    }
}