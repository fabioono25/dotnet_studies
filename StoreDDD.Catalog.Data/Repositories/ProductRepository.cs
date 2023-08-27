using BasicStore.Catalog.Domain;
using BasicStore.Core.Data;
using Microsoft.EntityFrameworkCore;
using StoreDDD.Catalog.Data;

namespace BasicStore.Catalog.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetByCategory(int codigo)
        {
            return await _context.Products.AsNoTracking().Include(p => p.Category).Where(c => c.Category.Code == codigo).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public void Add(Product Product)
        {
            _context.Products.Add(Product);
        }

        public void Update(Product Product)
        {
            _context.Products.Update(Product);
        }

        public void Add(Category Category)
        {
            _context.Categories.Add(Category);
        }

        public void Update(Category Category)
        {
            _context.Categories.Update(Category);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
