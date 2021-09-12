using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);   
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            var filter = Builders<Product>.Filter.Eq(el => el.Id, productId);

            var opResult = await _context
                                    .Products
                                    .DeleteOneAsync(filter);

            return opResult.IsAcknowledged &&
                    opResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context
                            .Products
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                .Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                .Products
                .ReplaceOneAsync(g => g.Id == product.Id, product);
            
            return updateResult.IsAcknowledged 
                && updateResult.ModifiedCount > 0;

        }
    }
}







