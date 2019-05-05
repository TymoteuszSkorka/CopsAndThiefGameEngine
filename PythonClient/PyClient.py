import socket
import sys
import json

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
# Bind the socket to the port
server_address = ('127.0.0.1', 13000)
print('starting up on %s port %s' % server_address)
sock.connect(server_address)
while(True):
    message = input("Enter the message: ")
    if message == "q":
        print('closing socket')
        sock.close()
        break
    else:
        print('sending "%s"' % message)
        sock.sendto(message.encode(), server_address)
        # Look for the response
        amount_received = 0
        amount_expected = len(message)

        while amount_received < amount_expected:
            data = sock.recv(1024)
            amount_received += len(data)
            print('received "%s"' % data)
            # obj = json.loads(data)
            # for i in obj:
            #     print(obj[i])



# byte[] MsgFromServer_hello = new byte[1024];
#                     int size_h = ClientSocket.Receive(MsgFromServer_hello);
#                     Console.WriteLine("Server: " + System.Text.Encoding.ASCII.GetString(MsgFromServer_hello, 0, size_h));
#                     string asciiString_h = Encoding.ASCII.GetString(MsgFromServer_hello, 0, MsgFromServer_hello.Length);


#                     string messageFromClient_HS = null;
#                     Console.Write(">");
#                     messageFromClient_HS = Console.ReadLine();
#                     ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient_HS),
#                         0, messageFromClient_HS.Length, SocketFlags.None);


#                     byte[] MsgFromServer_state = new byte[1024];
#                     int size_state = ClientSocket.Receive(MsgFromServer_state);
#                     string asciiString_state = Encoding.ASCII.GetString(MsgFromServer_state, 0, MsgFromServer_state.Length);

#                     while (string.Compare(asciiString_state, "already taken or input error, pick again:") == 0)
#                     {
#                         Console.WriteLine(asciiString_state);
#                         Console.Write(">");
#                         messageFromClient_HS = Console.ReadLine();
#                         ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient_HS),
#                             0, messageFromClient_HS.Length, SocketFlags.None);
#                         size_state = ClientSocket.Receive(MsgFromServer_state);
#                         asciiString_state = Encoding.ASCII.GetString(MsgFromServer_state, 0, size_state);
#                     }
#                     name = messageFromClient_HS;
#                     Console.WriteLine(asciiString_state);


#                     //dostajemy od serwera plansze
#                     byte[] board_state = new byte[1024];
#                     int board_state_Size = ClientSocket.Receive(board_state);
#                     Console.WriteLine(System.Text.Encoding.ASCII.GetString(board_state, 0, board_state_Size));

#                     //wysyłamy wiadomosc ze chcemy zaczac
#                     string GO_messageFromClient_HS = Console.ReadLine();
#                     ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(GO_messageFromClient_HS),
#                            0, GO_messageFromClient_HS.Length, SocketFlags.None);

#                     //oboje czekają na potwierdzenie od servera czy rozgrywka jest zaczęta
#                     byte[] start_flag = new byte[1024];
#                     int start_flag_size = ClientSocket.Receive(board_state);
#                     string flag_string = Encoding.ASCII.GetString(start_flag, 0, start_flag_size);

#                 }
# //end of handshake --------------------------------------------------------------------------------------------------------------------------

#                 //main loop
#                 while (true)
#                 {
#                     try
#                     {
#                         //w tym miejsu dodać wysyłanie JSONEM do serwera obliczonych zmian na planszy
#                         //
#                         //
#                         //
#                         //
#                         //
#                         //
                        
#                             System.Threading.Thread.Sleep(100);

#                         string messageFromClient = null;
#                         messageFromClient = name;
#                         ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient),
#                             0, messageFromClient.Length, SocketFlags.None);
                        
#                         byte[] MsgFromServer = new byte[1024];
#                         int size = ClientSocket.Receive(MsgFromServer);

#                         Console.WriteLine("Server answer: " + System.Text.Encoding.ASCII.GetString(MsgFromServer, 0, size));
#                         string asciiString = Encoding.ASCII.GetString(MsgFromServer, 0, MsgFromServer.Length);
#                         Board plansza = JsonConvert.DeserializeObject<Board>(asciiString);
                     
#                     }
#                     catch
#                     {
#                         if (Program.error_flag == false)
#                         {
#                             Console.WriteLine("connection failure");
#                             Program.error_flag = true;
#                         }
#                         ClientSocket.Close();
#                     }

#                 }
#             }
#             catch
#             {
#                 Console.WriteLine("server not found");
#                 Console.ReadLine();
#             }
        
#         }