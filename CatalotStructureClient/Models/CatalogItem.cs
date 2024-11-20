using CatalogStructureDto;
using System.Xml.Linq;

namespace CatalogStructureClient.Models
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsChildVisible { get; set; } = true;
        public List<CatalogItem> Children { get; set; } = new();

        public static CatalogItem MapFromDto(CatalogItemDto dto)
        {
            var catalogItem = new CatalogItem
            {
                Id = dto.Id,
                Name = dto.Name,
                ParentId = dto.ParentId,
                IsChildVisible = false,
                Children = dto.Children?.Select(MapFromDto).ToList()
            };

            return catalogItem;
        }

        public static CatalogItemUpdateDto MapToUpdateDto(CatalogItem dto)
        {
            var catalogItemUpdate = new CatalogItemUpdateDto
            {
                Id = dto.Id,
                Name = dto.Name,
                ParentId = dto.ParentId
            };

            return catalogItemUpdate;
        }
    }

}
