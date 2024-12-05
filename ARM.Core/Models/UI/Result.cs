namespace ARM.Core.Models.UI;

/// <summary>
/// Результат операции/действия
/// </summary>
public class Result<T> where T : class
{

    /// <summary>
    /// Состояние результата
    /// </summary>
    public bool IsSuccess { get; set; } = true;

    /// <summary>
    /// Данные
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Конструктор для успешных операций
    /// </summary>
    public Result() { }

    /// <summary>
    /// Конструктор для ошибок
    /// </summary>
    /// <param name="message">Текст ошибки</param>
    public Result(string message)
    {
        IsSuccess = false;
        Data = null;
        Message = message;
    }

    /// <summary>
    /// Конструктор при успешной отработке операции
    /// </summary>
    /// <param name="isSuccess">Предполагается, что = true</param>
    /// <param name="data">Данные</param>
    /// <param name="message">Сообщение об успешном выполнении</param>
    public Result(bool isSuccess, T data, string message = "")
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

}