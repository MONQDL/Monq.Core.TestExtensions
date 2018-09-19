# Библиотека поддержки тестовых проектов микросервисов .net core

Библиотека содержит набор расширений, который применяется в тестовых проектах AspNet Core.

#### AssertFilterIsValid<TFilter, TModel>()
Проверяет соответствие фильтра и модели (все ли поля из фильтра `TFilter` содержаться в модели `TModel`, и соответствуют ли их типы).

**Пример**
```csharp
[Fact(DisplayName = "Проверить соответствие модели фильтру.")]
public void ShouldProperlyValidFilter()
{
    AssertExtensions.AssertFilterIsValid<TestFilterViewModel, ValueViewModel>();
}
```