using ARM.Core.Models.Entities.Intf;
using ARM.Core.Models.UI;
using MediatR;

namespace ARM.Core.Commands.Requests.Entities.ActualEntities;

/// <summary>
/// Команда на получение сущности по Id
/// </summary>
/// <param name="Id"></param>
/// <typeparam name="T"></typeparam>
public record GetActualDataRequest<T>(Guid Id) : IRequest<Result<T>> where T : class, IActualEntity;

/// <summary>
/// Команда на получение данных по фильтру <paramref name="BaseParams"/>
/// </summary>
public record GetActualAllDataRequest<T>(BaseListParams BaseParams) : IRequest<Result<List<T>>> where T : class, IActualEntity;

/// <summary>
/// Команда на добавление данных в БД
/// </summary>
public record AddActualDataRequest<T>(T Entity) : IRequest<Result<T>> where T : class, IActualEntity;

/// <summary>
/// Команда на изменение данных в БД
/// </summary>
public record EditActualDataRequest<T>(T Entity) : IRequest<Result<T>> where T : class, IActualEntity;

/// <summary>
/// Команда на удаление данных в БД
/// </summary>
public record DeleteActualDataRequest<T>(T Entity) : IRequest<Result<T>> where T : class, IActualEntity;