using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.request;
using api.DTOs.response;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel) {
            return new CommentDTO {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreateOn = commentModel.CreateOn,
                StockId = commentModel.StockId,
            };
        }

        public static Comment ToModelFromCreateDTO(this CreateCommentDTO createCommentDTO, int stockId) {
            return new Comment {
                Title = createCommentDTO.Title,
                Content = createCommentDTO.Content,
                StockId = stockId,
            };
        }

        public static Comment ToModelFromUpdateDTO(this UpdateCommentDTO updateCommentDTO) {
            return new Comment {
                Title = updateCommentDTO.Title,
                Content = updateCommentDTO.Content,
            };
        }
    }
}