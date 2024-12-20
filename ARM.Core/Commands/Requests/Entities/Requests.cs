using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using MediatR;

namespace ARM.Core.Commands.Requests.Entities;

/// <summary>
/// Команда на получение сущности по Id
/// </summary>
/// <param name="Id"></param>
/// <typeparam name="T"></typeparam>
public record GetDataRequest<T>(Guid Id) : IRequest<Result<T>> where T : class, IEntity;

/// <summary>
/// Команда на получение данных по фильтру <paramref name="BaseParams"/>
/// </summary>
public record GetAllDataRequest<T>(BaseListParams BaseParams) : IRequest<Result<List<T>>> where T : class, IEntity;

/// <summary>
/// Команда на добавление данных в БД
/// </summary>
public record AddDataRequest<T>(T Entity) : IRequest<Result<T>> where T : class, IEntity;

/// <summary>
/// Команда на изменение данных в БД
/// </summary>
public record EditDataRequest<T>(T Entity) : IRequest<Result<T>> where T : class, IEntity;

/// <summary>
/// Команда на удаление данных в БД
/// </summary>
public record DeleteDataRequest<T>(Guid Id) : IRequest<Result<T>> where T : class, IEntity;