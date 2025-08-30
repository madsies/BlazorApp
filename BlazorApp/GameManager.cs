using System;

namespace BlazorApp;

/*
    Hero classes enum
*/
public enum Classes
{
    TANK = 0,
    DAMAGE = 1,
    SUPPORT = 2
}

public class Player
{
    public required string username { get; set; }
    public Classes mainRole;
    public List<Classes> roles;
    public int elo;

    public Player(string name, Classes mainRole, int elo)
    {
        username = name;
        this.mainRole = mainRole;
        this.elo = elo;
        roles = new List<Classes>();
    }

}

public class Roster
{
    public required Player TANK { get; set; }
    public required Player DAMAGE_1 { get; set; }
    public required Player DAMAGE_2 { get; set; }
    public required Player SUPPORT_1 { get; set; }
    public required Player SUPPORT_2 { get; set; }

    public List<Player> getDamage()
    {
        return new List<Player>() { DAMAGE_1, DAMAGE_2 };
    }
    public List<Player> getSupport()
    {
        return new List<Player>() { SUPPORT_1, SUPPORT_2 };
    }

    public List<Player> GetPlayers()
    {
        return new List<Player>() {TANK, DAMAGE_1, DAMAGE_2, SUPPORT_1, SUPPORT_2 };
    }
}


public class Team
{
    public required string name { get; set; }
    public List<Player> members;
    public Player? captain;
    public Roster? roster = null;

    public Team(string name)
    {
        this.name = name;
        this.members = new List<Player>();
    }

    public void addPlayer(Player plr)
    {
        if (members.Contains(plr)) return;

        members.Add(plr);
    }

    public void removePlayer(Player plr)
    {
        members.Remove(plr);
    }

    public setRoster()
    {
        
    }


    public int getTeamSize()
    {
        return members.Count();
    }

    /*
        Checks if the team has a valid team config
        i.e. Tank, 2 DPS, 2 Support
    */

    public bool isValidTeam()
    {
        if (getTeamSize() < 5) return false;

        int tankCount = 0;
        int supCount = 0;
        int dpsCount = 0;

        foreach (Player member in members)
        {
            switch (member.mainRole)
            {
                case Classes.TANK:
                    tankCount++;
                    break;
                case Classes.DAMAGE:
                    dpsCount++;
                    break;
                case Classes.SUPPORT:
                    supCount++;
                    break;
            }
        }
        if (tankCount >= 1 && dpsCount >= 2 && supCount >= 2) return true;

        return false;
    }

}

public class GameManager
{

}
