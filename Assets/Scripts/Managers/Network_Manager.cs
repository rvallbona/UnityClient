using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class Network_Manager : MonoBehaviour
{
    public static Network_Manager _NETWORK_MANAGER;

    [SerializeField] LoginScript loginScript;

    private TcpClient socket;
    private NetworkStream stream;

    private StreamWriter writer;
    private StreamReader reader;

    private bool connected = false;
    [SerializeField] private bool isLoggedIn = false;

    //Ip del servidor
    const string host = "localhost";
    const int port = 6543;

    private void Awake()
    {
        if (_NETWORK_MANAGER != null && _NETWORK_MANAGER != this)
        {
            Destroy(_NETWORK_MANAGER);
        }
        else
        {
            _NETWORK_MANAGER = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Update()
    {
        //Si estoy conectado reviso si existen datos
        if (connected)
        {
            //Si hay datos disponibles para leer
            if (stream.DataAvailable)
            {
                //Leo una linea de datos
                string data = reader.ReadLine();

                //Si los datos no son nulos los trabajo
                if (data != null)
                {
                    ManageData(data);
                }
            }
        }
    }
    public void ConnectToServer(string name, string passwd)
    {
        try
        {
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            //Si hay streaming de datos hay conexion
            connected = true;

            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio 0 con nick y ususario separados por / ya que son los valores que he definido en el servidor
            writer.WriteLine("0" + "/" + name + "/" + passwd);

            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    private void ManageData(string data)
    {
        string[] parameters = data.Split('/');
        //Si recibo ping devuelvo 1 como respuesta al servidor
        if (data == "ping")
        {
            writer.WriteLine("1");
            writer.Flush();
        }
        else if (parameters[0] == "CorrectRaces")
        {
            Debug.Log("Razas correctas");
            AddRace(int.Parse(parameters[1]), parameters[2], int.Parse(parameters[3]), int.Parse(parameters[4]), int.Parse(parameters[5]), int.Parse(parameters[6]), int.Parse(parameters[7]), int.Parse(parameters[8]));
            writer.Flush();
            if (int.Parse(parameters[8]) == Session_Manager._SESSION_MANAGER.Races.Count)
            {
                Scene_Manager.Instance.LoadScene(1);
            }
        }
        else if (parameters[0] == "CorrectLogin")
        {
            Debug.Log("Recibido logeo");
            isLoggedIn = true;
            loginScript.ClearInputs(false);
            SetSessionHandlerUser(int.Parse(parameters[1]), parameters[2], parameters[3], int.Parse(parameters[4]));
            writer.Flush();
        }
        else if (parameters[0] == "IncorrectLogin")
        {
            isLoggedIn = false;
            loginScript.ClearInputs(true);
            Debug.Log("Login incorrecto");
        }
        else if (parameters[0] == "CorrectRegister")
        {
            Debug.Log("Registro correcto");
            writer.Flush();
        }
    }

    private void SetSessionHandlerUser(int id, string name, string password, int idRace)
    {
        Users data = new Users();

        data.id_user = id;
        data.name = name;
        data.password = password;
        data.id_race_user = idRace;

        Session_Manager._SESSION_MANAGER.CurrentUser = data;
    }

    private void AddRace(int id, string name, int maxHealth, int damage, int velocity, int jumpForce, int cadence, int countRaces)
    {
        Race race = new Race();

        race.IdRace = id;
        race.Name = name;
        race.MaxHealth = maxHealth;
        race.Damage = damage;
        race.Velocity = velocity;
        race.JumpForce = jumpForce;
        race.Cadence = cadence;

        race.CountRaces = countRaces;

        Session_Manager._SESSION_MANAGER.Races.Add(id, race);
    }

    public void Register(string name, string password, int id_race_user)
    {
        try
        {
            //Realizo conexion con el servidor
            socket = new TcpClient(host, port);

            //Almaceno el canal de envio y recepcion de datos
            stream = socket.GetStream();

            //Indicamos que tenemos conexion
            connected = true;

            //Genero clases para escribir y leer los datos del canal de datos
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio valores de login
            writer.WriteLine("2" + "/" + name + "/" + password + "/" + id_race_user.ToString());
            writer.Flush();
            Debug.Log("nickname: " + name + " and password: " + password);
        }
        catch { connected = false; }
    }

    public void GetRaces()
    {
        try
        {
            //Realizo conexion con el servidor
            socket = new TcpClient(host, port);

            //Almaceno el canal de envio y recepcion de datos
            stream = socket.GetStream();

            //Indicamos que tenemos conexion
            connected = true;

            //Genero clases para escribir y leer los datos del canal de datos
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio valores de Razas
            writer.WriteLine("3");
            writer.Flush();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error: " + ex.Message);
            Scene_Manager.Instance.Exit();
            connected = false;
        }

    }

    public bool GetIsLoggedInt() => isLoggedIn;
}