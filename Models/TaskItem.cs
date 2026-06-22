using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Navigation;

namespace ProjectManager.Models
{
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = ("任务标题不能为空"))]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        public TaskPriority Priority { get; set; } = TaskPriority.Normal;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        //外键
        public int? AssignedEmployeeId { get; set; }

        public int? ProjectId { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey(nameof(AssignedEmployeeId))]
        public Employee? AssignedEmployee { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public TaskCategory? Category { get; set; }

        public ICollection<TaskLog> Logs { get; set; } = [];

        [NotMapped]
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Today && Status != TaskStatus.Completed;

        [NotMapped]
        public string PriorityDisplay => Priority switch
        {
            TaskPriority.Critical => "🔴 紧急",
            TaskPriority.High => "🟠 高",
            TaskPriority.Normal => "🟡 普通",
            TaskPriority.Low => "🟢 低",
            _ => "未知"
        };

        [NotMapped]
        public string StatusDisplay => Status switch
        {
            TaskStatus.Todo => "待办",
            TaskStatus.InProgress => "进行中",
            TaskStatus.Review => "审核中",
            TaskStatus.Completed => "已完成",
            _ => ""
        };

        public static IEnumerable<TaskStatus> StatusValues => Enum.GetValues<TaskStatus>();

        public static IEnumerable<TaskPriority> PriorityValues => Enum.GetValues<TaskPriority>();
    }

    public enum TaskStatus
    {
        [Display(Name = "待办")]
        Todo,
        [Display(Name = "进行中")]
        InProgress,
        [Display(Name = "审核中")]
        Review,
        [Display(Name = "已完成")]
        Completed
    }

    public enum TaskPriority
    {
        [Display(Name = "低")]
        Low,
        [Display(Name = "普通")]
        Normal,
        [Display(Name = "高")]
        High,
        [Display(Name = "紧急")]
        Critical
    }
}
