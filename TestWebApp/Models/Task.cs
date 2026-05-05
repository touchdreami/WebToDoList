using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models;

/// <summary>
/// Класс, описывающий задачу
/// </summary>
public class ToDoTask
{
    #region Свойства
    /// <summary>
    /// Идентификатор
    /// </summary>
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Название
    /// </summary>
    public string? Title { get; set; } = string.Empty;
    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// Флаг, сделана ли задача
    /// </summary>
    public bool IsCompleted { get; set; }
    /// <summary>
    /// Начало выполнения
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// Время окончания (планируемое)
    /// </summary>
    public DateTime EndDate { get; set; }
    #endregion

    #region Конструкторы
    /// <summary>
    /// по умолчананию
    /// </summary>
    public ToDoTask() {}

    /// <summary>
    /// Конструктор с данными по задаче
    /// </summary>
    /// <param name="id">Идентификатор задачи</param>
    /// <param name="title">Заголовок</param>
    /// <param name="description">Описание</param>
    /// <param name="startDate">Начало выполнения</param>
    /// <param name="endDate">Срок завершения</param>
    public ToDoTask(string title, string? description, DateTime startDate, DateTime endDate)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }
    #endregion
}