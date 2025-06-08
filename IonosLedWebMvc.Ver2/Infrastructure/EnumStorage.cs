namespace IonosLedWebMvc.Ver2.Infrastructure
{
    public static class EnumStorage
    {
        public enum Group
        {
            LED_LIGHT, DRONE
        }

        public enum UserRole
        {
            EMPLOYEER, ADMIN
        }

        public static string GetRuStringGroup(EnumStorage.Group group) =>
            group switch
            {
                Group.LED_LIGHT => "Светильники",
                Group.DRONE => "Дроны",
                _ => throw new InvalidOperationException()
            };
        public static EnumStorage.Group GetGroupFromRuStr(string group) =>
            group switch
            {
                "Светильники" => Group.LED_LIGHT,
                "Дроны" => Group.DRONE,
                _ => throw new InvalidOperationException()
            };

        public static string GetRuStringRole(EnumStorage.UserRole role) =>
            role switch
            {
                UserRole.ADMIN => "Администратор",
                UserRole.EMPLOYEER => "Сотрудник",
                _ => throw new InvalidOperationException()
            };
    }
}
