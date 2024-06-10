using ToDo.Interfaces.DataAccessInterfaces;
using ToDo.Interfaces.Services;
using ToDo.Models.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Diagnostics;

namespace ToDo.Services.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();

            await taskItemRepository.CreateAsync(taskItem, cancellationToken);

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int taskItemId)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();
            await taskItemRepository.DeleteAsync(taskItemId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<TaskItem> GetTaskItemAsync(int taskItemId, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();
            return await taskItemRepository.ReadEntityByIdAsync(taskItemId, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TaskItem>?> GetTaskItemsAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();

            var taskItems = await taskItemRepository.ReadEntitiesByPredicate(e => e.TenantId == tenantId, cancellationToken: cancellationToken);

            return taskItems;
        }

        public async Task<TaskItem> UpdateTaskItemAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        {
            var taskItemRepository = await _unitOfWork.GetRepository<TaskItem>();
            return await taskItemRepository.UpdateAsync(taskItem, cancellationToken);
        }
        public async Task GenerateReportAsync(int tenantId, CancellationToken cancellationToken = default)
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string outputPath = Path.Combine(downloadsPath, $"TaskReport_Tenant_{tenantId}.pdf");
            var taskItems = await GetTaskItemsAsync(tenantId);
            if (taskItems == null)
            {
                return;
            }

            int totalTasks = taskItems.Count();
            int completedTasks = taskItems.Count(t => t.Status == Models.Enum.Status.Done); // Assuming there is a "Status" property

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont headerFont = new XFont("Verdana", 20, XFontStyle.Bold);

            gfx.DrawString($"Звіт задач Для представників групи {tenantId}", headerFont, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopCenter);
            gfx.DrawString($"Загальна кількість задач: {totalTasks}", font, XBrushes.Black, new XRect(0, 60, page.Width, 50), XStringFormats.TopLeft);
            gfx.DrawString($"Зроблені задачі: {completedTasks}", font, XBrushes.Black, new XRect(0, 90, page.Width, 50), XStringFormats.TopLeft);

            int yPoint = 150;
            gfx.DrawString("Список задач:", headerFont, XBrushes.Black, new XRect(0, yPoint, page.Width, 50), XStringFormats.TopLeft);

            yPoint += 40;

            gfx.DrawString("ID", font, XBrushes.Black, new XRect(40, yPoint, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Назва", font, XBrushes.Black, new XRect(100, yPoint, 300, 20), XStringFormats.TopLeft);
            gfx.DrawString("Статус", font, XBrushes.Black, new XRect(420, yPoint, 100, 20), XStringFormats.TopLeft);

            yPoint += 30;
            foreach (var taskItem in taskItems)
            {
                gfx.DrawString(taskItem.Id.ToString(), font, XBrushes.Black, new XRect(40, yPoint, 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(taskItem.Title, font, XBrushes.Black, new XRect(100, yPoint, 300, 20), XStringFormats.TopLeft);
                gfx.DrawString(taskItem.Status.ToString(), font, XBrushes.Black, new XRect(420, yPoint, 100, 20), XStringFormats.TopLeft);
                yPoint += 30;
            }

            using (var stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                document.Save(stream);
            }

            // Open the PDF file
            OpenPdfFile(outputPath);
        }

        private void OpenPdfFile(string filePath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                }
            };
            process.Start();
        }
    }
}
