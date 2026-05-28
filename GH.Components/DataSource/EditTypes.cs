using System.ComponentModel.DataAnnotations;
using static GH.Components.EditTypesConst;
namespace GH.Components
{
    public enum EditTypes
    {
        [Map(EditCategory, EditPath, "add", Insert), Display(Name = "Добавить", Description = "Добавить новую запись")]
        Insert,
        [Map(EditCategory, EditPath, "editname", Edit), Display(Name = "Изменить", Description = "Изменить запись")]
        Edit,
        [Map(EditCategory, EditPath, "remove", Delete), Display(Name = "Удалить", Description = "Удалить запись")]
        Delete,
        [Map(SaveCategory, EditPath, "apply", Save), Display(Name = "Сохранить", Description = "Сохранить изменения")]
        Save,
        [Map(SaveCategory, DocumentPath, "bofolder", Save), Display(Name = "Закрыть", Description = "Закрыть документ")]
        CloseDocument,
        [Map(SaveCategory, DocumentPath, "bopermission", Save), Display(Name = "Открыть", Description = "Открыть документ")]
        OpenDocument,
        [Map(SaveCategory, EditPath, "cancel", Cancel), Display(Name = "Отменить", Description = "Отменить изменения")]
        Cancel,
        [Map(ViewCategory, EditPath, "convert", RefreshAll), Display(Name = "Обновить всё", Description = "Обновить все записи")]
        RefreshAll,
        [Map(ViewCategory, ViewPath, "exporttoxlsx", Preview), Display(Name = "Вывести для печати", Description = "Вывести для печати")]
        Preview,
        [Map(AdditionCategory, AdditionPath, "initialstate", Additional), Display(Name = "Дополнительно", Description = "Создать Дополнительно")]
        Additional
    }
}
