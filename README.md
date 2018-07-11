# NET.S.2018.Siaukovich.08

Процесс выполнения задачи [Customer](https://github.com/Siaukovich/NET.S.2018.Siaukovich.08/tree/master/Customer):  
* Описаны поля класса и их валидация.
* Реализован интерфейс IFormattable.
* Написаны [тесты](https://github.com/Siaukovich/NET.S.2018.Siaukovich.08/tree/master/Customer.Tests) для валидации полей, обычного ToString() и ToString() из IFormattable.
* Создан свой [форматтер](https://github.com/Siaukovich/NET.S.2018.Siaukovich.08/tree/master/ShortNameFormatProvider), который сокращает все части имени, кроме последнего, до одной буквы. Пример: "John Clark Robin Doe" -> "J. C. R. Doe".
* Для своего форматтера написаны [тесты](https://github.com/Siaukovich/NET.S.2018.Siaukovich.08/tree/master/ShortNameFormatProvider.Tests).