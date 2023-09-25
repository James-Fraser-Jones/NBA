using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NBA.Models;

namespace NBA.Controllers
{
    public class OverviewController : Controller
    {
		private readonly string overviewQuery = @"
with
----------------------------------------------------------------
--Utility query to simplify all others
GameSingles as (
	select 
	  HomeTeamID as TeamID
	, 1 as Home
	, Stadium
	, GameDateTime
	, HomeScore as Score
	, AwayScore as OpposingScore
	, HomeScore - AwayScore as ScoreDiff
	, MVPPlayerID
	, Team_Player.TeamID as MVPTeamID
	from Games
	left join Team_Player on MVPPlayerID = Team_Player.PlayerID
	left join Teams on HomeTeamID = Teams.TeamID

	union

	select 
	  AwayTeamID as TeamID
	, 0 as Home
	, Stadium
	, GameDateTime
	, AwayScore as Score
	, HomeScore as OpposingScore
	, HomeScore - AwayScore as ScoreDiff
	, MVPPlayerID
	, Team_Player.TeamID as MVPTeamID
	from Games
	left join Team_Player on MVPPlayerID = Team_Player.PlayerID
	left join Teams on HomeTeamID = Teams.TeamID
),
----------------------------------------------------------------
--Name of Team, Team Stadium Name, Team Logo
--Queries: BasicData
BasicData as (
	select 
	  TeamId
	, Name
	, Stadium
	, Logo 
	from Teams
),
----------------------------------------------------------------
--Season MVP on team
--Queries: SeasonMVP
MVPPlayerFreq as (
	select 
	  TeamID
	, MVPPlayerID
	, count(MVPPlayerID) as MVPPlayerFreq
	from GameSingles
	
	where TeamID = MVPTeamID
	group by TeamID, MVPPlayerID
),
RankedMVPPlayerFreq as (
	select *
	, row_number() over (partition by TeamId order by MVPPlayerFreq desc) as DenseRank
	from MVPPlayerFreq
),
SeasonMVP as (
	select 
	  TeamID
	, Name as SeasonMVP
	from RankedMVPPlayerFreq
	left join Players on MVPPlayerID = PlayerID
	where DenseRank = 1
),
----------------------------------------------------------------
--Number of games: Played, Won, Lost, Home, Away
--Queries: GamesWon, GamesLost, GamesHome, GamesAway
GamesWon as (
	select 
	  TeamID
	, count(TeamID) as GamesWon
	from GameSingles 
	where ScoreDiff > 0
	group by TeamID
),
GamesLost as (
	select 
	  TeamID
	, count(TeamID) as GamesLost
	from GameSingles 
	where ScoreDiff < 0
	group by TeamID
),
GamesHome as (
	select 
	  TeamID
	, count(TeamID) as GamesHome
	from GameSingles 
	where Home = 1
	group by TeamID
),
GamesAway as (
	select 
	  TeamID
	, count(TeamID) as GamesAway
	from GameSingles 
	where Home = 0
	group by TeamID
),
----------------------------------------------------------------
--Points in biggest: Win, Loss
--Queries: BiggestWins, BiggestLosses
RankedWins as (
	select 
	  TeamID
	, Score
	, dense_rank() over (partition by TeamID order by ScoreDiff desc) as DenseRank
	from GameSingles
),
BiggestWins as (
	select 
	  TeamID
	, Score as BiggestWin
	from RankedWins
	where DenseRank = 1
),
RankedLosses as (
	select 
	  TeamID
	, Score
	, dense_rank() over (partition by TeamID order by ScoreDiff asc) as DenseRank
	from GameSingles
),
BiggestLosses as (
	select 
	  TeamID
	, Score as BiggestLoss
	from RankedLosses
	where DenseRank = 1
),
----------------------------------------------------------------
--Last Game: Stadium Name, Date
--Queries: RecentGames
RankedDates as (
	select 
	  TeamID
	, Stadium
	, GameDateTime
	, dense_rank() over (partition by TeamID order by GameDateTime desc) as DenseRank
	from GameSingles
),
RecentGames as (
	select 
	  TeamID
	, Stadium as LastGameStadium
	, GameDateTime as LastGameDate
	from RankedDates
	where DenseRank = 1
)
----------------------------------------------------------------
--Final Join
select
  Name
, Stadium
, Logo
, SeasonMVP
, GamesHome + GamesAway as GamesPlayed
, GamesWon
, GamesLost
, GamesHome
, GamesAway
, BiggestWin
, BiggestLoss
, LastGameStadium
, LastGameDate
from BasicData
left join SeasonMVP on BasicData.TeamID = SeasonMVP.TeamID
left join GamesWon on BasicData.TeamID = GamesWon.TeamID
left join GamesLost on BasicData.TeamID = GamesLost.TeamID
left join GamesHome on BasicData.TeamID = GamesHome.TeamID
left join GamesAway on BasicData.TeamID = GamesAway.TeamID
left join BiggestWins on BasicData.TeamID = BiggestWins.TeamID
left join BiggestLosses on BasicData.TeamID = BiggestLosses.TeamID
left join RecentGames on BasicData.TeamID = RecentGames.TeamID
";

        private readonly NbaContext _context;

        public OverviewController(NbaContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
			var nbaContext = _context.Overviews.FromSqlRaw(overviewQuery);

            return View(await nbaContext.ToListAsync());
        }
    }
}
