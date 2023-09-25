using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NBA.Models;

[Keyless]
public partial class Overview
{
    public string Name { get; set; } = null!;

    public string Stadium { get; set; } = null!;

    public string? Logo { get; set; }

    public string SeasonMVP { get; set; } = null!;

    public int GamesPlayed { get; set; }

    public int GamesWon { get; set; }

    public int GamesLost { get; set; }

    public int GamesHome { get; set; }

    public int GamesAway { get; set; }

    public int BiggestWin { get; set; }

    public int BiggestLoss { get; set; }

    public string LastGameStadium { get; set; } = null!;

    public DateTime LastGameDate { get; set; }
}
