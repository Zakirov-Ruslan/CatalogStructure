namespace CatalogStructureClient.Models
{
    public static class CatalogItemExtensions
    {
        // Recoursive tree search by bool delegate
        public static CatalogItem? FindNode(this IEnumerable<CatalogItem> nodes, Func<CatalogItem, bool> predicate)
        {
            if (nodes == null) return null;

            foreach (var node in nodes)
            {
                if (predicate(node))
                {
                    return node;
                }

                var found = node.Children.FindNode(predicate);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }
    }
}
