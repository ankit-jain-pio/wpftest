using SimpleWpfMvvmAppV8.Data;
using SimpleWpfMvvmAppV8.Models;

namespace SimpleWpfMvvmAppV8.Repositories;

public class ProductRepository
{
    public List<Product> GetAllProducts()
    {
        using var db = new AppDbContext();
        return [.. db.Products.Where(p => p.IsActive)];
    }

    public Product? GetProductById(int id)
    {
        using var db = new AppDbContext();
        return db.Products.FirstOrDefault(p => p.Id == id);
    }

    public void AddProduct(Product product)
    {
        using var db = new AppDbContext();
        db.Products.Add(product);
        db.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        using var db = new AppDbContext();
        db.Products.Update(product);
        db.SaveChanges();
    }

    public void DeleteProduct(int id)
    {
        using var db = new AppDbContext();
        var product = db.Products.Find(id);
        if (product != null)
        {
            product.IsActive = false; // Soft delete
            db.SaveChanges();
        }
    }

    public bool SKUExists(string sku, int? excludeId = null)
    {
        using var db = new AppDbContext();
        return db.Products.Any(p => p.SKU == sku && p.IsActive && (excludeId == null || p.Id != excludeId));
    }
}
