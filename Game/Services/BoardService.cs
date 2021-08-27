
using Game.Data;
using Game.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Services
{
    public class BoardService : IBoardService
    {
        private readonly DataContext _dataContext;

        public BoardService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeleteBoardAsync(Guid boardId)
        {
            var board = await GetBoardByIdAsync(boardId);
            if (board == null)
            {
                return false;
            }
            _dataContext.Boards.Remove(board);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> CreateBoardAsync(Board board)
        {

            await _dataContext.Boards.AddAsync(board);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0;
        }



        public async Task<Board> GetBoardByIdAsync(Guid boardId)
        {

            return await _dataContext.Boards.SingleOrDefaultAsync(x => x.Id == boardId);
        }

        public async Task<List<Board>> GetBoardsAsync()
        {
            return await _dataContext.Boards.ToListAsync(); ;
        }

        public async Task<bool> UpdateBoardAsync(Board boardToUpdate)
        {

            if (boardToUpdate == null)
            {
                return false;
            }
            _dataContext.Boards.Update(boardToUpdate);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0;
        }


        public async Task<bool> UserOwnsBoardsAsync(Guid boardId, string getUserId)
        {
          var board = await _dataContext.Boards.AsNoTracking().SingleOrDefaultAsync(x => x.Id == boardId);

            if (board == null)
            {
                return false;
            }

            if (board.AdminUserId != getUserId)
            {
                return false;
            }
            return true;
        }

        public Task<bool> EnterBoardByIdAsync(Board boardToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
