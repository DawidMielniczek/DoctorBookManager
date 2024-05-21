using DoctorBookManager.Data.EntityBase;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace DoctorBookManager.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FirstAsync(x => x.Id == id);
        }

        public async Task InsertAsync(T entity)
        {
            HandleCreationAuditing(entity);
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public void Insert(T entity)
        {
            HandleCreationAuditing(entity);
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public async Task UpdateAsync(T entity)
        {
            var actual = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (actual == null)
                throw new Exception("Record with the specified ID was not found.");
            HandleCreationAuditing(entity);
            //actual = entity;
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<int> InsertAndGetIdAsync(T entity)
        {
            HandleCreationAuditing(entity);
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FirstAsync(x => x.Id == id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        #region HandleAuditing
        private void HandleCreationAuditing(dynamic entity, List<Type> types = null, bool addToCheck = true)
        {
            if (types == null)
                types = new List<Type>();
            Type entityType = entity.GetType();
            if (addToCheck)
                types.Add(entityType);
            var relatedAudited = entityType.GetProperties()
                .Where(x => typeof(IHasCreationTime).IsAssignableFrom(x.PropertyType) ||
                (x.PropertyType.IsGenericType &&
                 x.PropertyType.GetGenericArguments().Any(genericType => typeof(IHasCreationTime).IsAssignableFrom(genericType)))
                || x.PropertyType.GetInterfaces().Contains(typeof(IHasCreationAudited))).ToList()/*Select(x => x.GetValue(entity))*/;

            foreach (var prop in relatedAudited)
            {
                var propValue = prop.GetValue(entity);
                if (propValue != null)
                {
                    if (propValue is IEnumerable auditedCollection)
                    {
                        foreach (var item in auditedCollection)
                        {
                            if (!types.Any(t => t.Name == item.GetType().Name))
                                HandleCreationAuditing(item, types, false);
                        }
                        types.Add(entityType);
                    }
                    else
                    {
                        if (!types.Any(t => t.Name == propValue.GetType().Name))
                            HandleCreationAuditing(propValue, types, true);
                    }



                }
            }
            var creationTimeProp = entityType.GetProperty("CreationTime");
            var creatorUserProp = entityType.GetProperty("CreatorUser");
            var creatorUserIdProp = entityType.GetProperty("CreatorUserId");


            if (creationTimeProp != null && creationTimeProp.GetValue(entity) == DateTime.MinValue)
                creationTimeProp.SetValue(entity, DateTime.Now);

            if (creatorUserIdProp != null && string.IsNullOrEmpty(creatorUserIdProp.GetValue(entity)))
            {
                var currentUser = _httpContextAccessor.HttpContext.User;
                creatorUserIdProp.SetValue(entity, currentUser.FindFirstValue(ClaimTypes.NameIdentifier));
            }
        }




        #endregion
    }
}
