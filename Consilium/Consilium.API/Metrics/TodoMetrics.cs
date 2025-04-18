using System.Diagnostics.Metrics;

namespace Consilium.API.Metrics;
public class TodoMetrics {
    private readonly UpDownCounter<int> _netTodosAdded;

    public TodoMetrics(IMeterFactory meterFactory) {
        var meter = meterFactory.Create("Consilium.Todos");
        _netTodosAdded = meter.CreateUpDownCounter<int>("consilium.todos.added");
    }

    public void TodoAdded() {
        _netTodosAdded.Add(1);
    }

    public void TodoRemoved() {
        _netTodosAdded.Add(-1);
    }
}