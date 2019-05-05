import socket
import sys
import json

ClientSocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

name = None
error_flag = False

server_address = ('127.0.0.1', 13000)
print('starting up on %s port %s' % server_address)
ClientSocket.connect(server_address)


MsgFromServer_hello = ClientSocket.recv(1024)
print(MsgFromServer_hello)
messageFromClient_HS = input(">")
ClientSocket.sendto(messageFromClient_HS.encode(), server_address)

MsgFromServer_state = ClientSocket.recv(1024)

while str(MsgFromServer_state) == "b'already taken or input error, pick again:'":
    print(MsgFromServer_state)
    messageFromClient_HS = input(">")
    ClientSocket.sendto(messageFromClient_HS.encode(), server_address)
    MsgFromServer_state = ClientSocket.recv(1024)

name = messageFromClient_HS
print(MsgFromServer_state)
#ustawienia ogolne ozgrywki
board_state = ClientSocket.recv(1024)
print(board_state)
#wysylamy wiadomosc ze chcemy juz zaczac
messageFromClient_GO = input(">")
ClientSocket.sendto(messageFromClient_GO.encode(), server_address)
#czekanie na potwierdzenie serwera ze gra sie zaczela
rolled_board = ClientSocket.recv(1024)
print(rolled_board)

while True:
    #tutaj wstawic do jsona tablice z naszymi ruchami ktora wyslemy do servera
    messageFromClient = name
    ClientSocket.sendto(messageFromClient.encode(), server_address)
    MsgFromServer = ClientSocket.recv(1024)
    print("Server answer: " + str(MsgFromServer))
    MsgFromServer_moves = ClientSocket.recv(1024)
    print("Last five moves: " + str(MsgFromServer_moves))

