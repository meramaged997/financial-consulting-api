using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Startawy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cash_flow_forecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForecastMonths = table.Column<int>(type: "int", nullable: false),
                    InitialCashBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyRevenueTrend = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyExpenseTrend = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GrowthRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Insights = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrowthRecommendations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectedRunway = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_flow_forecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "chat_sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "consultation_requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedExpertId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpertNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultation_requests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Expenses = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NetProfit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CashBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BurnRate = table.Column<double>(type: "float", nullable: false),
                    CustomerCount = table.Column<int>(type: "int", nullable: false),
                    CustomerAcquisitionCost = table.Column<double>(type: "float", nullable: false),
                    CustomerLifetimeValue = table.Column<double>(type: "float", nullable: false),
                    ChurnRate = table.Column<double>(type: "float", nullable: false),
                    PredictiveInsights = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_snapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "financial_statements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GrossRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CostOfGoodsSold = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OperatingExpenses = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NetIncome = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAssets = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalLiabilities = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OperatingCashFlow = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InvestingCashFlow = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FinancingCashFlow = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AnalysisNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformanceForecast = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskAssessment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_financial_statements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "market_researches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetMarket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeographicScope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedMarketSize = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MarketGrowthRate = table.Column<double>(type: "float", nullable: false),
                    CompetitorAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrendAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpportunityInsights = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedReport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_market_researches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "marketing_campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetAudience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Strategy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelRecommendations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentCalendar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketing_campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "package",
                columns: table => new
                {
                    package_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_package", x => x.package_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "monthly_forecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashFlowForecastId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ProjectedRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProjectedExpenses = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CumulativeCashBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ConfidenceScore = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monthly_forecasts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_monthly_forecasts_cash_flow_forecasts_CashFlowForecastId",
                        column: x => x.CashFlowForecastId,
                        principalTable: "cash_flow_forecasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatSessionId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_chat_sessions_ChatSessionId",
                        column: x => x.ChatSessionId,
                        principalTable: "chat_sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "competitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketResearchId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strengths = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weaknesses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketShareEstimate = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_competitors_market_researches_MarketResearchId",
                        column: x => x.MarketResearchId,
                        principalTable: "market_researches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "market_trends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketResearchId = table.Column<int>(type: "int", nullable: false),
                    TrendName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImpactScore = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_market_trends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_market_trends_market_researches_MarketResearchId",
                        column: x => x.MarketResearchId,
                        principalTable: "market_researches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "basic",
                columns: table => new
                {
                    package_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    unlimited_ai = table.Column<bool>(type: "bit", nullable: true),
                    unlimited_analysis = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basic", x => x.package_id);
                    table.ForeignKey(
                        name: "FK_basic_package_package_id",
                        column: x => x.package_id,
                        principalTable: "package",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "budget_analyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Recommendations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptimizationPlan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalysisDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget_analyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_budget_analyses_package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "package",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "free",
                columns: table => new
                {
                    package_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    free_trial = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_free", x => x.package_id);
                    table.ForeignKey(
                        name: "FK_free_package_package_id",
                        column: x => x.package_id,
                        principalTable: "package",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "premium",
                columns: table => new
                {
                    package_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    follow_up_duration = table.Column<int>(type: "int", nullable: true),
                    consultant_review = table.Column<bool>(type: "bit", nullable: true),
                    consultant_support = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_premium", x => x.package_id);
                    table.ForeignKey(
                        name: "FK_premium_package_package_id",
                        column: x => x.package_id,
                        principalTable: "package",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "admin",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    admin_level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    access_scope = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_admin_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_bot",
                columns: table => new
                {
                    chat_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    chat_limit = table.Column<int>(type: "int", nullable: true),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sys_response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_bot", x => x.chat_id);
                    table.ForeignKey(
                        name: "FK_chat_bot_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "consultant",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    years_of_exp = table.Column<int>(type: "int", nullable: true),
                    certificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    availability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consultant", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_consultant_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "startup_founder",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    business_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business_sector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    founding_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_startup_founder", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_startup_founder_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscription",
                columns: table => new
                {
                    subs_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    trial_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    package_id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription", x => x.subs_id);
                    table.ForeignKey(
                        name: "FK_subscription_package_package_id",
                        column: x => x.package_id,
                        principalTable: "package",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_subscription_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "budget_line_items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetAnalysisId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlannedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget_line_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_budget_line_items_budget_analyses_BudgetAnalysisId",
                        column: x => x.BudgetAnalysisId,
                        principalTable: "budget_analyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageStartupFounder",
                columns: table => new
                {
                    FoundersUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PackagesPackageId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageStartupFounder", x => new { x.FoundersUserId, x.PackagesPackageId });
                    table.ForeignKey(
                        name: "FK_PackageStartupFounder_package_PackagesPackageId",
                        column: x => x.PackagesPackageId,
                        principalTable: "package",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageStartupFounder_startup_founder_FoundersUserId",
                        column: x => x.FoundersUserId,
                        principalTable: "startup_founder",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    session_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    founder_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    consultant_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.session_id);
                    table.ForeignKey(
                        name: "FK_session_consultant_consultant_id",
                        column: x => x.consultant_id,
                        principalTable: "consultant",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_session_startup_founder_founder_id",
                        column: x => x.founder_id,
                        principalTable: "startup_founder",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "startawy_reports",
                columns: table => new
                {
                    report_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    upload_date = table.Column<DateOnly>(type: "date", nullable: true),
                    founder_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_startawy_reports", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_startawy_reports_startup_founder_founder_id",
                        column: x => x.founder_id,
                        principalTable: "startup_founder",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    trans_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    trans_date = table.Column<DateOnly>(type: "date", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    payment_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    subs_id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.trans_id);
                    table.ForeignKey(
                        name: "FK_transaction_subscription_subs_id",
                        column: x => x.subs_id,
                        principalTable: "subscription",
                        principalColumn: "subs_id");
                    table.ForeignKey(
                        name: "FK_transaction_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_budget_analyses_PackageId",
                table: "budget_analyses",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_budget_line_items_BudgetAnalysisId",
                table: "budget_line_items",
                column: "BudgetAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_chat_bot_user_id",
                table: "chat_bot",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatSessionId",
                table: "ChatMessages",
                column: "ChatSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_competitors_MarketResearchId",
                table: "competitors",
                column: "MarketResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_market_trends_MarketResearchId",
                table: "market_trends",
                column: "MarketResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_monthly_forecasts_CashFlowForecastId",
                table: "monthly_forecasts",
                column: "CashFlowForecastId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageStartupFounder_PackagesPackageId",
                table: "PackageStartupFounder",
                column: "PackagesPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_session_consultant_id",
                table: "session",
                column: "consultant_id");

            migrationBuilder.CreateIndex(
                name: "IX_session_founder_id",
                table: "session",
                column: "founder_id");

            migrationBuilder.CreateIndex(
                name: "IX_startawy_reports_founder_id",
                table: "startawy_reports",
                column: "founder_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscription_package_id",
                table: "subscription",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscription_user_id",
                table: "subscription",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_subs_id",
                table: "transaction",
                column: "subs_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_user_id",
                table: "transaction",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin");

            migrationBuilder.DropTable(
                name: "basic");

            migrationBuilder.DropTable(
                name: "budget_line_items");

            migrationBuilder.DropTable(
                name: "chat_bot");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "competitors");

            migrationBuilder.DropTable(
                name: "consultation_requests");

            migrationBuilder.DropTable(
                name: "dashboard_snapshots");

            migrationBuilder.DropTable(
                name: "financial_statements");

            migrationBuilder.DropTable(
                name: "free");

            migrationBuilder.DropTable(
                name: "market_trends");

            migrationBuilder.DropTable(
                name: "marketing_campaigns");

            migrationBuilder.DropTable(
                name: "monthly_forecasts");

            migrationBuilder.DropTable(
                name: "PackageStartupFounder");

            migrationBuilder.DropTable(
                name: "premium");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "startawy_reports");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "budget_analyses");

            migrationBuilder.DropTable(
                name: "chat_sessions");

            migrationBuilder.DropTable(
                name: "market_researches");

            migrationBuilder.DropTable(
                name: "cash_flow_forecasts");

            migrationBuilder.DropTable(
                name: "consultant");

            migrationBuilder.DropTable(
                name: "startup_founder");

            migrationBuilder.DropTable(
                name: "subscription");

            migrationBuilder.DropTable(
                name: "package");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
