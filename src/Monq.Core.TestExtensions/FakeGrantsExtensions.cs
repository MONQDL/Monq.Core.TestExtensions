using Monq.Core.Authorization.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Xunit;

/// <summary>
/// Методы расширения для предоставления фейковых прав пользователя при помощи <see cref="FakeGrantsImpl"/>.
/// </summary>
public static class FakeGrantsExtensions
{
    /// <summary>
    /// Задать флаг наличия права (<paramref name="value"/>) для пользователя (<see cref="ClaimsPrincipal"/>) для прохождения проверки наличия конкретного права.
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="value">Флаг наличия права.</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeHasGrant(this FakeGrantsImpl fakeGrants, bool value = true)
    {
        fakeGrants.HasGrantFunc = (_, __, ___, ____) => value;
        return fakeGrants;
    }

    /// <summary>
    /// Задать флаг наличия права (<paramref name="value"/>) для пользователя (<see cref="ClaimsPrincipal"/>) для прохождения проверки наличия хотя бы одного из заданных прав.
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="value">Флаг наличия права.</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeHasAnyGrant(this FakeGrantsImpl fakeGrants, bool value = true)
    {
        fakeGrants.HasAnyGrantFunc = (_, __, ___, ____) => value;
        return fakeGrants;
    }

    /// <summary>
    /// Задать флаг наличия права (<paramref name="value"/>) для рабочих групп из <paramref name="workGroupIds"/>
    /// для прохождения проверки наличия хотя бы одного из заданных прав из <paramref name="grants"/>.
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="workGroupIds">Идентификаторы рабочих групп.</param>
    /// <param name="grants">Список прав.</param>
    /// <param name="value">Флаг наличия права.</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeHasAnyGrant(this FakeGrantsImpl fakeGrants,
        IEnumerable<long> workGroupIds,
        IEnumerable<string> grants,
        bool value = true)
    {
        fakeGrants.HasAnyGrantFunc = (_, __, ___, ____) => value;
        return fakeGrants;
    }

    /// <summary>
    /// Зарегистрировать рабочие группы из <paramref name="workGroupIds"/>, у которых есть хотя бы одно из заданных прав из <paramref name="grants"/>.
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="workGroupIds">Идентификаторы рабочих групп, у которых есть хотя бы одно из заданных прав из <paramref name="grants"/>.</param>
    /// <param name="grants">Список прав, по которым будет осуществляться проверка списка рабочих групп <paramref name="workGroupIds"/> на наличие хотя бы одного из заданных.</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeGetWorkGroupsWithAnyGrant(this FakeGrantsImpl fakeGrants,
        IEnumerable<long> workGroupIds,
        IEnumerable<string> grants)
    {
        fakeGrants.GetWorkGroupsWithAnyGrantFunc = (_, __, grantNames) =>
            !grantNames.Intersect(grants).Any() ? Enumerable.Empty<long>() : workGroupIds;
        return fakeGrants;
    }

    /// <summary>
    /// Зарегистрировать рабочие группы из <paramref name="workGroupIds"/>, у которых есть право <paramref name="grantName"/>.
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="workGroupIds">Идентификаторы рабочих групп, у которых есть право <paramref name="grantName"/>.</param>
    /// <param name="grantName">Наименование права, по которому будет осуществляться проверка списка рабочих групп <paramref name="workGroupIds"/> на его наличие.</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeGetWorkGroupsWithGrant(this FakeGrantsImpl fakeGrants,
        IEnumerable<long> workGroupIds,
        string grantName)
    {
        fakeGrants.GetWorkGroupsWithGrantFunc = (_, __, grant) => grant == grantName ? workGroupIds : Enumerable.Empty<long>();
        return fakeGrants;
    }

    /// <summary>
    /// Задать флаг наличия статуса "Системного пользователя" для пользователя (<see cref="ClaimsPrincipal"/>).
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="value">Флаг наличия статуса "Системного пользователя".</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeIsSystemUser(this FakeGrantsImpl fakeGrants, bool value = true)
    {
        fakeGrants.IsSystemUserFunc = _ => value;
        return fakeGrants;
    }

    /// <summary>
    /// Задать флаг наличия статуса администратора пользовательского пространства для пользователя (<see cref="ClaimsPrincipal"/>).
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="value">Флаг наличия статуса администратора пользовательского пространства.</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeIsUserspaceAdmin(this FakeGrantsImpl fakeGrants, bool value = true)
    {
        fakeGrants.IsUserspaceAdminFunc = (_, __) => value;
        return fakeGrants;
    }

    /// <summary>
    /// Задать флаг "Суперпользователя" для пользователя (<see cref="ClaimsPrincipal"/>) для прохождения проверки -
    /// является ли пользователь системным или администратором пользовательского пространства.
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="value">Флаг наличия статуса "Суперпользователя".</param>
    /// <returns></returns>
    public static FakeGrantsImpl FakeIsSuperUser(this FakeGrantsImpl fakeGrants, bool value = true)
    {
        fakeGrants.IsSuperUserFunc = (_, __) => value;
        return fakeGrants;
    }

    /// <summary>
    /// Зарегистрировать рабочие группы из <paramref name="workGroupIds"/> с заданными правами <paramref name="grants"/> в пользовательском пространстве <paramref name="userspaceId"/>
    /// в локальном словаре сопоставлений прав на рабочие группы <paramref name="workGroupsStorage"/>.
    /// </summary>
    /// <param name="fakeGrants">Реализация тестового представления методов расширения пользовательских прав для идентификаторов на основе утверждений.</param>
    /// <param name="userspaceId">Идентификатор пользовательского пространства.</param>
    /// <param name="workGroupIds">Идентификаторы рабочих групп, которым будут присвоены права из <paramref name="grants"/>.</param>
    /// <param name="grants">Права, которые будут присвоены рабочим группам.</param>
    /// <param name="workGroupsStorage">Словарь сопоставлений прав на рабочие группы.</param>
    /// <returns></returns>
    // TODO: Подумать о том, как утилизировать userspaceId без перехода к модели перечисления кортежей (userspaceId, workGroupId) вместо существующего workGroupIds.
    public static FakeGrantsImpl AddWorkGroupsWithGrants(this FakeGrantsImpl fakeGrants,
        long userspaceId,
        IEnumerable<long> workGroupIds,
        IEnumerable<string> grants,
        IDictionary<(long UserspaceId, long WorkGroupId), IEnumerable<string>> workGroupsStorage)
    {
        foreach (var workGroupId in workGroupIds)
            workGroupsStorage.Add((userspaceId, workGroupId), grants);

        return fakeGrants;
    }
}
