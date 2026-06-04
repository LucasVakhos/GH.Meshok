namespace GH.Components
{
    public enum Category { Connection, Security, User, DateInterval }

    public class DbConnectionProperty : UpdatablePropertyAttribute
    {
        public Category Category { get; set; }

    public override string Group
        {
            get
            {
                switch (Category)
                {
                    case Category.Connection:
                        return "Соединение";
                    case Category.Security:
                        return "Безопасность";
                    case Category.User:
                        return "Логин";
                    case Category.DateInterval:
                        return "Интервалы дат выборки данных для снятия и выставления товара";
                    default:
                        break;
                }
                return base.Group;
            }
            set
            {
                base.Group = value;
            }
        }
    }
}
