# ���������� ��������� �������� �������� ������������� .net core

���������� �������� ����� ����������, ������� ����������� � �������� �������� AspNet Core.

#### AssertFilterIsValid<TFilter, TModel>()
��������� ������������ ������� � ������ (��� �� ���� �� ������� `TFilter` ����������� � ������ `TModel`, � ������������� �� �� ����).

**������**
```csharp
[Fact(DisplayName = "��������� ������������ ������ �������.")]
public void ShouldProperlyValidFilter()
{
    AssertExtensions.AssertFilterIsValid<TestFilterViewModel, ValueViewModel>();
}
```