namespace FestivalScheduler.Data
{
    public class UserNode
    {
        /// <summary>
        /// global id
        /// schema: UserNode/{Section}/{ShortCut}
        /// </summary>
        public string NodeId { get; set; }
        /// <summary>
        /// Loginname
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Loginpassword
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// mobile number of the user
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// section of the user
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// permission level
        /// </summary>
        public PermissionGroups PermissionLevel { get; set; }
        /// <summary>
        /// short version of the name
        /// </summary>
        public string ShortCut { get; set; }
        /// <summary>
        /// full name
        /// </summary>
        public string Name { get; set; }
    }
}
