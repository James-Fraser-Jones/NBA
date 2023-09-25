using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NBA.Models;

public partial class Game
{
    public int GameId { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public DateTime GameDateTime { get; set; }

    public int HomeScore { get; set; }

    public int AwayScore { get; set; }

    public int MvpplayerId { get; set; }

    public virtual Team AwayTeam { get; set; } = null!;

    public virtual Team HomeTeam { get; set; } = null!;

    public virtual Player Mvpplayer { get; set; } = null!;
}
