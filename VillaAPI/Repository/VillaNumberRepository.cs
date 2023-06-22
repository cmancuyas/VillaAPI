using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Repository.IRepository;

namespace VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db):base(db) 
        {
            _db = db;
        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
