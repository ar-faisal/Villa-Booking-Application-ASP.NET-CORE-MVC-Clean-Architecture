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
    public class VillaRepository : Repository<Villa>, IVillaRepository  //It should also implement the repository<villa>
    {
        private readonly ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db) : base(db)  //because the Repository requires a dbcontext also now, so we have to pass that also
        {
            _db = db;
        }
        
        public void Update(Villa entity)
        {
            _db.Villas.Update(entity);  //explicitly add the villas to add to villas table
        }
    }
}
