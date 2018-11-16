using BDSA2018.Lecture11.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace BDSA2018.Lecture11.Services
{
    public interface ICharacterRepository
    {
        Task<CharacterDTO> CreateAsync(CharacterCreateUpdateDTO character);

        Task<CharacterDTO> FindAsync(int characterId);

        IQueryable<CharacterDTO> Read();

        Task<bool> UpdateAsync(CharacterCreateUpdateDTO character);

        Task<bool> DeleteAsync(int characterId);
    }
}
