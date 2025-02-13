using Reflection;
using System.Diagnostics;
using System.Text.Json;

F f = F.Get(); // Получаем объект F

//// первый запуск вне цикла.
CsvSerializer.Serialize(f);
JsonSerializer.Serialize(f);


//вызываем сериализацию 100000 для замера времени
int iterations = 100000;
Stopwatch sw = Stopwatch.StartNew();

for (int i = 0; i < iterations; i++)
{
    CsvSerializer.Serialize(f);
}

sw.Stop();
long csvTime = sw.ElapsedMilliseconds;

//сериализуем JSON
sw.Restart();

for (int i = 0; i < iterations; i++)
{
    JsonSerializer.Serialize(f);
}

sw.Stop();
long jsonTime = sw.ElapsedMilliseconds;

//выводим результаты
string csv = CsvSerializer.Serialize(f);
Console.WriteLine("CSV строка: " + csv);
Console.WriteLine($"Время CSV сериализации ({iterations} итераций): {csvTime} мс");
Console.WriteLine($"Время JSON сериализации ({iterations} итераций): {jsonTime} мс");

// Замер времени вывода в консоль
sw.Restart();
Console.WriteLine(csv);
sw.Stop();
Console.WriteLine($"Время вывода в консоль: {sw.ElapsedMilliseconds} мс");

// Десериализация
F deserialized = CsvSerializer.Deserialize<F>(csv);
Console.WriteLine($"Десериализация: i1={deserialized.i1}, i2={deserialized.i2}, i3={deserialized.i3},i4={deserialized.i4},i5={deserialized.i5}");
