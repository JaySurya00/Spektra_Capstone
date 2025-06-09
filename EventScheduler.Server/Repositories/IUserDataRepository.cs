namespace EventScheduler.Server.Repositories
{
    public interface IUserDataRepository
    {
        Task<bool> assign_ticket_to_user(string user_email, int ticket_id);
    }
}
