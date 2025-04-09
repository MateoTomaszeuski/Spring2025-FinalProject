using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.Models;

public partial class Assignment : ObservableObject {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? DateStarted { get; set; }
    public DateTime? DateCompleted { get; set; }

    [ObservableProperty]
    private bool isCompleted;

    partial void OnIsCompletedChanged(bool value) {
        if (value) {
            DateCompleted = DateTime.Now;
        } else {
            DateCompleted = null;
        }
    }


}