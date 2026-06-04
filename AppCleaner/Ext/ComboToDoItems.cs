//ComboItemsTypes
#nullable disable
namespace AppCleaner
{
    public enum ComboToDoItems
    {
        [ComboItem(Name = "Удалить пустые строки...")]
        DeleteEmpty,
        [ComboItem(Name = "Удалить строки #region #endregion...")]
        DeleteRegionRows,
        [ComboItem(Name = "Найти и заменить...")]
        FindAndReplace,
        [ComboItem(Name = "Найти Class или значение в Class и добавить в папку проекта...")]
        FindValueOrClassAddScaveToProject,
        [ComboItem(Name = "Удалить лишние ссылки на namespace...")]
        ClearNameSpace,
        [ComboItem(Name = "Собрать все namespace проекта...")]
        CollectAllNameSpaces,
        [ComboItem(Name = "Собрать нужные using Packages проекта...")]
        CollectUsingPackages,
        [ComboItem(Name = "Удалить *.bak-файлы...")]
        DeleteBakFiles,
        [ComboItem(Name = "Удалить файлы не входящие в проект...", SearchLabel = "Cканировать Project:")]
        DeleteNonProjectFiles,
        [ComboItem(Name = "Синхронизировать файл проекта с образцом файла проекта ...", SearchLabel = "Cканировать Project:", PlaceLabel = "Образец Project:")]
        SyncProjectFileWithSample,
        [ComboItem(Name = "Конвертировать старый csproj в SDK-style...", SearchLabel = "Старый Project:", PlaceLabel = "Новый Project:")]
        ConvertOldCsprojToSdkStyle,
        [ComboItem(Name = "Перевести английский текст на русский в файлах проекта (включая комментарии)...")]
        TranslateEnToRu,
        [ComboItem(Name = "Нормализовать сигнатуры методов...")]
        NormalizeMethodSignatures,
        [ComboItem(Name = "Восстановление файлов CSharp из Bak...")]
        RestoreCSharpFilesFromBak,
        [ComboItem(Name = "Восстановление using в указанном проекте...", SearchLabel = "Recovery project:", PlaceLabel = "Sample project:")]
        RestoreMissingUsings,
        [ComboItem(Name = "Добавить комментарий /*Path File*/ к файлам .сs в папке...")]
        AddFilePathCommentToCsFiles
    }
}
