using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class TaskLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime LogDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 24)]
        public double HoursSpent { get; set; }

        public int TaskItemId { get; set; }

        [ForeignKey(nameof(TaskItemId))]
        public TaskItem? TaskItem { get; set; }
    }
}
