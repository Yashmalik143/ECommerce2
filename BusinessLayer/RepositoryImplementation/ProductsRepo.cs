﻿using AutoMapper;
using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;
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

        public ProductsRepo(EcDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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
                var products = await _db.Products.Include(x => x.Category).ToListAsync();
                List<ProductDTO> Productresult = new List<ProductDTO>();
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