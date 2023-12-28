using ShoppingCartMVC.DataAccess.Data;
using ShoppingCartMVC.DataAccess.Repository.IRepository;
using ShoppingCartMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartMVC.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        //public void Save()
        //{
        //    _context.SaveChanges();
        //}

        public void Update(Product obj)
        {
            _context.Products.Update(obj);

            // For manual updating 
            //var objFromDb = _context.Products.FirstOrDefault(u => u.Id == obj.Id);
            //if(objFromDb != null)
            //{
            //    objFromDb.Title = obj.Title;
            //    objFromDb.Description = obj.Description;
            //    objFromDb.ISBN = obj.ISBN;
            //    objFromDb.Price = obj.Price;
            //    objFromDb.ListPrice = obj.ListPrice;
            //    objFromDb.Price50 = obj.Price50;
            //    objFromDb.Price100 = obj.Price100;
            //    objFromDb.CategoryId = obj.CategoryId;
            //    objFromDb.Author = obj.Author;
            //    if(obj.ImageURL != null)
            //    {
            //        objFromDb.ImageURL = obj.ImageURL;
            //    }
            //}
        }
    }
}
