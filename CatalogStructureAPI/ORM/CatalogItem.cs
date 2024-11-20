namespace CatalogStructureAPI.ORM
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public CatalogItem Parent { get; set; }
        public ICollection<CatalogItem> Children { get; set; }
    }
}
