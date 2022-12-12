using EFCoreRelationships.Data.Context;
using EFCoreRelationships.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRelationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly AppDataContext context;

        public CharacterController(AppDataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int UserId)
        {
            var characters = await context.Characters
            .Where(d => d.UserId == UserId)
            .Include(c => c.Weapon)
            .Include(c => c.Skills)
            .ToListAsync();

            return characters;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(Character character)
        {
            context.Characters.Add(character);
            await context.SaveChangesAsync();

            return await Get(character.UserId);
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> CreateWeapon(Weapon weapon)
        {
            context.Weapons.Add(weapon);
            await context.SaveChangesAsync();
            var character = await context.Characters.FindAsync(weapon.CharacterId);
            if (character == null)
            {
                return NotFound();
            }
            return character;
        }

        [HttpPost("skill")]
        public async Task<ActionResult<Character>> CreateSkill(DtoCharacterSkill req)
        {
            var skill = await context.Skills.FindAsync(req.SkillId);
            if (skill == null)
            {
                return NotFound();
            }

            var character = await context.Characters.Where(c => c.Id == req.CharacterId).Include(c => c.Skills).Include(c => c.Weapon).FirstOrDefaultAsync();
            
            if (character == null)
            {
                return NotFound();
            }

            character.Skills.Add(skill);
            await context.SaveChangesAsync();
            
            return character;
        }

    }
}
