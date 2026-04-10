namespace startawy.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IBudgetRepository          Budgets          { get; }
    IMarketResearchRepository  MarketResearches { get; }
    IFinancialRepository       Financials       { get; }
    ICashFlowRepository        CashFlows        { get; }
    IMarketingRepository       Campaigns        { get; }
    IDashboardRepository       Dashboards       { get; }
    IConsultationRepository    Consultations    { get; }
    IChatRepository            Chats            { get; }

    // IPackageRepository is defined in Startawy.Domain.Interfaces and
    // consumed directly where needed; it is not part of the core unit of work.

    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task      BeginTransactionAsync(CancellationToken ct = default);
    Task      CommitAsync(CancellationToken ct = default);
    Task      RollbackAsync(CancellationToken ct = default);
}
