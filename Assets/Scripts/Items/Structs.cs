using System.Data;
using UnityEngine;
using System;

[Serializable]
public struct Users
{
    public int id_user;
    public string name;
    public string password;
    public int id_race_user;

    public Users(IDataReader dataReader)
    {
        id_user = dataReader.GetInt32(0);
        name = dataReader.GetString(1);
        password = dataReader.GetString(2);
        id_race_user = dataReader.GetInt32(3);
    }
}

[Serializable]
public struct Race
{
    [SerializeField] int id_race;
    [SerializeField] string name;
    [SerializeField] int maxHealth;
    [SerializeField] int damage;
    [SerializeField] int velocity;
    [SerializeField] int jumpForce;
    [SerializeField] int cadence;
    [SerializeField] int countRaces;

    public Race(IDataReader dataReader)
    {
        id_race = dataReader.GetInt32(0);
        name = dataReader.GetString(1);
        maxHealth = dataReader.GetInt32(2);
        damage = dataReader.GetInt32(3);
        velocity = dataReader.GetInt32(4);
        jumpForce = dataReader.GetInt32(5);
        cadence = dataReader.GetInt32(6);
        countRaces = dataReader.GetInt32(7);
    }

    //------ GETTERS
    public int IdRace { get { return id_race; } set { id_race = value; } }
    public string Name { get { return name; } set { name = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int Velocity { get { return velocity; } set { velocity = value; } }
    public int JumpForce { get { return jumpForce; } set { jumpForce = value; } }
    public int Cadence { get { return cadence; } set { cadence = value; } }
    public int CountRaces { get { return countRaces; } set { countRaces = value; } }

}
