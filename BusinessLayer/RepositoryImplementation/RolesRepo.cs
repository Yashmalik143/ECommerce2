using AutoMapper;
using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.RepositoryImplementation
{
    public class RolesRepo : IRoles
    {
        private readonly EcDbContext _db;
        private readonly IMapper _mapper;
        public RolesRepo(EcDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RoleDTO> GetRolesAsync(RoleDTO role)
        {
           var obj =  _mapper.Map<Roles>(role);
            try
            {
                obj.CreatedAt = DateTimeOffset.Now;

              await  _db.Roles.AddAsync(obj);
               await _db.SaveChangesAsync();
                
                return role;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleDTO>> ViewRolesAsync()
        {
            try
            {
                var obj = _db.Roles.Include(x => x.users)
                    .Select(x => new RoleDTO()
                    {
                        ID = x.ID,
                        RoleName = x.RoleName,
                        users = x.users.Select(u => new UserRoleDTO()
                        {
                            ID = u.ID,
                            Name = u.Name
                        }).ToList(),
                    })
                    .ToList();

                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}