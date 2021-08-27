using Game.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Services
{
    public interface IBoardService
    {
        Task<bool> CreateBoardAsync(Board board);

        Task<List<Board>> GetBoardsAsync();

        Task<Board> GetBoardByIdAsync(Guid Id);

        Task<bool> EnterBoardByIdAsync(Board boardToUpdate);

        Task<bool> UpdateBoardAsync(Board boardToUpdate);

        Task<bool> DeleteBoardAsync(Guid boardId);

        Task<bool> UserOwnsBoardsAsync(Guid boardId, string getUserId);
       
    }
}
