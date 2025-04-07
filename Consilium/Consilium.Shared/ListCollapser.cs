using Consilium.Shared.Models;

namespace Consilium.Shared;

public static class ListCollapser {
    public static IEnumerable<TodoItem> CollapseList(List<TodoItem> items) {
        for (int i = 0; i < items.Count; i++) {
            TodoItem item = items[i];
            if (item.ParentId is null) { continue; }

            TodoItem parent = items.First(a => a.Id == item.ParentId);
            parent.Subtasks.Add(item);
            items.Remove(item);
            i--;
        }
        return items;
    }
}