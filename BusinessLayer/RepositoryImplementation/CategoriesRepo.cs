using AutoMapper;
using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.RepositoryImplementation
{
    public class CategoriesRepo : ICategories
    {
        private readonly EcDbContext _db;
        private readonly IMapper mapper;
        public CategoriesRepo(EcDbContext db ,IMapper mapper)
        {
            this.mapper = mapper;
            _db = db;
        }

        public async Task<Category> CategoryAdd(string Name, int Id, int userId)
        {
            try
            {
                Category obj = new Category
                {
                    CategoryName = Name,
                    Id = Id,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userId,
                };
                await _db.Categories.AddAsync(obj);
                await _db.SaveChangesAsync();
                return (obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Category> DeleteCategories(int id)
        {
            try
            {
                var category = _db.Categories.Where(x => x.Id == id).FirstOrDefault();

                if (category == null)
                {
                    throw new Exception("Category not Found");
                }
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
                return (category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<AllCategoriesDTO>> GetAllCategories()
        {
            try
            {
                var categoryList = await _db.Categories.Include(
                    x=>x.Products)

                    .ToListAsync();
                List<AllCategoriesDTO> list = new List<AllCategoriesDTO>();

                foreach (var category in categoryList)
                {
                    AllCategoriesDTO a = new AllCategoriesDTO();

                    a.ID = category.Id;
                    a.categoryName = category.CategoryName;

                    List<ProductDTO> prodList = new List<ProductDTO>();
                    foreach (var product in category.Products)
                    {
                        prodList.Add(new ProductDTO
                        {
                            Id = product.Id,
                            ProductName = product.ProductName,
                            ProductDescription = product.ProductDescription,
                            price = product.price,
                            ImgUrl = _db.Images.FirstOrDefault(x=>x.ProductId == product.Id).url
                           
                            
                        });

                    }

                    a.CatProduct = prodList;
                    list.Add(a);

                }

                //var cat = await _db.Products.Include(x => x.Category)
                //    .Select(x => new AllCategoriesDTO
                //    {
                //        categoryName = x.Category.CategoryName,
                //        ID = x.CategoryID,
                //        CatProduct = new ProductDTO()
                //        {
                //            Id = x.Id,
                //            ProductName= x.ProductName,
                //            price = x.price,
                           
                //            ProductDescription = x.ProductDescription,
                //            ImgUrl = x.Image.url,
                //        }
                //    }).ToListAsync();

                
                return (list);

                //var cat = await _db.Categories.Include(x => x.Products).ToListAsync();

                //return (cat);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UpdateCategories(string name, int id, int UserId)
        {
            try
            {
                var cat = _db.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (cat == null)
                {
                    throw new Exception("Categories not Found");
                }

                cat.CategoryName = name;
                cat.ModifiedAt = DateTime.Now;
                cat.ModifiedBy = UserId;

                _db.Categories.Update(cat);
                await _db.SaveChangesAsync();
                return ($"Categories Name {name} is Updates Sucessfully");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}