using AutoMapper;
using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation
{
    public class ProductsRepo : IProduct
    {
        private readonly EcDbContext _db;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ProductsRepo(EcDbContext db,IMapper mapper, IMemoryCache memoryCache)
        {
            _db = db;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<ProductDTO> AddProductAsync(ProductDTO product, int userId)
        {
            try
            {
                var pro = _mapper.Map<Products>(product);
                pro.UserId = userId;
                pro.CreatedAt = DateTimeOffset.Now;
                pro.CreatedBy = userId;

               
                 _db.Products.Add(pro);
                await _db.SaveChangesAsync();

                return (_mapper.Map<ProductDTO>(pro));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductDTO>> View()
        {
            try
            {
                var cacheKey = "Productresult";
                if (!_memoryCache.TryGetValue(cacheKey, out List<ProductDTO> Productresult))
                {

                    var products = await _db.Products.Include(x => x.Category).ToListAsync();
                    Productresult = new List<ProductDTO>();
                    foreach (var i in products)
                    {
                        ProductDTO pro = new ProductDTO()
                        {
                            Id = i.Id,
                            ProductName = i.ProductName,
                            ProductDescription = i.ProductDescription,
                            CategoryID = i.CategoryID,

                            price = i.price,
                        };
                        Productresult.Add(pro);


                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(30),

                        Priority = CacheItemPriority.High,

                        SlidingExpiration = TimeSpan.FromSeconds(20)

                    };
                    _memoryCache.Set(cacheKey, Productresult, cacheExpiryOptions);

                }
               
                }
                return Productresult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> DeleteProducts(int id)
        {
            try
            {
                var productDelete = _db.Products.Where(x => x.Id == id).FirstOrDefault();
                if (productDelete == null)
                {
                    throw new Exception("No Product Found");
                }

                _db.Products.Remove(productDelete);
                _db.SaveChangesAsync();
                return ($"Product {productDelete.ProductName} is Deleted Sucessfully ");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO obj, int userId)
        {
            try
            {
                var productupdate = _db.Products.Where(x => x.Id == obj.Id).FirstOrDefault();

                productupdate.ProductName = obj.ProductName;
                productupdate.price = obj.price;
                productupdate.ProductDescription = obj.ProductDescription;
                productupdate.CategoryID = obj.CategoryID;
                productupdate.ModifiedAt = DateTimeOffset.Now;
                productupdate.ModifiedBy = userId;

                _db.Products.Update(productupdate);
                _db.SaveChanges();

                return (obj);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}