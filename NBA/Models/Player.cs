using System;
using System.Collections.Generic;

namespace NBA.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual TeamPlayer? TeamPlayer { get; set; }
}
