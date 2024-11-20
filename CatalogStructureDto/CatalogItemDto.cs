namespace CatalogStructureDto
{
    public class CatalogItemDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public IEnumerable<CatalogItemDto> Children { get; set; }
    }
}
