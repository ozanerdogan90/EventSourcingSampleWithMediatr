using EventSourcingSampleWithCQRSandMediatr.Persistence.DataAccess;
using EventSourcingSampleWithCQRSandMediatr.Persistence.Entities;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Repositories
{
    public interface IGameRepository
    {
        Task<bool> CreateGame(Game game);
        Task<bool> EndGame(Guid id);
        Task<bool> StartGame(Guid id);
        Task<bool> AddScore(Score score);
        Task<IReadOnlyList<Score>> GetScores(Guid gameId);
        Task<IReadOnlyList<Card>> GetCards(Guid gameId);
        Task<bool> AddCard(Card card);
        Task<IReadOnlyList<Faul>> GetFauls(Guid gameId);
        Task<bool> AddFaul(Faul faul);
        Task<bool> DoesGameExist(Guid id);
    }

    public class GameRepository : IGameRepository
    {
        private readonly Context context;
        public GameRepository(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CreateGame(Game game)
        {
            await context.Games.AddAsync(game);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EndGame(Guid id)
        {
            var game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            game.EndedAt = DateTime.UtcNow;
            context.Update(game);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<bool> AddScore(Score score)
        {
            await context.Scores.AddAsync(score);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IReadOnlyList<Score>> GetScores(Guid gameId)
        {
            return await context.Scores.Where(x => x.GameId == gameId).ToListAsync();
        }

        public async Task<bool> AddFaul(Faul faul)
        {
            await context.Fauls.AddAsync(faul);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IReadOnlyList<Faul>> GetFauls(Guid gameId)
        {
            return await context.Fauls.Where(x => x.GameId == gameId).ToListAsync();
        }

        public async Task<bool> AddCard(Card card)
        {
            await context.Cards.AddAsync(card);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IReadOnlyList<Card>> GetCards(Guid gameId)
        {
            return await context.Cards.Where(x => x.GameId == gameId).ToListAsync();
        }

        public async Task<bool> StartGame(Guid id)
        {
            var game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            game.StartedAt = DateTime.UtcNow;
            context.Update(game);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public Task<bool> DoesGameExist(Guid id)
        {
            return context.Games.AnyAsync(x => x.Id == id);
        }
    }
}
