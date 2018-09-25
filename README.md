# Библиотека поддержки тестовых проектов микросервисов .net core

Библиотека содержит набор расширений, который применяется в тестовых проектах AspNet Core.
### Assert
#### FilterIsValid<TFilter, TModel>()
Проверяет соответствие фильтра и модели (все ли поля из фильтра `TFilter` содержаться в модели `TModel`, и соответствуют ли их типы).

**Пример**
```csharp
[Fact(DisplayName = "Проверить соответствие модели фильтру.")]
public void ShouldProperlyValidFilter()
{
    Assert.FilterIsValid<TestFilterViewModel, ValueViewModel>();
}
```
#### Collection
Проверить коллекцию, с использованием проверочной коллекции.
Если для проверочной коллекции не найден соответствующий элемент, то ему присваивается значение `null`.

**Пример**
```csharp
Assert.Collection(expectedCollection, actualCollection, x => x.Id, x => x.Id,
 (expected, actual) =>
 {
     Assert.NotNull(actual);
     Assert.Equal(expected.Name, actual.Name);
 });
```