using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "项目名称不能为空")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime? EndDate { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;

        [NotMapped]
        public bool IsOverdue => EndDate.HasValue && EndDate.Value < DateTime.Today && Status != ProjectStatus.Completed;

        public int DurationDays => (EndDate ?? DateTime.Today).Subtract(StartDate).Days;

        public ICollection<TaskItem> Tasks { get; set; } = [];

        public static IEnumerable<ProjectStatus> StatusValues => Enum.GetValues<ProjectStatus>();
    }

    public enum ProjectStatus
    {
        [Display(Name = "规划中")]
        Planning,
        [Display(Name = "进行中")]
        InProgress,
        [Display(Name = "已暂停")]
        Suspended,
        [Display(Name = "已完成")]
        Completed
    }
}
