using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using api.DTOs.request;
using api.Extensions;
using api.Filters;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/comments")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;
        private readonly UserManager<AppUser> userManager;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository
        , UserManager<AppUser> userManager)
        {

            this.commentRepository = commentRepository;
            this.stockRepository = stockRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        // [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> GetAll()
        {
     
            var comments = await commentRepository.GetAllAsync();
            var commontDTOs = comments.Select(comment => comment.ToCommentDTO());
            return Ok(commontDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDTO createCommentDTO)
        {
       
            if (!await stockRepository.IsStockExisted(stockId))
            {
                return BadRequest("Stock doesn't exist");
            }
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            var commentModel = createCommentDTO.ToModelFromCreateDTO(stockId);
            commentModel.AppUserId = appUser.Id;

            await commentRepository.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDTO updateCommentDTO)
        {
             if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await commentRepository.UpdateAsync(id, updateCommentDTO.ToModelFromUpdateDTO());
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
             if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comment = await commentRepository.DeleteAsync(id);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment);
        }
    }
}