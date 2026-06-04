using DevExpress.Dialogs.Core.View;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
namespace GH.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly object CacheLock = new object();
        //using GTranslate.Translators;
        //private static YandexTranslator? translator = new YandexTranslator() ;
        // Кэш для хранения скомпилированных методов копирования свойств
        private static readonly Dictionary<(Type, Type), Action<object, object>> CopyCache = new();
        /// <summary>
        /// Рекурсивно ищет первый дочерний элемент, реализующий указанный интерфейс.
        /// </summary>
    private static Control FindDescendant(Control parent, System.Type interfaceType)
        {
            // Проверяем всех прямых потомков текущего контейнера
            foreach (Control child in parent.Controls)
            {
                // Если текущий элемент реализует нужный интерфейс — возвращаем его
                if (interfaceType.IsInstanceOfType(child))
                {
                    return child;
                }
                // Если у элемента есть свои дети, идем вглубь по дереву
                if (child.HasChildren)
                {
                    Control result = FindDescendant(child, interfaceType);
                    if (result != null)
                    {
                        // Если в поддереве что-то нашли — возвращаем результат наверх
                        return result;
                    }
                }
            }
            // Ничего не нашли в этой ветке
            return null;
        }
        /// <summary>
        /// Обновляет список объектов назначения данными из списка источников по ключу.
        /// </summary>
        public static void AssignFrom<TSource, TDest>(this List<TDest> destinationList, List<TSource> sourceList, string linkedBy)
            where TSource : class where TDest : class
        {
            if (sourceList == null || destinationList == null || string.IsNullOrWhiteSpace(linkedBy))
                return;
            // Группируем источники по ключу для быстрого поиска
            var sourceDict = sourceList.Where(s => s != null).GroupBy(s => GetPropertyValue(s, linkedBy)).ToDictionary(g => g.Key, g => g.First());
            var copier = GetCopier<TSource, TDest>();
            // Перебираем объекты назначения
            foreach (var destItem in destinationList.ToList())
            {
                if (destItem == null)
                    continue;
                var key = GetPropertyValue(destItem, linkedBy);
                if (sourceDict.TryGetValue(key, out var sourceItem))
                {
                    copier(sourceItem, destItem);
                }
            }
        }
        /// <summary>
        /// Находит первый элемент управления, реализующий интерфейс T, во всех открытых формах.
        /// Предполагается, что такой элемент существует в единственном экземпляре.
        /// </summary>
        /// <typeparam name = "T">Тип интерфейса для поиска.</typeparam>
        /// <returns>Найденный элемент управления или null, если ничего не найдено.</returns>
        public static T FindImplementation<T>()
            where T : class
        {
            // Перебираем все открытые окна приложения
            foreach (Form form in Application.OpenForms)
            {
                // Начинаем рекурсивный поиск с корня каждой формы
                var foundControl = FindDescendant(form, typeof(T));
                if (foundControl is T intf)
                {
                    // Как только нашли — сразу возвращаем результат, не продолжая поиск
                    return intf;
                }
            }
            return (T)default;
        }
        /// <summary>
        /// Копирует значения свойств из объекта-источника в объект-приёмник.
        /// </summary>
        public static void AssignFrom<TSource, TDest>(this TDest destination, TSource source)
            where TDest : class where TSource : class
        {
            if (source == null || destination == null)
                return;
            var copier = GetCopier<TSource, TDest>();
            copier(source, destination);
        }
        /// <summary>
        /// Создает новый объект типа TConvert и копирует в него значения свойств из исходного объекта.
        /// </summary>
        public static TConvert ConvertTo<TConvert>(this object entity)
            where TConvert : new()
        {
            if (entity == null)
                return new TConvert();
            var copier = GetCopier(entity.GetType(), typeof(TConvert));
            var destination = new TConvert();
            copier(entity, destination);
            return destination;
        }
    private static object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
        }

    private static Action<object, object> GetCopier<TSource, TDest>()
        {
            return GetCopier(typeof(TSource), typeof(TDest));
        }
        //private static readonly Dictionary<(Type, Type), Action<object, object>> CopyCache = new();
    private static Action<object, object> GetCopier(Type sourceType, Type destType)
        {
            var key = (sourceType, destType);
            // 1. Быстрая проверка без блокировки
            if (CopyCache.TryGetValue(key, out var copier))
            {
                return copier;
            }
            // 2. Блокировка для создания объекта (Double-checked locking)
            lock (CacheLock)
            {
                // Проверяем еще раз внутри блокировки на случай, если другой поток уже добавил значение
                if (!CopyCache.TryGetValue(key, out copier))
                {
                    copier = BuildCopier(sourceType, destType); // Здесь должен быть ваш try-catch из примера выше
                    CopyCache[key] = copier;
                }
            }
            return copier;
        }
        //private static Action<object, object> GetCopier(Type sourceType, Type destType)
        //{
        //    var key = (sourceType, destType);
        //    if (!CopyCache.TryGetValue(key, out var copier))
        //    {
        //        copier = BuildCopier(sourceType, destType);
        //        CopyCache[key] = copier;
        //    }
        //    return copier;
        //}

    private static Action<object, object> BuildCopier(Type sourceType, Type destType)
        {
            var sourceParam = Expression.Parameter(typeof(object), "source");
            var destParam = Expression.Parameter(typeof(object), "dest");
            // Приводим параметры к нужным типам для работы со свойствами
            var sourceTyped = Expression.Convert(sourceParam, sourceType);
            var destTyped = Expression.Convert(destParam, destType);
            var bindings = new List<MemberBinding>();
            // Находим все совпадающие свойства для маппинга
            foreach (var sourceProp in sourceType.GetProperties().Where(p => p.CanRead))
            {
                var destProp = destType.GetProperty(sourceProp.Name);
                if (destProp != null && destProp.CanWrite && destProp.PropertyType == sourceProp.PropertyType)
                {
                    // Создаем биндинг: DestProp = SourceProp
                    bindings.Add(Expression.Bind(destProp, Expression.Property(sourceTyped, sourceProp)));
                }
            }
            // Если нет свойств для копирования, возвращаем пустое действие
            if (!bindings.Any())
                return (src, dst) =>
                {
                };
            // Создаем выражение для инициализации объекта назначения через MemberInit
            var memberInit = Expression.MemberInit(Expression.New(destType), bindings);
            // Создаем лямбду: (object source, object dest) => { dest = new Dest { Prop1 = ((Source)source).Prop1, ... } }
            var lambda = Expression.Lambda<Action<object, object>>(memberInit, sourceParam, destParam);
            return lambda.Compile();
        }
        /// <summary>
        /// Получает значение атрибута [Display(Name="...")] у элемента перечисления.
        /// </summary>
    public static string GetDisplayValue(this Enum value)
        {
            return value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? value.ToString();
        }
    public static int Int(this InfoType value)
        {
            return (int)value;
        }
        /// <summary>
        /// Генерирует MD5-хеш строки для использования в качестве ключа идемпотентности.
        /// </summary>
    public static string ToIdempotencyKey(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(value);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
