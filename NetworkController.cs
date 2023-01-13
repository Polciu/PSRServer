using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSRServer
{
    internal class NetworkController
    {
        TcpListener server = null;
        TcpClient clientSocket = default(TcpClient);
        Int32 port = 55001;
        IPAddress localAddress = IPAddress.Parse("127.0.0.1");
        int counter = 0;
        ProgramController programController = null;


        public NetworkController()
        {
           
        }

        public NetworkController(ref ProgramController pc)
        {
            programController = pc;
        }

        public void initialize()
        {
            try
            {
                server = new TcpListener(localAddress, port);
                server.Start();
                Console.WriteLine(" >> " + "Server Started!");


                counter = 0;

                while(true)
                {
                    counter += 1;
                    clientSocket = server.AcceptTcpClient();
                    Console.WriteLine(" >> " + " Client No:" + Convert.ToString(counter) + " started!");
                    ClientHandler client = new ClientHandler(ref programController);
                    client.startClient(clientSocket, Convert.ToString(counter));
                }

                clientSocket.Close();
                server.Stop();
                Console.WriteLine(" >> " + "exit");
                Console.ReadLine();

            }
            catch(SocketException e)
            {
                Console.WriteLine("Socket exception: {0}", e);
            }
            finally
            {
                // Zakonczenie nasluchiwania
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }


        public class ClientHandler
        {
            TcpClient clientSocket;
            string clNo;
            ProgramController pc;
            NetworkStream networkStream = null;


            public ClientHandler(ref ProgramController programController)
            {
                this.pc = programController;
            }

            public void startClient(TcpClient inClientSocket, String clineNo)
            {
                this.clientSocket = inClientSocket;
                this.clNo = clineNo;
                Thread ctThread = new Thread(communicate);
                ctThread.Start();
            }

            private void communicate()
            {

                StreamReader reader = new StreamReader(clientSocket.GetStream());

                while((true))
                {
                    try
                    {
                        if(!this.clientSocket.Connected)
                        {
                            Console.WriteLine("Client  disconnected.");
                            break;
                        }

                        NetworkStream stream = clientSocket.GetStream();
                        Byte[] data = new byte[400];
                        stream.Read(data, 0, data.Length);
                        

                        Console.WriteLine(System.Text.Encoding.UTF8.GetString(data, 0, data.Length));

                        string type = JObject.Parse(System.Text.Encoding.UTF8.GetString(data))["Type"].ToString();

                        Console.WriteLine("Request type: " + type);
                        string result = "";

                        switch(type)
                        {
                            case "methods":
                                networkStream = clientSocket.GetStream();
                                pc.GetClassesMethods();
                                Console.WriteLine(pc.ObjectToSend);
                                Byte[] bytes = Encoding.ASCII.GetBytes(pc.ObjectToSend.ToString());
                                sendResponse(bytes);
                                break;
                            case "invoke":
                                networkStream = clientSocket.GetStream();
                                JObject json = JObject.Parse(System.Text.Encoding.UTF8.GetString(data));
                                result = pc.invokeMethod(json);

                                JObject resultObj = serializeResult(result);
                                Byte[] resultBytes = Encoding.ASCII.GetBytes(resultObj.ToString());
                                sendResponse(resultBytes);

                                // serializacja i wysłanie wyniku
                                // odbiór wyniku
                                // dorobic ze 2 klasy
                                // posprzatac i na gita.
                                break;                        
                            default:
                                break;     
                        }


            
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" >> " + ex.ToString());
                    }
                }
            }

            public JObject serializeResult(string result)
            {
                // Result i value
                JObject obj = new JObject();
                obj["Result"] = result;
                return obj;
            }



            public void sendResponse(Byte[] bytes)
            {
                //Console.WriteLine(System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length));
                networkStream.Write(bytes, 0, bytes.Length);
                networkStream.Flush();
            }


        }




    }
    
}

