using AutoMapper;
using CatalogStructureAPI.ORM;
using CatalogStructureDto;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CatalogStructureAPI.Services
{
    public class CatalogItemsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CatalogItemsService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CatalogItemDto>> GetCatalogItemsAsync()
        {
            var rootCatalogs = await _context.CatalogItems
                .Where(c => c.ParentId == null)
                .Include(c => c.Children)
                .ToListAsync();

            var rootCatalogsDto = rootCatalogs.Select(i => _mapper.Map<CatalogItemDto>(i));

            return rootCatalogsDto;
        }

        public async Task<CatalogItemDto> GetCatalogItemAsync(int id)
        {
            var catalogItem = await _context.CatalogItems.
                Include(i => i.Children).
                FirstOrDefaultAsync(i => i.Id == id);

            var catalotItemDto = _mapper.Map<CatalogItemDto>(catalogItem);

            return catalotItemDto;
        }

        public async Task<CatalogItemDto> CreateCatalogItemAsync(CatalogItemCreateDto catalogItemCreateDto)
        {
            if (catalogItemCreateDto.ParentId.HasValue)
            {
                var parentId = catalogItemCreateDto.ParentId;
                var parent = await _context.CatalogItems.FindAsync(parentId);
                if (parent == null)
                    throw new ArgumentException("Parent doesn exist");
            }

            var catalogItem = _mapper.Map<CatalogItem>(catalogItemCreateDto);

            _context.CatalogItems.Add(catalogItem);
            await _context.SaveChangesAsync();

            var createdCatalogItem = await _context.CatalogItems.FindAsync(catalogItem.Id);
            var catalogItemDto = _mapper.Map<CatalogItemDto>(createdCatalogItem);

            return catalogItemDto;
        }

        public async Task UpdateCatalogItemAsync(CatalogItemUpdateDto catalogItemUpdateDto)
        {
            if (catalogItemUpdateDto.ParentId.HasValue)
            {
                var parentId = catalogItemUpdateDto.ParentId;
                var parent = await _context.CatalogItems.FindAsync(parentId);
                if (parent == null)
                    throw new ArgumentException("Parent doesn exist");
                if (parentId == catalogItemUpdateDto.Id)
                    throw new ArgumentException("Parent can't be the same as creatable object");

                if (catalogItemUpdateDto.ParentId.HasValue)
                {
                    bool isDescendant = await IsDescendantAsync(catalogItemUpdateDto.Id, catalogItemUpdateDto.ParentId.Value);
                    if (isDescendant)
                        throw new ArgumentException("New parent can't be a child of current node");
                }
            }

            var catalogItem = _mapper.Map<CatalogItem>(catalogItemUpdateDto);

            _context.ChangeTracker.Clear();
            _context.Entry(catalogItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CatalogItemExists(catalogItem.Id))
                {
                    throw new KeyNotFoundException(ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteCatalogItemAsync(int id)
        {
            var catalogItem = await _context.CatalogItems.FindAsync(id);
            if (catalogItem == null)
            {
                throw new KeyNotFoundException($"Catalog with id {id} doesnt exists");
            }

            _context.CatalogItems.Remove(catalogItem);
            await _context.SaveChangesAsync();
        }

        private bool CatalogItemExists(int id)
        {
            return _context.CatalogItems.Any(e => e.Id == id);
        }

        public async Task<bool> IsDescendantAsync(int currentNodeId, int newParentId)
        {
            var currentNode = await _context.CatalogItems
                .Include(n => n.Children)
                .FirstOrDefaultAsync(n => n.Id == currentNodeId);

            if (currentNode == null)
                return false;

            return await IsDescendantRecursive(currentNode, newParentId);
        }

        private async Task<bool> IsDescendantRecursive(CatalogItem node, int newParentId)
        {
            if (node.Id == newParentId)
                return true;

            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    if (await IsDescendantRecursive(child, newParentId))
                        return true;
                }
            }

            return false;
        }
    }
}
