using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository  //It should also implement the repository<villa>
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)  //because the Repository requires a dbcontext also now, so we have to pass that also
        {
            _db = db;
        }
        
        public void Update(VillaNumber entity)
        {
            _db.VillaNumbers.Update(entity);  //explicitly add the villas to add to villas table
        }
    }
}
