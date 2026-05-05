using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApp.Data;
using TestWebApp.Models;

namespace TestWebApp.Controllers;

/// <summary>
/// Контроллер для работы списком задач
/// </summary>
public class ToDoListController : Controller
{
    /// <summary>
    /// Список задач
    /// </summary>
    private static readonly List<ToDoTask> Todos = [];
    /// <summary>
    /// Следующий идентификатор задачи
    /// </summary>
    private static int _nextId = 0;
    
    private readonly TaskDbContext _dbContext;
    
    // Конструктор с внедрением зависимости
    public ToDoListController(TaskDbContext context)
    {
        _dbContext = context;
    }

    #region GET методы

    /// <summary>
    /// Добавление задачи
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult AddTask()
    {
        var task = new ToDoTask
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
        };
        ViewBag.StartDateValue = task.StartDate.ToString("yyyy-MM-ddTHH:mm");
        ViewBag.EndDateValue = task.EndDate.ToString("yyyy-MM-ddTHH:mm");
        return View(task);
    }
    
    /// <summary>
    /// Редактирование задачи
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> EditTask(int id)
    {
        var task = await _dbContext.Tasks.FindAsync(id);
        
        if (task == null)
        {
            TempData["ErrorMessage"] = "❌ Задача не найдена!";
            return RedirectToAction("ShowAll");
        }
        
        ViewBag.StartDateValue = task.StartDate.ToString("yyyy-MM-ddTHH:mm");
        ViewBag.EndDateValue = task.EndDate.ToString("yyyy-MM-ddTHH:mm");
        return View(task);
    }
    
    /// <summary>
    /// Удаление задачи
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult DeleteTask(int id)
    {
        var removingTask = _dbContext.Tasks.FirstOrDefault(t => t.Id == id);

        if (removingTask != null)
        {
            _dbContext.Tasks.Remove(removingTask);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = $"🗑️ Задача \"{removingTask.Title}\" успешно удалена!";
        }
        
        return RedirectToAction("ShowAll");
    }

    /// <summary>
    /// Отобразить все задачи
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult ShowAll()
    {
        return View((_dbContext.Tasks.OrderBy(t => t.Id)));
    }

    public async Task<IActionResult> Tasks(int id)
    {
        var post = await _dbContext.Tasks.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return View(post);
    }

    #endregion

    #region POST методы

    /// <summary>
    /// Добавление задачи в список
    /// </summary>
    /// <param name="title">Заголовок</param>
    /// <param name="description">Описание</param>
    /// <param name="startDate">Начало задачи</param>
    /// <param name="endDate">Завершение задачи</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult AddTask(string title, string description, DateTime startDate, DateTime endDate)
    {
        var task = new ToDoTask(title, description, startDate, endDate);
        _dbContext.Add(task);
        _dbContext.SaveChanges();
        TempData["SuccessMessage"] = $"✅ Задача \"{title}\" успешно добавлена!";
        // TempData - сохраняет данные на один след запрос
     
        return RedirectToAction("ShowAll");
    }

    /// <summary>
    /// Редактирование задачи
    /// </summary>
    /// <param name="updatedTask">Редактируемая задача</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> EditTask(int id, ToDoTask updatedTask)
    {
        if (id != updatedTask.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _dbContext.Update(updatedTask);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"✏️ Задача \"{updatedTask.Title}\" успешно изменена!";
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("ShowAll");
        }
        return RedirectToAction("Index", "Home");
    }

    #endregion
}