using Game.Contracts.V1;
using Game.Contracts.V1.Requests;
using Game.Contracts.V1.Responses;
using Game.Domain;
using Game.Extensions;
using Game.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Controllers.V1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin,Casual")]
    public class BoardController : Controller
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }


        [HttpGet(ApiRoutes.Boards.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _boardService.GetBoardsAsync());
        }

        [HttpDelete(ApiRoutes.Boards.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid boardId)
        {

            /*Kto usuwać może
             * var userOwnsBoard = await _boardService.UserOwnsBoardsAsync(boardId, HttpContext.GetUserId());

            if (!userOwnsBoard)
            {
                return BadRequest(new { error = "You dont own this" });
            }*/

            var deleted = await _boardService.DeleteBoardAsync(boardId);


            if (deleted)
                return NoContent();
            return NotFound();
        }

        [HttpPut(ApiRoutes.Boards.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid boardId, [FromBody]UpdateBoardRequest request)
        {
            
            var userOwnsBoard = await _boardService.UserOwnsBoardsAsync(boardId, HttpContext.GetUserId());

            if (!userOwnsBoard)
            {
                return BadRequest(new { error = "You dont own this board" });
            }
            

            var board = await _boardService.GetBoardByIdAsync(boardId);
            board.IsClosed = request.IsClosed;

            var updated = await _boardService.UpdateBoardAsync(board);

            if (updated)
                return Ok(board);

            return NotFound();
        }


        [HttpPut(ApiRoutes.Boards.UpdateCurrUsers)]
        public async Task<IActionResult> UpdateCurrUsers([FromRoute]Guid boardId, [FromBody]UpdateCurrentPlayersInBoard request)
        {

            var userOwnsBoard = await _boardService.UserOwnsBoardsAsync(boardId, HttpContext.GetUserId());

            if (!userOwnsBoard)
            {
                return BadRequest(new { error = "You dont own this board" });
            }


            var board = await _boardService.GetBoardByIdAsync(boardId);
            board.CurrentNumberOfPlayers += request.Count;

            var updated = await _boardService.UpdateBoardAsync(board);

            if (updated)
                return Ok(board);

            return NotFound();
        }




        [HttpGet(ApiRoutes.Boards.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid boardId)
        {
            var board = await _boardService.GetBoardByIdAsync(boardId);

            if (board == null)
            {
                return NotFound();
            }

            return Ok(board);
        }


        [HttpPost(ApiRoutes.Boards.Create)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBoardRequest boardRequest)
        {
            var board = new Board
            {
                Name = boardRequest.Name,
                AdminUserId = HttpContext.GetUserId(),
                MaxNumberOfPlayers = boardRequest.MaxNumberOfPlayers


            };

            await _boardService.CreateBoardAsync(board);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Boards.Get.Replace("{boardId}", board.Id.ToString());
            var response = new BoardResponse { Id = board.Id };
            return Created(locationUrl, board);
        }
    }
}
