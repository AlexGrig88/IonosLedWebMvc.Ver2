namespace IonosLedWebMvc.Ver2.Models.Entities
{
    public class AccessRights
    {
        public uint Id { get; set; }

        public bool AccessRoom1 { get; set; }

        public bool AccessRoom2 { get; set; }

        public bool AccessRoom3 { get; set; }

        public bool AccessRoom4 { get; set; }

        public bool AccessRoom5 { get; set; }

        public bool AccessRoom6 { get; set; }

        public bool AccessRoom7 { get; set; }

        public bool AccessRoom8 { get; set; }

        public UserAuth? UserAuth { get; set; }
        public int UserAuthId { get; set; }
    }
}
