using ProjectManager.Models;
using Microsoft.EntityFrameworkCore;
using TaskStatus = ProjectManager.Models.TaskStatus;


namespace ProjectManager.Data
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (await context.Employees.AnyAsync())
            {
                return;
            }

            var employees = new List<Employee>
            {
                new() { Name = "张三", Email = "zhangsan@company.com", Department = "技术部", HireDate = new DateTime(2023, 1, 15), Phone = "13800001001" },
                new() { Name = "李四", Email = "lisi@company.com", Department = "产品部", HireDate = new DateTime(2023, 3, 20), Phone = "13800001002" },
                new() { Name = "王五", Email = "wangwu@company.com", Department = "技术部", HireDate = new DateTime(2022, 6, 1), Phone = "13800001003" },
                new() { Name = "赵六", Email = "zhaoliu@company.com", Department = "设计部", HireDate = new DateTime(2024, 1, 10), Phone = "13800001004" },
            };

            context.Employees.AddRange(employees);

            await context.SaveChangesAsync();

            var categories = new List<TaskCategory>
            {
                new() { Name = "新功能开发", Description = "新增功能相关任务" },
                new() { Name = "Bug修复", Description = "缺陷修复相关任务" },
                new() { Name = "文档编写", Description = "技术文档、用户手册" },
                new() { Name = "代码审查", Description = "Code Review 相关" },
                new() { Name = "测试", Description = "单元测试、集成测试" },
            };

            context.Categories.AddRange(categories);

            await context.SaveChangesAsync();

            var projects = new List<Project>
            {
                new()
                {
                    Name = "CRM 客户管理系统",
                    Description = "企业级客户关系管理系统，包括客户管理、销售漏斗、数据分析等模块",
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 12, 31),
                    Status = ProjectStatus.InProgress
                },
                new()
                {
                    Name = "移动端 APP 重构",
                    Description = "将现有移动端应用从原生开发迁移到 Flutter 跨平台框架",
                    StartDate = new DateTime(2025, 6, 1),
                    EndDate = new DateTime(2026, 6, 30),
                    Status = ProjectStatus.Planning
                },
            };

            await context.Projects.AddRangeAsync(projects);

            await context.SaveChangesAsync();

            var tasks = new List<TaskItem>
            {
                new()
                {
                    Title = "用户登录模块开发",
                    Description = "实现用户名密码登录、记住密码、密码加密存储等功能",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.InProgress,
                    CreatedDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(5),
                    AssignedEmployeeId = employees[0].Id,
                    ProjectId = projects[0].Id,
                    CategoryId = categories[0].Id,
                },
                new()
                {
                    Title = "客户列表页面 UI 设计",
                    Description = "设计客户列表页面的布局、筛选器、分页组件",
                    Priority = TaskPriority.Normal,
                    Status = TaskStatus.Completed,
                    CreatedDate = DateTime.Now.AddDays(-20),
                    DueDate = DateTime.Now.AddDays(-5),
                    CompletedDate = DateTime.Now.AddDays(-3),
                    AssignedEmployeeId = employees[3].Id,
                    ProjectId = projects[0].Id,
                    CategoryId = categories[0].Id,
                },
                new()
                {
                    Title = "修复用户权限判断漏洞",
                    Description = "修复在特定场景下普通用户可以访问管理接口的安全漏洞",
                    Priority = TaskPriority.Critical,
                    Status = TaskStatus.Review,
                    CreatedDate = DateTime.Now.AddDays(-3),
                    DueDate = DateTime.Now.AddDays(1),
                    AssignedEmployeeId = employees[2].Id,
                    ProjectId = projects[0].Id,
                    CategoryId = categories[1].Id,
                },
                new()
                {
                    Title = "编写 API 接口文档",
                    Description = "为 CRM 系统的 REST API 编写 OpenAPI/Swagger 文档",
                    Priority = TaskPriority.Low,
                    Status = TaskStatus.Todo,
                    CreatedDate = DateTime.Now.AddDays(-1),
                    DueDate = DateTime.Now.AddDays(14),
                    AssignedEmployeeId = employees[1].Id,
                    ProjectId = projects[0].Id,
                    CategoryId = categories[2].Id,
                },
                new()
                {
                    Title = "Flutter 环境搭建与项目初始化",
                    Description = "搭建 Flutter 开发环境，创建项目骨架，配置 CI/CD 管道",
                    Priority = TaskPriority.High,
                    Status = TaskStatus.Todo,
                    CreatedDate = DateTime.Now.AddDays(-2),
                    DueDate = DateTime.Now.AddDays(7),
                    AssignedEmployeeId = employees[0].Id,
                    ProjectId = projects[1].Id,
                    CategoryId = categories[0].Id,
                },
                new()
                {
                    Title = "编写数据访问层单元测试",
                    Description = "为 DataService 中的所有方法编写 xUnit 单元测试，覆盖率 > 80%",
                    Priority = TaskPriority.Normal,
                    Status = TaskStatus.Todo,
                    CreatedDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(10),
                    AssignedEmployeeId = employees[2].Id,
                    ProjectId = projects[0].Id,
                    CategoryId = categories[4].Id,
                },
                new()
                {
                    Title = "代码审查：用户模块重构",
                    Description = "审查张三提交的用户模块重构代码，关注性能和安全性",
                    Priority = TaskPriority.Normal,
                    Status = TaskStatus.InProgress,
                    CreatedDate = DateTime.Now.AddDays(-5),
                    DueDate = DateTime.Now.AddDays(2),
                    AssignedEmployeeId = employees[1].Id,
                    ProjectId = projects[0].Id,
                    CategoryId = categories[3].Id,
                },
            };

            context.Tasks.AddRange(tasks);

            await context.SaveChangesAsync();

            var logs = new List<TaskLog>
            {
                new() { TaskItemId = tasks[0].Id, LogDate = DateTime.Now.AddDays(-10), Description = "开始分析需求文档", HoursSpent = 2 },
                new() { TaskItemId = tasks[0].Id, LogDate = DateTime.Now.AddDays(-8), Description = "搭建项目结构，配置加密方案", HoursSpent = 4 },
                new() { TaskItemId = tasks[0].Id, LogDate = DateTime.Now.AddDays(-5), Description = "完成登录 API 接口开发", HoursSpent = 6 },
                new() { TaskItemId = tasks[2].Id, LogDate = DateTime.Now.AddDays(-3), Description = "收到安全团队报告，开始排查", HoursSpent = 1 },
                new() { TaskItemId = tasks[2].Id, LogDate = DateTime.Now.AddDays(-2), Description = "定位漏洞原因，修复权限中间件", HoursSpent = 3 },
                new() { TaskItemId = tasks[2].Id, LogDate = DateTime.Now.AddDays(-1), Description = "编写修复验证测试", HoursSpent = 2 },
            };

            context.TaskLogs.AddRange(logs);

            await context.SaveChangesAsync();
        }
    }
}
