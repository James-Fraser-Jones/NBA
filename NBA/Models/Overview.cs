using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NBA.Models;

[Keyless]
public partial class Overview
{
    [Display(Name = "Team Name")]
    public string Name { get; set; } = null!;

    public string Stadium { get; set; } = null!;

    public string? Logo { get; set; }

    [Display(Name = "Season MVP")]
    public string SeasonMVP { get; set; } = null!;

    [Display(Name = "Games Played")]
    public int GamesPlayed { get; set; }

    [Display(Name = "Games Won")]
    public int GamesWon { get; set; }

    [Display(Name = "Games Lost")]
    public int GamesLost { get; set; }

    [Display(Name = "Games Home")]
    public int GamesHome { get; set; }

    [Display(Name = "Games Away")]
    public int GamesAway { get; set; }

    [Display(Name = "Biggest Win")]
    public int BiggestWin { get; set; }

    [Display(Name = "Biggest Loss")]
    public int BiggestLoss { get; set; }

    [Display(Name = "Last Game Stadium")]
    public string LastGameStadium { get; set; } = null!;

    [Display(Name = "Last Game Date")]
    [DataType(DataType.Date)]
    public DateTime LastGameDate { get; set; }
}
