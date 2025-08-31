using System;

namespace BlazorApp;

/*
    Hero classes enum
*/
public enum Classes
{
    TANK = 0,
    DAMAGE = 1,
    SUPPORT = 2,
    FLEX = 3
    //TANK_DAMAGE = 4,
    //TANK_SUPPORT = 5,
    //DAMAGE_SUPPORT = 6
}

public class Match
{
    public Team team1;
    public Team team2;
    public bool ranked;

    public Match()
    {
        team1 = new Team("Red");
        team2 = new Team("Blue");
    }

}

public class Player
{
    public string username { get; set; }
    public bool dummy = false;
    public Classes mainRole;
    public List<Classes> roles;
    public int elo;
    public bool onTeam { get; set; } = false;
    public bool priority { get; set; } = false;

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
    public List<Player> listed = new List<Player>(5);
    public Player? TANK { get; set; }
    public Player? DAMAGE_1 { get; set; }
    public Player? DAMAGE_2 { get; set; }
    public Player? SUPPORT_1 { get; set; }
    public Player? SUPPORT_2 { get; set; }

    public Roster()
    {
        for (int i = 0; i < 5; i++)
        {
            listed.Add(new Player("DUMMY", Classes.FLEX, 0) { dummy = true});
        }
    }

    public void addPlayer(Player player, int slot)
    {
        switch (slot)
        {
            case 0:
                TANK = player;
                break;
            case 1:
                DAMAGE_1 = player;
                break;
            case 2:
                DAMAGE_2 = player;
                break;
            case 3:
                SUPPORT_1 = player;
                break;
            case 4:
                SUPPORT_2 = player;
                break;
        }
        if (slot > 4) return;
        listed[slot] = player;
    }

    public void removePlayer(Player player)
    {
        int idx = listed.IndexOf(player);
        if (idx == -1) return;

        listed[idx] = new Player("DUMMY", Classes.FLEX, 0) { dummy = true };
    }

    public List<Player> getDamage()
    {
        return new List<Player>() { DAMAGE_1, DAMAGE_2 };
    }
    public List<Player> getSupport()
    {
        return new List<Player>() { SUPPORT_1, SUPPORT_2 };
    }

    public List<Player> getPlayers()
    {
        return listed;
    }
}


public class Team
{
    public string name { get; set; }
    public List<Player> members;
    public Player? captain;
    public Roster roster = new Roster();

    public Team(string name)
    {
        this.name = name;
        this.members = new List<Player>();
    }

    public Team()
    {
        this.name = "default";
        this.members = new List<Player>();
    }

    public void addPlayer(Player plr, int slot)
    {
        if (members.Contains(plr)) return;
        plr.onTeam = true;
        Console.Write(plr.dummy +" "+ slot);

        members.Add(plr);
        roster.addPlayer(plr, slot);
    }

    public void setCaptain(Player plr)
    {
        if (!members.Contains(plr)) return;
        captain = plr;
    }

    public void removePlayer(Player plr)
    {
        plr.onTeam = false;
        members.Remove(plr);
        roster.removePlayer(plr);
    }

    public int getTeamAverageElo()
    {
        if (this.roster is null) return 0;
        int count = 0;

        foreach (Player member in this.roster.getPlayers())
        {
            count += member.elo;
        }
        return count / 5;
    }

    public void setRoster(Player tank, Player dps1, Player dps2, Player sup1, Player sup2)
    {
        if (!members.Contains(tank)) return;

        this.roster = new Roster() { TANK = tank, DAMAGE_1 = dps1, DAMAGE_2 = dps2, SUPPORT_1 = sup1, SUPPORT_2 = sup2 };
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
        int flexCount = 0;

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
                case Classes.FLEX:
                    flexCount++;
                    break;
            }
        }
        if (tankCount >= 1 && dpsCount >= 2 && supCount >= 2) return true;

        int missingSlots = Math.Max(0, 1 - tankCount) + Math.Max(0, 2 - dpsCount) + Math.Max(0, 2 - supCount);

        if (flexCount >= missingSlots) return true;

        return false;
    }

}

/*
    Manages picking captains, team matchups etc.
*/

public class GameManager
{
    public List<Match> matchPool;
    public List<Player> playerPool;

    public GameManager()
    {
        matchPool = new List<Match>();
        playerPool = new List<Player>();
    }

    public Player? getPlayerFromUser(string user)
    {
        Player? plr = playerPool.FirstOrDefault(p => p.username == user);
        return plr;
    }

    public void addPlayer(string name, Classes role, int elo)
    {
        if (getPlayerFromUser(name) != null) return; // Stops duplicate names
        Player newPlayer = new Player(name, role, elo){dummy=false};

        playerPool.Add(newPlayer);
    }

    public void closeMatch(Match m)
    {
        int idx = matchPool.IndexOf(m);
        if (idx == -1) return;

        // Removing onTeam flags to make players searchable
        foreach (Player plr in m.team1.members)
        {
            plr.onTeam = false;
        }
        foreach (Player plr in m.team2.members)
        {
            plr.onTeam = false;
        }
        matchPool.Remove(m);
    }

    public void removePlayer(Player plr)
    {
        playerPool.Remove(plr);
    }

    public void createEmptyMatch()
    {
        Match newMatch = new Match();
        matchPool.Add(newMatch);
    }


    /*
        Returns a list of players marked with "onTeam" as false
    */

    public List<Player> getFreePlayers()
    {
        List<Player> freePlayers = new List<Player>();
        foreach (Player plr in playerPool)
        {
            if (!plr.onTeam) freePlayers.Add(plr);
        }
        return freePlayers;
    }

    public void makeRolelessMatch(bool balanced, bool ranked)
    {
        List<Team> teams = new List<Team>();
        List<Player> freePlayers = getFreePlayers();
        if (freePlayers.Count < 10) return;

        Team team1 = new Team(){name = "red"};

        if (balanced)
        {

        }
        else
        {
            //for
        }

    }

    public void makeRoleMatch(bool balanced, bool ranked)
    {
        List<Team> teams = new List<Team>();
        List<Player> freePlayers = getFreePlayers();
        if (freePlayers.Count < 10) return; 

    }

}
