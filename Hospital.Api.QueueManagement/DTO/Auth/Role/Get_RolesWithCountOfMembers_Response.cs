namespace Inventory.Api.DTO.Auth.Role
{
    public class Get_RolesWithCountOfMembers_Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MembersCount { get; set; }
        public string ActionsCount { get; set; }
    }
}
